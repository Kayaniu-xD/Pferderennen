using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Pferderennen
{
    /// Repräsentiert ein einzelnes Pferd mit Namen und aktueller Position auf der Rennstrecke.
    class Pferd
    {
        public string Name { get; }
        public int Position { get; private set; }

        public Pferd(string name)
        {
            Name = name;
            Position = 0;
        }

        /// Bewegt das Pferd um eine zufällige Distanz zwischen minSchritt und maxSchritt (beide inklusive) vorwärts.
        public void BewegeVorwaerts(Random zufall, int minSchritt, int maxSchritt)
        {
            int schritt = zufall.Next(minSchritt, maxSchritt + 1); // Next() ist an der Obergrenze exklusiv
            Position += schritt;
        }
    }

    class Program
    {
        private const int Ziellinie = 50;      // Länge der Rennstrecke in Metern
        private const int MinSchritt = 1;      // kleinstmögliche Schrittweite pro Runde
        private const int MaxSchritt = 9;      // größtmögliche Schrittweite pro Runde
        private const int RundenPauseMs = 400; // kurze Pause zwischen den Runden, damit man mitlesen kann

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            List<Pferd> pferde = new List<Pferd>
            {
                new Pferd("Blitz"),
                new Pferd("Sturm"),
                new Pferd("Donner")
            };

            Random zufall = new Random();
            int runde = 0;
            Pferd gewinner = null;

            Console.WriteLine("=== Pferderennen ===");
            Console.WriteLine($"Ziellinie: {Ziellinie} Meter");
            Console.WriteLine($"Teilnehmer: {string.Join(", ", pferde.Select(p => p.Name))}");
            Console.WriteLine();

            while (gewinner == null)
            {
                runde++;
                Console.WriteLine($"Runde {runde}");
                Console.WriteLine();

                foreach (Pferd pferd in pferde)
                {
                    pferd.BewegeVorwaerts(zufall, MinSchritt, MaxSchritt);
                    Console.WriteLine($"{pferd.Name}: {pferd.Position} Meter");
                }

                Console.WriteLine();
                Console.WriteLine();

                // Alle Pferde ermitteln, die die Ziellinie in dieser Runde erreicht oder überschritten haben.
                // Kommen mehrere gleichzeitig ins Ziel, gewinnt das Pferd mit der größten Position.
                gewinner = pferde
                    .Where(p => p.Position >= Ziellinie)
                    .OrderByDescending(p => p.Position)
                    .FirstOrDefault();

                if (gewinner == null)
                {
                    Thread.Sleep(RundenPauseMs);
                }
            }

            Console.WriteLine("**********************************");
            Console.WriteLine($"  {gewinner.Name} gewinnt das Rennen!");
            Console.WriteLine("**********************************");
            Console.WriteLine();
            Console.WriteLine("Drücke eine beliebige Taste zum Beenden...");
            Console.ReadKey();
        }
    }
}
