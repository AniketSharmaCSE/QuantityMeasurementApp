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
            Console.WriteLine("4. Exit");

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
                    Console.WriteLine("Exiting...");
                    break;

                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }

        // UC1/UC2: Equality
        private void CheckEquality()
        {
            Console.WriteLine("Enter first value:");
            double value1 = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine("Enter first unit:");
            LengthUnit unit1 = (LengthUnit)Enum.Parse(typeof(LengthUnit), Console.ReadLine());

            Console.WriteLine("Enter second value:");
            double value2 = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine("Enter second unit:");
            LengthUnit unit2 = (LengthUnit)Enum.Parse(typeof(LengthUnit), Console.ReadLine());

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

            Console.WriteLine("Enter source unit (Feet, Inches, Yards, Centimeters):");
            LengthUnit source = (LengthUnit)Enum.Parse(typeof(LengthUnit), Console.ReadLine());

            Console.WriteLine("Enter target unit (Feet, Inches, Yards, Centimeters):");
            LengthUnit target = (LengthUnit)Enum.Parse(typeof(LengthUnit), Console.ReadLine());

            double result = Quantity.Convert(value, source, target);

            Console.WriteLine("Converted Value: " + result + " " + target);
        }

        // UC6: Addition
        private void AddLengths()
        {
            Console.WriteLine("Enter first value:");
            double value1 = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine("Enter first unit (Feet, Inches, Yards, Centimeters):");
            LengthUnit unit1 = (LengthUnit)Enum.Parse(typeof(LengthUnit), Console.ReadLine());

            Console.WriteLine("Enter second value:");
            double value2 = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine("Enter second unit (Feet, Inches, Yards, Centimeters):");
            LengthUnit unit2 = (LengthUnit)Enum.Parse(typeof(LengthUnit), Console.ReadLine());

            Quantity q1 = new Quantity(value1, unit1);
            Quantity q2 = new Quantity(value2, unit2);

            Quantity result = q1.Add(q2);

            Console.WriteLine("Result: " + result.GetValue() + " " + unit1);
        }

        }
}
