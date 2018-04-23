using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainAvailabilityChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            bool printResultsAfterwards = true;

            DACConsoleSession session = new DACConsoleSession(DACSettings.GetDefaultSettings());
            Console.WriteLine("Starting...");
            DACSessionResult result = session.Start();
            if (printResultsAfterwards)
            {
                if (result.AvailableDomains != null)
                {
                    Console.WriteLine("\nAvailable domains:");
                    foreach (string domain in result.AvailableDomains)
                    {
                        Console.WriteLine(domain);
                    }
                }
                if (result.UnavailableDomains != null)
                {
                    Console.WriteLine("\nUnavailable domains:");
                    foreach (string domain in result.UnavailableDomains)
                    {
                        Console.WriteLine(domain);
                    }
                }
                if (result.ErroneousDomains != null)
                {
                    Console.WriteLine("\nErroneous domains:");
                    foreach (string domain in result.ErroneousDomains)
                    {
                        Console.WriteLine(domain);
                    }
                }
            }
        }


    }
}
