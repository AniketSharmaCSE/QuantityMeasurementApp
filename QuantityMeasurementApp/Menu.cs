using System;

namespace QuantityMeasurementApp
{
    public class Menu
    {
        public void Show()
        {
            Console.WriteLine("---- Quantity Measurement App ----");
            Console.WriteLine("1. Add Two Lengths");
            Console.WriteLine("2. Convert Length");
            Console.WriteLine("3. Check Equality");
            Console.WriteLine("4. Add Two Lengths With Target Unit");
            Console.WriteLine("5. Exit");
            Console.WriteLine("Enter your choice:");

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
                    Console.WriteLine("Exiting...");
                    break;

                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }

        // Reusable unit selection method
        private LengthUnit ReadUnit()
        {
            Console.WriteLine("Select Unit:");

            LengthUnit[] units = (LengthUnit[])Enum.GetValues(typeof(LengthUnit));

            for (int i = 0; i < units.Length; i++)
            {
                Console.WriteLine((i + 1) + ". " + units[i]);
            }

            int choice = Convert.ToInt32(Console.ReadLine());

            return units[choice - 1];
        }

        // UC1/UC2: Equality
        private void CheckEquality()
        {
            Console.WriteLine("Enter first value:");
            double value1 = Convert.ToDouble(Console.ReadLine());
            LengthUnit unit1 = ReadUnit();

            Console.WriteLine("Enter second value:");
            double value2 = Convert.ToDouble(Console.ReadLine());
            LengthUnit unit2 = ReadUnit();

            Quantity q1 = new Quantity(value1, unit1);
            Quantity q2 = new Quantity(value2, unit2);

            EqualityChecker checker = new EqualityChecker();

            Console.WriteLine("Are Equal: " + checker.CheckEquality(q1, q2));
        }

        // UC5: Conversion
        private void ConvertLength()
        {
            Console.WriteLine("Enter value:");
            double value = Convert.ToDouble(Console.ReadLine());
            LengthUnit source = ReadUnit();

            Console.WriteLine("Select target unit:");
            LengthUnit target = ReadUnit();

            double result = Quantity.Convert(value, source, target);

            Console.WriteLine("Converted Value: " + result + " " + target);
        }

        // UC6: Addition (result in first operand unit)
        private void AddLengths()
        {
            Console.WriteLine("Enter first value:");
            double value1 = Convert.ToDouble(Console.ReadLine());
            LengthUnit unit1 = ReadUnit();

            Console.WriteLine("Enter second value:");
            double value2 = Convert.ToDouble(Console.ReadLine());
            LengthUnit unit2 = ReadUnit();

            Quantity q1 = new Quantity(value1, unit1);
            Quantity q2 = new Quantity(value2, unit2);

            Quantity result = q1.Add(q2);

            Console.WriteLine("Result: " + result.GetValue() + " " + unit1);
        }

        // UC7: Addition with explicit target unit
        private void AddWithTargetUnit()
        {
            Console.WriteLine("Enter first value:");
            double value1 = Convert.ToDouble(Console.ReadLine());
            LengthUnit unit1 = ReadUnit();

            Console.WriteLine("Enter second value:");
            double value2 = Convert.ToDouble(Console.ReadLine());
            LengthUnit unit2 = ReadUnit();

            Console.WriteLine("Select target unit:");
            LengthUnit target = ReadUnit();

            Quantity q1 = new Quantity(value1, unit1);
            Quantity q2 = new Quantity(value2, unit2);

            Quantity result = q1.Add(q2, target);

            Console.WriteLine("Result: " + result.GetValue() + " " + target);
        }
    }
}