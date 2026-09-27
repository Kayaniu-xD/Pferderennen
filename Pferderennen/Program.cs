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
        private const int RundenPauseMs = 800; // kurze Pause zwischen den Runden, damit man mitlesen kann

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
            List<Pferd> gewinnerListe = new List<Pferd>();

            Console.WriteLine("=== Pferderennen ===");
            Console.WriteLine($"Ziellinie: {Ziellinie} Meter");
            Console.WriteLine($"Teilnehmer: {string.Join(", ", pferde.Select(p => p.Name))}");
            Console.WriteLine();

            while (gewinnerListe.Count == 0)
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
                List<Pferd> pferdeImZiel = pferde.Where(p => p.Position >= Ziellinie).ToList();

                if (pferdeImZiel.Count > 0)
                {
                    // Von den angekommenen Pferden gewinnen die mit der größten Position.
                    // Haben davon mehrere exakt dieselbe Position, ist es ein Unentschieden.
                    int bestePosition = pferdeImZiel.Max(p => p.Position);
                    gewinnerListe = pferdeImZiel.Where(p => p.Position == bestePosition).ToList();
                }

                if (gewinnerListe.Count == 0)
                {
                    Thread.Sleep(RundenPauseMs);
                }
            }

            Console.WriteLine("**********************************");
            if (gewinnerListe.Count == 1)
            {
                Console.WriteLine($"  {gewinnerListe[0].Name} gewinnt das Rennen!");
            }
            else
            {
                string namen = NamenAufzaehlen(gewinnerListe);
                Console.WriteLine($"  Unentschieden zwischen {namen}!");
                Console.WriteLine($"  (jeweils {gewinnerListe[0].Position} Meter)");
            }
            Console.WriteLine("**********************************");
            Console.WriteLine();
            Console.WriteLine("Drücke eine beliebige Taste zum Beenden...");
            Console.ReadKey();
        }

        /// Verbindet die Namen mehrerer Pferde zu einer lesbaren Aufzählung, z. B. "Blitz und Sturm" oder "Blitz, Sturm und Donner".
        static string NamenAufzaehlen(List<Pferd> liste)
        {
            List<string> namen = liste.Select(p => p.Name).ToList();

            if (namen.Count == 1)
            {
                return namen[0];
            }

            string alleAusserLetztem = string.Join(", ", namen.Take(namen.Count - 1));
            string letzterName = namen[namen.Count - 1];
            return $"{alleAusserLetztem} und {letzterName}";
        }
    }
}