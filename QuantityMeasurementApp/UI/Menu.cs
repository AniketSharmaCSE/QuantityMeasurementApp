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
            Console.WriteLine("17. Exit");

            Console.Write("Enter choice: ");
            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 5: SubtractLengths(); break;
                case 6: DivideLengths(); break;
                case 10: SubtractWeights(); break;
                case 11: DivideWeights(); break;
                case 15: SubtractVolumes(); break;
                case 16: DivideVolumes(); break;
                case 17: Console.WriteLine("Exiting..."); break;
                default: Console.WriteLine("Other operations remain same as UC11"); break;
            }
        }

        private double ReadDouble(string msg)
        {
            Console.Write(msg);
            return Convert.ToDouble(Console.ReadLine());
        }

        private LengthUnit ReadLengthUnit()
        {
            LengthUnit[] units = (LengthUnit[])Enum.GetValues(typeof(LengthUnit));
            for (int i = 0; i < units.Length; i++)
            {
                Console.WriteLine((i + 1) + " " + units[i]);
            }

            int choice = Convert.ToInt32(Console.ReadLine());
            return units[choice - 1];
        }

        private WeightUnit ReadWeightUnit()
        {
            WeightUnit[] units = (WeightUnit[])Enum.GetValues(typeof(WeightUnit));
            for (int i = 0; i < units.Length; i++)
            {
                Console.WriteLine((i + 1) + " " + units[i]);
            }

            int choice = Convert.ToInt32(Console.ReadLine());
            return units[choice - 1];
        }

        private VolumeUnit ReadVolumeUnit()
        {
            VolumeUnit[] units = (VolumeUnit[])Enum.GetValues(typeof(VolumeUnit));
            for (int i = 0; i < units.Length; i++)
            {
                Console.WriteLine((i + 1) + " " + units[i]);
            }

            int choice = Convert.ToInt32(Console.ReadLine());
            return units[choice - 1];
        }

        private void SubtractLengths()
        {
            double v1 = ReadDouble("Enter first length: ");
            LengthUnit u1 = ReadLengthUnit();

            double v2 = ReadDouble("Enter second length: ");
            LengthUnit u2 = ReadLengthUnit();

            Quantity<LengthUnit> q1 = new Quantity<LengthUnit>(v1, u1);
            Quantity<LengthUnit> q2 = new Quantity<LengthUnit>(v2, u2);

            Quantity<LengthUnit> result = q1.Subtract(q2);

            Console.WriteLine("Result: " + result.GetValue() + " " + u1);
        }

        private void DivideLengths()
        {
            double v1 = ReadDouble("Enter first length: ");
            LengthUnit u1 = ReadLengthUnit();

            double v2 = ReadDouble("Enter second length: ");
            LengthUnit u2 = ReadLengthUnit();

            Quantity<LengthUnit> q1 = new Quantity<LengthUnit>(v1, u1);
            Quantity<LengthUnit> q2 = new Quantity<LengthUnit>(v2, u2);

            double result = q1.Divide(q2);

            Console.WriteLine("Result: " + result);
        }

        private void SubtractWeights()
        {
            double v1 = ReadDouble("Enter first weight: ");
            WeightUnit u1 = ReadWeightUnit();

            double v2 = ReadDouble("Enter second weight: ");
            WeightUnit u2 = ReadWeightUnit();

            Quantity<WeightUnit> q1 = new Quantity<WeightUnit>(v1, u1);
            Quantity<WeightUnit> q2 = new Quantity<WeightUnit>(v2, u2);

            Quantity<WeightUnit> result = q1.Subtract(q2);

            Console.WriteLine("Result: " + result.GetValue() + " " + u1);
        }

        private void DivideWeights()
        {
            double v1 = ReadDouble("Enter first weight: ");
            WeightUnit u1 = ReadWeightUnit();

            double v2 = ReadDouble("Enter second weight: ");
            WeightUnit u2 = ReadWeightUnit();

            Quantity<WeightUnit> q1 = new Quantity<WeightUnit>(v1, u1);
            Quantity<WeightUnit> q2 = new Quantity<WeightUnit>(v2, u2);

            double result = q1.Divide(q2);

            Console.WriteLine("Result: " + result);
        }

        private void SubtractVolumes()
        {
            double v1 = ReadDouble("Enter first volume: ");
            VolumeUnit u1 = ReadVolumeUnit();

            double v2 = ReadDouble("Enter second volume: ");
            VolumeUnit u2 = ReadVolumeUnit();

            Quantity<VolumeUnit> q1 = new Quantity<VolumeUnit>(v1, u1);
            Quantity<VolumeUnit> q2 = new Quantity<VolumeUnit>(v2, u2);

            Quantity<VolumeUnit> result = q1.Subtract(q2);

            Console.WriteLine("Result: " + result.GetValue() + " " + u1);
        }

        private void DivideVolumes()
        {
            double v1 = ReadDouble("Enter first volume: ");
            VolumeUnit u1 = ReadVolumeUnit();

            double v2 = ReadDouble("Enter second volume: ");
            VolumeUnit u2 = ReadVolumeUnit();

            Quantity<VolumeUnit> q1 = new Quantity<VolumeUnit>(v1, u1);
            Quantity<VolumeUnit> q2 = new Quantity<VolumeUnit>(v2, u2);

            double result = q1.Divide(q2);

            Console.WriteLine("Result: " + result);
        }
    }
}