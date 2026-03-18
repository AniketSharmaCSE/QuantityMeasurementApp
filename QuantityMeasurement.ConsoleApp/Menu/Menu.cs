using QuantityMeasurement.Repository;
using QuantityMeasurement.Repository.Database;
using QuantityMeasurement.Repository.Interfaces;
using QuantityMeasurement.Repository.Util;
using QuantityMeasurement.Model.Units;
using QuantityMeasurement.BusinessLayer.Services;
using QuantityMeasurement.BusinessLayer.Controllers;
using QuantityMeasurement.BusinessLayer.Interfaces;

namespace QuantityMeasurement.ConsoleApp.UI
{
    public class Menu : IMenu
    {
        private readonly QuantityController _controller;

        // kept as a field so the history menu options can call repo methods directly
        private readonly IQuantityMeasurementRepository _repo;

        public Menu()
        {
            AppConfig config = AppConfig.Instance;

            // pick the repo based on appsettings.json – swap "cache" for "database" to use SQL Server
            if (config.RepositoryType.Equals("database", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("[Menu] Using SQL Server database repository.");
                _repo = new QuantityMeasurementDatabaseRepository(config);
            }
            else
            {
                Console.WriteLine("[Menu] Using in-memory cache repository.");
                _repo = new QuantityMeasurementCacheRepository();
            }

            IQuantityService service = new QuantityService(_repo);
            _controller = new QuantityController(service);
        }

        public void Show()
        {
            while (true)
            {
                Console.WriteLine("---- Quantity Measurement App ----");
                Console.WriteLine("1. Add Two Lengths");
                Console.WriteLine("2. Convert Length");
                Console.WriteLine("3. Check Length Equality");
                Console.WriteLine("4. Add Two Lengths With Target Unit");
                Console.WriteLine("5. Subtract Lengths");
                Console.WriteLine("6. Divide Lengths");
                Console.WriteLine("7. Add Two Weights");
                Console.WriteLine("8. Convert Weight");
                Console.WriteLine("9. Check Weight Equality");
                Console.WriteLine("10. Subtract Weights");
                Console.WriteLine("11. Divide Weights");
                Console.WriteLine("12. Add Two Volumes");
                Console.WriteLine("13. Convert Volume");
                Console.WriteLine("14. Check Volume Equality");
                Console.WriteLine("15. Subtract Volumes");
                Console.WriteLine("16. Divide Volumes");
                Console.WriteLine("17. Convert Temperature");
                Console.WriteLine("18. Check Temperature Equality");
                Console.WriteLine("19. Exit");
                Console.WriteLine("--- History ---");
                Console.WriteLine("20. View Full Operation History");
                Console.WriteLine("21. View History by Operation Type");
                Console.WriteLine("22. View History by Category");
                Console.WriteLine("23. Show Repository Statistics");
                Console.WriteLine("24. Delete All History Records");

                Console.Write("Enter choice: ");
                int choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:  AddLength();              break;
                    case 2:  ConvertLength();          break;
                    case 3:  CheckLengthEquality();    break;
                    case 4:  AddLengthsWithTarget();   break;
                    case 5:  SubtractLengths();        break;
                    case 6:  DivideLengths();          break;
                    case 7:  AddWeights();             break;
                    case 8:  ConvertWeight();          break;
                    case 9:  CheckWeightEquality();    break;
                    case 10: SubtractWeights();        break;
                    case 11: DivideWeights();          break;
                    case 12: AddVolumes();             break;
                    case 13: ConvertVolume();          break;
                    case 14: CheckVolumeEquality();    break;
                    case 15: SubtractVolumes();        break;
                    case 16: DivideVolumes();          break;
                    case 17: ConvertTemperature();     break;
                    case 18: CheckTemperatureEquality(); break;
                    case 19:
                        _repo.ReleaseResources();
                        Console.WriteLine("Exiting...");
                        return;
                    case 20: ViewFullHistory();        break;
                    case 21: ViewByOperation();        break;
                    case 22: ViewByCategory();         break;
                    case 23: ShowRepoStatistics();     break;
                    case 24: DeleteAllRecords();       break;
                    default: Console.WriteLine("Invalid choice"); break;
                }

                Console.WriteLine();
            }
        }

        private void ViewFullHistory()
        {
            var all = _repo.GetAll();
            Console.WriteLine($"\nFull History ({all.Count} record(s)):");
            if (all.Count == 0) { Console.WriteLine("No records found."); return; }
            foreach (var r in all)
                Console.WriteLine("  " + r);
        }

        private void ViewByOperation()
        {
            Console.Write("Enter operation name (e.g. Add, Convert, Compare): ");
            string op = Console.ReadLine() ?? "";
            var results = _repo.GetByOperation(op);
            Console.WriteLine($"\nRecords for '{op}' ({results.Count} found):");
            if (results.Count == 0) { Console.WriteLine("No matching records."); return; }
            foreach (var r in results)
                Console.WriteLine("  " + r);
        }

        private void ViewByCategory()
        {
            Console.Write("Enter category (Length, Weight, Volume, Temperature): ");
            string cat = Console.ReadLine() ?? "";
            var results = _repo.GetByCategory(cat);
            Console.WriteLine($"\nRecords for category '{cat}' ({results.Count} found):");
            if (results.Count == 0) { Console.WriteLine("No matching records."); return; }
            foreach (var r in results)
                Console.WriteLine("  " + r);
        }

        private void ShowRepoStatistics()
        {
            Console.WriteLine($"\nTotal stored records : {_repo.GetTotalCount()}");
            Console.WriteLine($"Pool status          : {_repo.GetPoolStatistics()}");
        }

        private void DeleteAllRecords()
        {
            Console.Write("Delete ALL history records? (yes/no): ");
            string confirm = Console.ReadLine() ?? "";
            if (confirm.Equals("yes", StringComparison.OrdinalIgnoreCase))
            {
                _repo.Clear();
                Console.WriteLine("All records deleted.");
            }
            else
            {
                Console.WriteLine("Cancelled.");
            }
        }

        private double ReadDouble(string msg)
        {
            Console.Write(msg);
            return Convert.ToDouble(Console.ReadLine());
        }

        private LengthUnit ReadLengthUnit()
        {
            var units = (LengthUnit[])Enum.GetValues(typeof(LengthUnit));
            for (int i = 0; i < units.Length; i++)
                Console.WriteLine((i + 1) + " " + units[i]);
            return units[Convert.ToInt32(Console.ReadLine()) - 1];
        }

        private WeightUnit ReadWeightUnit()
        {
            var units = (WeightUnit[])Enum.GetValues(typeof(WeightUnit));
            for (int i = 0; i < units.Length; i++)
                Console.WriteLine((i + 1) + " " + units[i]);
            return units[Convert.ToInt32(Console.ReadLine()) - 1];
        }

        private VolumeUnit ReadVolumeUnit()
        {
            var units = (VolumeUnit[])Enum.GetValues(typeof(VolumeUnit));
            for (int i = 0; i < units.Length; i++)
                Console.WriteLine((i + 1) + " " + units[i]);
            return units[Convert.ToInt32(Console.ReadLine()) - 1];
        }

        private TemperatureUnit ReadTemperatureUnit()
        {
            var units = new[] { TemperatureUnit.Celsius, TemperatureUnit.Fahrenheit, TemperatureUnit.Kelvin };
            for (int i = 0; i < units.Length; i++)
                Console.WriteLine((i + 1) + " " + units[i]);
            return units[Convert.ToInt32(Console.ReadLine()) - 1];
        }

        // LENGTH

        private void AddLength()
        {
            Console.WriteLine(_controller.AddLength(
                ReadDouble("Enter first length: "),  ReadLengthUnit().ToString(),
                ReadDouble("Enter second length: "), ReadLengthUnit().ToString()));
        }

        private void ConvertLength()
        {
            Console.WriteLine(_controller.ConvertLength(
                ReadDouble("Enter length: "),
                ReadLengthUnit().ToString(),
                ReadLengthUnit().ToString()));
        }

        private void CheckLengthEquality()
        {
            Console.WriteLine(_controller.CompareLength(
                ReadDouble("Enter first length: "),  ReadLengthUnit().ToString(),
                ReadDouble("Enter second length: "), ReadLengthUnit().ToString()));
        }

        private void AddLengthsWithTarget()
        {
            Console.WriteLine(_controller.AddLengthWithTarget(
                ReadDouble("Enter first length: "),  ReadLengthUnit().ToString(),
                ReadDouble("Enter second length: "), ReadLengthUnit().ToString(),
                ReadLengthUnit().ToString()));
        }

        private void SubtractLengths()
        {
            Console.WriteLine(_controller.SubtractLength(
                ReadDouble("Enter first length: "),  ReadLengthUnit().ToString(),
                ReadDouble("Enter second length: "), ReadLengthUnit().ToString()));
        }

        private void DivideLengths()
        {
            Console.WriteLine(_controller.DivideLength(
                ReadDouble("Enter first length: "),  ReadLengthUnit().ToString(),
                ReadDouble("Enter second length: "), ReadLengthUnit().ToString()));
        }

        // WEIGHT

        private void AddWeights()
        {
            Console.WriteLine(_controller.AddWeight(
                ReadDouble("Enter first weight: "),  ReadWeightUnit().ToString(),
                ReadDouble("Enter second weight: "), ReadWeightUnit().ToString()));
        }

        private void ConvertWeight()
        {
            Console.WriteLine(_controller.ConvertWeight(
                ReadDouble("Enter weight: "),
                ReadWeightUnit().ToString(),
                ReadWeightUnit().ToString()));
        }

        private void CheckWeightEquality()
        {
            Console.WriteLine(_controller.CompareWeight(
                ReadDouble("Enter first weight: "),  ReadWeightUnit().ToString(),
                ReadDouble("Enter second weight: "), ReadWeightUnit().ToString()));
        }

        private void SubtractWeights()
        {
            Console.WriteLine(_controller.SubtractWeight(
                ReadDouble("Enter first weight: "),  ReadWeightUnit().ToString(),
                ReadDouble("Enter second weight: "), ReadWeightUnit().ToString()));
        }

        private void DivideWeights()
        {
            Console.WriteLine(_controller.DivideWeight(
                ReadDouble("Enter first weight: "),  ReadWeightUnit().ToString(),
                ReadDouble("Enter second weight: "), ReadWeightUnit().ToString()));
        }

        // VOLUME

        private void AddVolumes()
        {
            Console.WriteLine(_controller.AddVolume(
                ReadDouble("Enter first volume: "),  ReadVolumeUnit().ToString(),
                ReadDouble("Enter second volume: "), ReadVolumeUnit().ToString()));
        }

        private void ConvertVolume()
        {
            Console.WriteLine(_controller.ConvertVolume(
                ReadDouble("Enter volume: "),
                ReadVolumeUnit().ToString(),
                ReadVolumeUnit().ToString()));
        }

        private void CheckVolumeEquality()
        {
            Console.WriteLine(_controller.CompareVolume(
                ReadDouble("Enter first volume: "),  ReadVolumeUnit().ToString(),
                ReadDouble("Enter second volume: "), ReadVolumeUnit().ToString()));
        }

        private void SubtractVolumes()
        {
            Console.WriteLine(_controller.SubtractVolume(
                ReadDouble("Enter first volume: "),  ReadVolumeUnit().ToString(),
                ReadDouble("Enter second volume: "), ReadVolumeUnit().ToString()));
        }

        private void DivideVolumes()
        {
            Console.WriteLine(_controller.DivideVolume(
                ReadDouble("Enter first volume: "),  ReadVolumeUnit().ToString(),
                ReadDouble("Enter second volume: "), ReadVolumeUnit().ToString()));
        }

        // TEMPERATURE

        private void ConvertTemperature()
        {
            Console.WriteLine(_controller.ConvertTemperature(
                ReadDouble("Enter temperature: "),
                ReadTemperatureUnit().ToString(),
                ReadTemperatureUnit().ToString()));
        }

        private void CheckTemperatureEquality()
        {
            Console.WriteLine(_controller.CompareTemperature(
                ReadDouble("Enter first temperature: "),  ReadTemperatureUnit().ToString(),
                ReadDouble("Enter second temperature: "), ReadTemperatureUnit().ToString()));
        }
    }
}
