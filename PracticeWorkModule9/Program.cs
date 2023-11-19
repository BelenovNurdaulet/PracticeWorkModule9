using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticeWorkModule9
{

    abstract class Storage
    {
        private string name;
        private string model;

        public Storage(string name, string model)
        {
            this.name = name;
            this.model = model;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Model
        {
            get { return model; }
            set { model = value; }
        }

        public abstract double GetMemory();

        public abstract void CopyData(double dataSize);

        public abstract double GetFreeSpace();

        public abstract string GetDeviceInfo();
    }

    class Flash : Storage
    {
        private double usbSpeed;
        private double memorySize;

        public Flash(string name, string model, double usbSpeed, double memorySize)
            : base(name, model)
        {
            this.usbSpeed = usbSpeed;
            this.memorySize = memorySize;
        }

        public override double GetMemory()
        {
            return memorySize;
        }

        public override void CopyData(double dataSize)
        {
            double timeRequired = dataSize / usbSpeed;
            Console.WriteLine($"Копирование {dataSize} ГБ на Flash-накопитель. Время: {timeRequired} часа");
        }

        public override double GetFreeSpace()
        {
            // Предположим, что 10% Flash-накопителя зарезервировано для системных нужд
            return memorySize * 0.9;
        }

        public override string GetDeviceInfo()
        {
            return $"Flash-накопитель: {Name} - {Model}, Скорость USB: {usbSpeed} ГБ/с, Объем памяти: {memorySize} ГБ";
        }
    }

    class DVD : Storage
    {
        private double readWriteSpeed;
        private string type;

        public DVD(string name, string model, double readWriteSpeed, string type)
            : base(name, model)
        {
            this.readWriteSpeed = readWriteSpeed;
            this.type = type;
        }

        public override double GetMemory()
        {
            // Предположим, что 4.7 ГБ для одностороннего, 9 ГБ для двустороннего
            return type == "односторонний" ? 4.7 : 9;
        }

        public override void CopyData(double dataSize)
        {
            double timeRequired = dataSize / readWriteSpeed;
            Console.WriteLine($"Копирование {dataSize} ГБ на DVD-диск. Время: {timeRequired} часа");
        }

        public override double GetFreeSpace()
        {
            // DVD - только для чтения, поэтому свободного места всегда 0
            return 0;
        }

        public override string GetDeviceInfo()
        {
            return $"DVD-диск: {Name} - {Model}, Скорость чтения/записи: {readWriteSpeed} ГБ/с, Тип: {type}";
        }
    }

    class HDD : Storage
    {
        private double usbSpeed;
        private int partitions;
        private double partitionSize;

        public HDD(string name, string model, double usbSpeed, int partitions, double partitionSize)
            : base(name, model)
        {
            this.usbSpeed = usbSpeed;
            this.partitions = partitions;
            this.partitionSize = partitionSize;
        }

        public override double GetMemory()
        {
            return partitions * partitionSize;
        }

        public override void CopyData(double dataSize)
        {
            double timeRequired = dataSize / usbSpeed;
            Console.WriteLine($"Копирование {dataSize} ГБ на HDD. Время: {timeRequired} часа");
        }

        public override double GetFreeSpace()
        {
            // Предположим, что 5% HDD зарезервировано для системных нужд
            return GetMemory() * 0.95;
        }

        public override string GetDeviceInfo()
        {
            return $"HDD: {Name} - {Model}, Скорость USB: {usbSpeed} ГБ/с, Разделы: {partitions}, Объем раздела: {partitionSize} ГБ";
        }
    }

    class Program
    {
        static void Main()
        {
            Storage[] devices = new Storage[]
            {
            new Flash("FlashDrive1", "Model1", 3.0, 64),
            new DVD("DVDDrive1", "Model2", 2.0, "односторонний"),
            new HDD("HDD1", "Model3", 2.0, 2, 500)
            };

            while (true)
            {
                Console.WriteLine("Выберите опцию:");
                Console.WriteLine("1. Расчет общего количества памяти всех устройств");
                Console.WriteLine("2. Копирование информации на устройства");
                Console.WriteLine("3. Расчет времени необходимого для копирования");
                Console.WriteLine("4. Расчет необходимого количества носителей информации");
                Console.WriteLine("0. Выход");

                int choice;
                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Введите корректное число.");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        double totalMemory = 0;
                        foreach (var device in devices)
                        {
                            totalMemory += device.GetMemory();
                        }
                        Console.WriteLine($"Общий объем памяти всех устройств: {totalMemory} ГБ");
                        break;

                    case 2:
                        double dataSize = 565; // в ГБ
                        foreach (var device in devices)
                        {
                            device.CopyData(dataSize);
                        }
                        break;

                    case 3:
                        dataSize = 565;
                        double totalTime = 0;
                        foreach (var device in devices)
                        {
                            totalTime += dataSize / device.GetMemory(); // Предполагается константная скорость копирования
                        }
                        Console.WriteLine($"Общее необходимое время: {totalTime} часов");
                        break;

                    case 4:
                        dataSize = 565;
                        int devicesNeeded = (int)Math.Ceiling(dataSize / devices[0].GetFreeSpace());
                        Console.WriteLine($"Количество необходимых устройств: {devicesNeeded}");
                        break;

                    case 0:
                        Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine("Выбрана неверная опция. Пожалуйста, выберите снова.");
                        break;
                }
            }
        }
    }


}
