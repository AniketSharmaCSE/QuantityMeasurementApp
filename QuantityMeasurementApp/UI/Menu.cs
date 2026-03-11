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

            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    AddLengths();
                    break;

                case 2:
                    ConvertLength();
                    break;

                case 3:
                    CheckEquality();
                    break;

                case 4:
                    AddWithTargetUnit();
                    break;

                case 5:
                    AddWeights();
                    break;

                case 6:
                    ConvertWeight();
                    break;

                case 7:
                    CheckWeightEquality();
                    break;

                case 8:
                    Console.WriteLine("Exiting...");
                    break;

                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }

        private LengthUnit ReadUnit()
        {
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
            double v1 = Convert.ToDouble(Console.ReadLine());
            LengthUnit u1 = ReadUnit();

            double v2 = Convert.ToDouble(Console.ReadLine());
            LengthUnit u2 = ReadUnit();

            Quantity q1 = new Quantity(v1, u1);
            Quantity q2 = new Quantity(v2, u2);

            Quantity result = q1.Add(q2);

            Console.WriteLine(result.GetValue());
        }

        private void ConvertLength()
        {
            double value = Convert.ToDouble(Console.ReadLine());
            LengthUnit source = ReadUnit();
            LengthUnit target = ReadUnit();

            double result = Quantity.Convert(value, source, target);

            Console.WriteLine(result);
        }

        private void CheckEquality()
        {
            double v1 = Convert.ToDouble(Console.ReadLine());
            LengthUnit u1 = ReadUnit();

            double v2 = Convert.ToDouble(Console.ReadLine());
            LengthUnit u2 = ReadUnit();

            Quantity q1 = new Quantity(v1, u1);
            Quantity q2 = new Quantity(v2, u2);

            EqualityChecker checker = new EqualityChecker();

            Console.WriteLine(checker.CheckEquality(q1, q2));
        }

        private void AddWithTargetUnit()
        {
            double v1 = Convert.ToDouble(Console.ReadLine());
            LengthUnit u1 = ReadUnit();

            double v2 = Convert.ToDouble(Console.ReadLine());
            LengthUnit u2 = ReadUnit();

            LengthUnit target = ReadUnit();

            Quantity q1 = new Quantity(v1, u1);
            Quantity q2 = new Quantity(v2, u2);

            Quantity result = q1.Add(q2, target);

            Console.WriteLine(result.GetValue());
        }

        private void AddWeights()
        {
            double v1 = Convert.ToDouble(Console.ReadLine());
            WeightUnit u1 = ReadWeightUnit();

            double v2 = Convert.ToDouble(Console.ReadLine());
            WeightUnit u2 = ReadWeightUnit();

            QuantityWeight q1 = new QuantityWeight(v1, u1);
            QuantityWeight q2 = new QuantityWeight(v2, u2);

            QuantityWeight result = q1.Add(q2);

            Console.WriteLine(result.GetValue());
        }

        private void ConvertWeight()
        {
            double value = Convert.ToDouble(Console.ReadLine());
            WeightUnit source = ReadWeightUnit();
            WeightUnit target = ReadWeightUnit();

            QuantityWeight q = new QuantityWeight(value, source);

            QuantityWeight result = q.ConvertTo(target);

            Console.WriteLine(result.GetValue());
        }

        private void CheckWeightEquality()
        {
            double v1 = Convert.ToDouble(Console.ReadLine());
            WeightUnit u1 = ReadWeightUnit();

            double v2 = Convert.ToDouble(Console.ReadLine());
            WeightUnit u2 = ReadWeightUnit();

            QuantityWeight q1 = new QuantityWeight(v1, u1);
            QuantityWeight q2 = new QuantityWeight(v2, u2);

            Console.WriteLine(q1.Equals(q2));
        }
    }
}