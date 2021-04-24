using System;
using System.Threading;

namespace nuka
{

    class NukaReactor
    {

        static void Main(string[] args)
        {
            Boolean enabled = true;
            Reactor reactor = new Reactor(enabled);
            Thread KernelUp = new Thread(new ThreadStart(reactor.up));
            Thread KernelDown = new Thread(new ThreadStart(reactor.down));
            Thread CoreSpectator = new Thread(new ThreadStart(reactor.spectator));
            Thread CoreActivity = new Thread(new ThreadStart(reactor.activity));
            KernelUp.Start();
            KernelDown.Start();
            CoreSpectator.Start();
            CoreActivity.Start();
        }
    }

    class Reactor
    {
        private object locker = new object();
        Int32 temperature = 30;
        Int32 operatingtemp = 70;
        Boolean enabled = true;

        public Reactor(Boolean enabled) { this.enabled = enabled; }

        public void up()
        {
            while (enabled)
            {
                lock (locker)
                {
                    temperature++;
                    Console.WriteLine(temperature);
                }
                Thread.Sleep(100);
            }
        }

        public void down()
        {
            while (enabled || temperature > 0)
            {
                lock (locker)
                {
                    if (temperature > operatingtemp)
                    {
                        int m = Math.Abs(operatingtemp - temperature);
                        if (m < 10) temperature -= 1;
                        else if (m > 10 && m < 20) temperature -= 2;
                        else if (m > 20) temperature -= 3;
                    }
                }
                Thread.Sleep(100);
            }
        }
        public void spectator()
        {
            while (enabled || temperature > -1)
            {
                Console.WriteLine("Текущая температура" + temperature);
                if (temperature >= 100)
    
                {
                    Console.Clear();
                    Console.WriteLine("Перебор!");
                }
                else if (temperature >= 85 && temperature <= 99)
                {
                    Console.WriteLine("Внимание! Высокая температура!");
                }
                else if (temperature >= 1 && temperature <= 20)
                {
                    Console.WriteLine("Внимание! Низкая температура!");
                }
                else if (temperature <= 0)
                {
                    Console.Clear();
                    Console.WriteLine("Отключение!");
                }
                Thread.Sleep(1000);
            }
        }
        public void activity()
        {
            while (enabled)
            {
                var q = Console.ReadLine();
                var row = q;
                if (row.Equals("off")) Environment.Exit(0);
                else operatingtemp = int.Parse(row);
                Thread.Sleep(100);
            }
        }
    }
}