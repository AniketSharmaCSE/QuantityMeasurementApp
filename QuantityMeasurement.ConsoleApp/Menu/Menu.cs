using QuantityMeasurement.Repository;
using QuantityMeasurement.Repository.Interfaces;
using QuantityMeasurement.Model.Units;
using QuantityMeasurement.BusinessLayer.Services;
using QuantityMeasurement.BusinessLayer.Controllers;
using QuantityMeasurement.BusinessLayer.Interfaces;

namespace QuantityMeasurement.ConsoleApp.UI
{
    public class Menu : IMenu
    {
        private readonly QuantityController controller;

        public Menu()
        {
            IQuantityMeasurementRepository repo = new QuantityMeasurementCacheRepository();
            IQuantityService service = new QuantityService(repo);
            controller = new QuantityController(service);
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

                Console.Write("Enter choice: ");
                int choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1: AddLength(); break;
                    case 2: ConvertLength(); break;
                    case 3: CheckLengthEquality(); break;
                    case 4: AddLengthsWithTarget(); break;
                    case 5: SubtractLengths(); break;
                    case 6: DivideLengths(); break;
                    case 7: AddWeights(); break;
                    case 8: ConvertWeight(); break;
                    case 9: CheckWeightEquality(); break;
                    case 10: SubtractWeights(); break;
                    case 11: DivideWeights(); break;
                    case 12: AddVolumes(); break;
                    case 13: ConvertVolume(); break;
                    case 14: CheckVolumeEquality(); break;
                    case 15: SubtractVolumes(); break;
                    case 16: DivideVolumes(); break;
                    case 17: ConvertTemperature(); break;
                    case 18: CheckTemperatureEquality(); break;
                    case 19: Console.WriteLine("Exiting..."); return;
                    default: Console.WriteLine("Invalid choice"); break;
                }

                Console.WriteLine();
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
            var units = new[]
            {
                TemperatureUnit.Celsius,
                TemperatureUnit.Fahrenheit,
                TemperatureUnit.Kelvin
            };

            for (int i = 0; i < units.Length; i++)
                Console.WriteLine((i + 1) + " " + units[i]);

            return units[Convert.ToInt32(Console.ReadLine()) - 1];
        }

        // LENGTH 

        private void AddLength()
        {
            var response = controller.AddLength(
                ReadDouble("Enter first length: "),
                ReadLengthUnit().ToString(),
                ReadDouble("Enter second length: "),
                ReadLengthUnit().ToString()
            );

            Console.WriteLine(response);
        }

        private void ConvertLength()
        {
            var response = controller.ConvertLength(
                ReadDouble("Enter length: "),
                ReadLengthUnit().ToString(),
                ReadLengthUnit().ToString()
            );

            Console.WriteLine(response);
        }

        private void CheckLengthEquality()
        {
            var response = controller.CompareLength(
                ReadDouble("Enter first length: "),
                ReadLengthUnit().ToString(),
                ReadDouble("Enter second length: "),
                ReadLengthUnit().ToString()
            );

            Console.WriteLine(response);
        }

        private void AddLengthsWithTarget()
        {
            var response = controller.AddLengthWithTarget(
                ReadDouble("Enter first length: "),
                ReadLengthUnit().ToString(),
                ReadDouble("Enter second length: "),
                ReadLengthUnit().ToString(),
                ReadLengthUnit().ToString()
            );

            Console.WriteLine(response);
        }

        private void SubtractLengths()
        {
            var response = controller.SubtractLength(
                ReadDouble("Enter first length: "),
                ReadLengthUnit().ToString(),
                ReadDouble("Enter second length: "),
                ReadLengthUnit().ToString()
            );

            Console.WriteLine(response);
        }

        private void DivideLengths()
        {
            var response = controller.DivideLength(
                ReadDouble("Enter first length: "),
                ReadLengthUnit().ToString(),
                ReadDouble("Enter second length: "),
                ReadLengthUnit().ToString()
            );

            Console.WriteLine(response);
        }

        // WEIGHT

        private void AddWeights()
        {
            Console.WriteLine(controller.AddWeight(
                ReadDouble("Enter first weight: "),
                ReadWeightUnit().ToString(),
                ReadDouble("Enter second weight: "),
                ReadWeightUnit().ToString()
            ));
        }

        private void ConvertWeight()
        {
            Console.WriteLine(controller.ConvertWeight(
                ReadDouble("Enter weight: "),
                ReadWeightUnit().ToString(),
                ReadWeightUnit().ToString()
            ));
        }

        private void CheckWeightEquality()
        {
            Console.WriteLine(controller.CompareWeight(
                ReadDouble("Enter first weight: "),
                ReadWeightUnit().ToString(),
                ReadDouble("Enter second weight: "),
                ReadWeightUnit().ToString()
            ));
        }

        private void SubtractWeights()
        {
            Console.WriteLine(controller.SubtractWeight(
                ReadDouble("Enter first weight: "),
                ReadWeightUnit().ToString(),
                ReadDouble("Enter second weight: "),
                ReadWeightUnit().ToString()
            ));
        }

        private void DivideWeights()
        {
            Console.WriteLine(controller.DivideWeight(
                ReadDouble("Enter first weight: "),
                ReadWeightUnit().ToString(),
                ReadDouble("Enter second weight: "),
                ReadWeightUnit().ToString()
            ));
        }

        // VOLUME

        private void AddVolumes()
        {
            Console.WriteLine(controller.AddVolume(
                ReadDouble("Enter first volume: "),
                ReadVolumeUnit().ToString(),
                ReadDouble("Enter second volume: "),
                ReadVolumeUnit().ToString()
            ));
        }

        private void ConvertVolume()
        {
            Console.WriteLine(controller.ConvertVolume(
                ReadDouble("Enter volume: "),
                ReadVolumeUnit().ToString(),
                ReadVolumeUnit().ToString()
            ));
        }

        private void CheckVolumeEquality()
        {
            Console.WriteLine(controller.CompareVolume(
                ReadDouble("Enter first volume: "),
                ReadVolumeUnit().ToString(),
                ReadDouble("Enter second volume: "),
                ReadVolumeUnit().ToString()
            ));
        }

        private void SubtractVolumes()
        {
            Console.WriteLine(controller.SubtractVolume(
                ReadDouble("Enter first volume: "),
                ReadVolumeUnit().ToString(),
                ReadDouble("Enter second volume: "),
                ReadVolumeUnit().ToString()
            ));
        }

        private void DivideVolumes()
        {
            Console.WriteLine(controller.DivideVolume(
                ReadDouble("Enter first volume: "),
                ReadVolumeUnit().ToString(),
                ReadDouble("Enter second volume: "),
                ReadVolumeUnit().ToString()
            ));
        }

        // TEMPERATURE

        private void ConvertTemperature()
        {
            Console.WriteLine(controller.ConvertTemperature(
                ReadDouble("Enter temperature: "),
                ReadTemperatureUnit().ToString(),
                ReadTemperatureUnit().ToString()
            ));
        }

        private void CheckTemperatureEquality()
        {
            Console.WriteLine(controller.CompareTemperature(
                ReadDouble("Enter first temperature: "),
                ReadTemperatureUnit().ToString(),
                ReadDouble("Enter second temperature: "),
                ReadTemperatureUnit().ToString()
            ));
        }
    }
}