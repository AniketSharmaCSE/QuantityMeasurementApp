using System;
using System.Collections.Concurrent;
using System.Data;
using System.Threading;
using Microsoft.Data.SqlClient;

namespace QuantityMeasurement.Repository.Util
{
    public sealed class ConnectionPool : IDisposable
    {
        private readonly string _connectionString;
        private readonly int _maxSize;
        private readonly int _timeoutMs;

        // idle connections waiting to be handed out
        private readonly ConcurrentBag<SqlConnection> _idle = new();

        // total connections ever created (idle + currently in use)
        private int _totalCreated;

        // each Acquire() takes one permit; each Release() gives one back
        // this is what enforces the max pool size limit
        private readonly SemaphoreSlim _semaphore;

        private bool _disposed;

        public ConnectionPool(AppConfig config)
        {
            _connectionString = config.ConnectionString;
            _maxSize          = config.MaxPoolSize;
            _timeoutMs        = config.ConnectionTimeoutSeconds * 1000;

            _semaphore = new SemaphoreSlim(_maxSize, _maxSize);

            // open the minimum connections upfront so the first few requests don't pay the connection cost
            for (int i = 0; i < config.MinPoolSize; i++)
                _idle.Add(CreateConnection());

            Console.WriteLine($"[ConnectionPool] Initialized. Min={config.MinPoolSize}, Max={_maxSize}");
        }

        public SqlConnection Acquire()
        {
            // blocks if all connections are checked out, throws after timeout
            if (!_semaphore.Wait(_timeoutMs))
                throw new InvalidOperationException(
                    $"[ConnectionPool] Timed out after {_timeoutMs / 1000}s waiting for a free connection. Pool size: {_maxSize}");

            if (_idle.TryTake(out SqlConnection? conn))
            {
                EnsureOpen(conn);
                return conn;
            }

            return CreateConnection();
        }

        public void Release(SqlConnection connection)
        {
            if (_disposed) { connection.Dispose(); return; }

            if (connection.State == ConnectionState.Open)
            {
                _idle.Add(connection);
            }
            else
            {
                // connection dropped while idle – replace it so pool size stays stable
                connection.Dispose();
                try { _idle.Add(CreateConnection()); }
                catch { /* if we can't reconnect right now just let the pool shrink */ }
            }

            _semaphore.Release();
        }

        public string GetStatistics()
        {
            int idle   = _idle.Count;
            int active = _totalCreated - idle;
            return $"ConnectionPool {{ Total={_totalCreated}, Idle={idle}, Active={active}, Max={_maxSize} }}";
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            while (_idle.TryTake(out SqlConnection? c))
                c.Dispose();

            _semaphore.Dispose();
            Console.WriteLine("[ConnectionPool] Disposed all connections closed.");
        }

        private SqlConnection CreateConnection()
        {
            var conn = new SqlConnection(_connectionString);
            conn.Open();
            Interlocked.Increment(ref _totalCreated);
            return conn;
        }

        private static void EnsureOpen(SqlConnection conn)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
        }
    }
}
