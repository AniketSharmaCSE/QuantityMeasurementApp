using System;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Units;
using QuantityMeasurementApp.Services;

namespace QuantityMeasurementApp.UI
{
    public class Menu
    {
        public void Show()
        {
            Console.WriteLine("---- Quantity Measurement App ----");

            Console.WriteLine("1. Add Two Lengths");
            Console.WriteLine("2. Convert Length");
            Console.WriteLine("3. Check Length Equality");
            Console.WriteLine("4. Add Two Lengths With Target Unit");
            Console.WriteLine("5. Add Two Weights");
            Console.WriteLine("6. Convert Weight");
            Console.WriteLine("7. Check Weight Equality");
            Console.WriteLine("8. Exit");

            Console.Write("Enter your choice: ");
            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1: AddLengths(); break;
                case 2: ConvertLength(); break;
                case 3: CheckEquality(); break;
                case 4: AddWithTargetUnit(); break;
                case 5: AddWeights(); break;
                case 6: ConvertWeight(); break;
                case 7: CheckWeightEquality(); break;
                case 8: Console.WriteLine("Exiting..."); break;
                default: Console.WriteLine("Invalid choice."); break;
            }
        }

        private double ReadDouble(string message)
        {
            Console.Write(message);
            double value;

            while (!double.TryParse(Console.ReadLine(), out value))
            {
                Console.Write("Invalid number. Try again: ");
            }

            return value;
        }

        private LengthUnit ReadUnit()
        {
            Console.WriteLine("Select Length Unit:");
            LengthUnit[] units = (LengthUnit[])Enum.GetValues(typeof(LengthUnit));

            for (int i = 0; i < units.Length; i++)
            {
                Console.WriteLine((i + 1) + ". " + units[i]);
            }

            int choice = Convert.ToInt32(Console.ReadLine());
            return units[choice - 1];
        }

        private WeightUnit ReadWeightUnit()
        {
            Console.WriteLine("Select Weight Unit:");
            WeightUnit[] units = (WeightUnit[])Enum.GetValues(typeof(WeightUnit));

            for (int i = 0; i < units.Length; i++)
            {
                Console.WriteLine((i + 1) + ". " + units[i]);
            }

            int choice = Convert.ToInt32(Console.ReadLine());
            return units[choice - 1];
        }

        private void AddLengths()
        {
            double v1 = ReadDouble("Enter first length value: ");
            LengthUnit u1 = ReadUnit();

            double v2 = ReadDouble("Enter second length value: ");
            LengthUnit u2 = ReadUnit();

            Quantity<LengthUnit> q1 = new Quantity<LengthUnit>(v1, u1);
            Quantity<LengthUnit> q2 = new Quantity<LengthUnit>(v2, u2);

            Quantity<LengthUnit> result = q1.Add(q2);

            Console.WriteLine($"Result: {result.GetValue()} {u1}");
        }

        private void ConvertLength()
        {
            double value = ReadDouble("Enter length value: ");
            LengthUnit source = ReadUnit();
            LengthUnit target = ReadUnit();

            Quantity<LengthUnit> q = new Quantity<LengthUnit>(value, source);

            Quantity<LengthUnit> result = q.ConvertTo(target);

            Console.WriteLine($"Converted Value: {result.GetValue()} {target}");
        }

        private void CheckEquality()
        {
            double v1 = ReadDouble("Enter first length value: ");
            LengthUnit u1 = ReadUnit();

            double v2 = ReadDouble("Enter second length value: ");
            LengthUnit u2 = ReadUnit();

            Quantity<LengthUnit> q1 = new Quantity<LengthUnit>(v1, u1);
            Quantity<LengthUnit> q2 = new Quantity<LengthUnit>(v2, u2);

            EqualityChecker checker = new EqualityChecker();

            Console.WriteLine("Are Equal: " + checker.CheckEquality(q1, q2));
        }

        private void AddWithTargetUnit()
        {
            double v1 = ReadDouble("Enter first length value: ");
            LengthUnit u1 = ReadUnit();

            double v2 = ReadDouble("Enter second length value: ");
            LengthUnit u2 = ReadUnit();

            LengthUnit target = ReadUnit();

            Quantity<LengthUnit> q1 = new Quantity<LengthUnit>(v1, u1);
            Quantity<LengthUnit> q2 = new Quantity<LengthUnit>(v2, u2);

            Quantity<LengthUnit> result = q1.Add(q2, target);

            Console.WriteLine($"Result: {result.GetValue()} {target}");
        }

        private void AddWeights()
        {
            double v1 = ReadDouble("Enter first weight value: ");
            WeightUnit u1 = ReadWeightUnit();

            double v2 = ReadDouble("Enter second weight value: ");
            WeightUnit u2 = ReadWeightUnit();

            Quantity<WeightUnit> q1 = new Quantity<WeightUnit>(v1, u1);
            Quantity<WeightUnit> q2 = new Quantity<WeightUnit>(v2, u2);

            Quantity<WeightUnit> result = q1.Add(q2);

            Console.WriteLine($"Result: {result.GetValue()} {u1}");
        }

        private void ConvertWeight()
        {
            double value = ReadDouble("Enter weight value: ");
            WeightUnit source = ReadWeightUnit();
            WeightUnit target = ReadWeightUnit();

            Quantity<WeightUnit> q = new Quantity<WeightUnit>(value, source);

            Quantity<WeightUnit> result = q.ConvertTo(target);

            Console.WriteLine($"Converted Value: {result.GetValue()} {target}");
        }

        private void CheckWeightEquality()
        {
            double v1 = ReadDouble("Enter first weight value: ");
            WeightUnit u1 = ReadWeightUnit();

            double v2 = ReadDouble("Enter second weight value: ");
            WeightUnit u2 = ReadWeightUnit();

            Quantity<WeightUnit> q1 = new Quantity<WeightUnit>(v1, u1);
            Quantity<WeightUnit> q2 = new Quantity<WeightUnit>(v2, u2);

            Console.WriteLine("Are Equal: " + q1.Equals(q2));
        }
    }
}