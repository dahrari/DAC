using System;
using System.Collections.Generic;
using System.Net;
using System.IO;

namespace DomainAvailabilityChecker
{
    interface DACSession
    {
        DACSessionResult Start();
    }

    class DACConsoleSession : DACSession
    {
        public DACSettings settings;

        public DACConsoleSession(DACSettings settings)
        {
            this.settings = settings;
        }

        public DACSessionResult Start()
        {
            if (settings == null)
            {
                throw new NoSettingsException("Settings can not be null when a session is started.");
            }

            DACSessionResult result = new DACSessionResult();
            List<string> availDomains = new List<string>();
            List<string> unavailDomains = new List<string>();
            List<string> erroDomains = new List<string>();
            string[] tlds = TLDs();

            result.SessionStarted = DateTime.Now;


            Console.WriteLine(settings.Introduction);
            bool stop = false;

            while (!stop)
            {
                string line = Console.ReadLine();

                if (line.Trim().Equals(""))
                {
                    stop = true;
                }
                else if (line.Contains(".") && !line.StartsWith(".") && !line.EndsWith("."))
                {
                    
                    if (!HasValidTLD(tlds, line))
                    {
                        Console.WriteLine(settings.ErroneousDomainText);
                        if (!erroDomains.Contains(line))
                        {
                            erroDomains.Add(line);
                        }
                        if (settings.PrintErrors)
                        {
                            Console.WriteLine("Invalid top-level domain");
                        }
                        continue;
                    }

                    IPHostEntry entry;
                    try
                    {
                        entry = Dns.GetHostEntry(line);
                        if (entry != null)
                        {
                            Console.WriteLine(settings.DomainUnavailableText);
                            if (!unavailDomains.Contains(line))
                            {
                                unavailDomains.Add(line);
                            }
                        }
                        else
                        {
                            Console.WriteLine(settings.DomainAvailableText);
                            if (!availDomains.Contains(line))
                            {
                                availDomains.Add(line);
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Equals("No such host is known"))
                        {
                            Console.WriteLine(settings.DomainAvailableText);
                            if (!availDomains.Contains(line))
                            {
                                availDomains.Add(line);
                            }
                        }
                        else
                        {
                            Console.WriteLine(settings.ErroneousDomainText);
                            if (!erroDomains.Contains(line))
                            {
                                erroDomains.Add(line);
                            }
                            if (settings.PrintErrors)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                }

            }

            Console.WriteLine(settings.FinishText);
            if (availDomains.Count > 0)
            {
                result.AvailableDomains = availDomains.ToArray();
            }

            if (unavailDomains.Count > 0)
            {
                result.UnavailableDomains = unavailDomains.ToArray();
            }

            if (erroDomains.Count > 0)
            {
                result.ErroneousDomains = erroDomains.ToArray();
            }

            result.SessionFinished = DateTime.Now;
            return result;
        }

        private bool HasValidTLD(string[] tlds, string testedUrl)
        {
            foreach(string tld in tlds)
            {
                if (testedUrl.EndsWith("." + tld))
                {
                    return true;
                }
            }
            return false;
        }

        private string[] TLDs()
        {
            List<string> result = new List<string>();
            string rawUrl = "https://github.com/umpirsky/tld-list/raw/master/data/en/tld.txt";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(rawUrl);
            HttpWebResponse res = (HttpWebResponse) req.GetResponse();
            StreamReader reader = new StreamReader(res.GetResponseStream());
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (line.Contains("("))
                {
                    result.Add(line.Split(new string[] { "(" }, StringSplitOptions.None)[1].Replace(")","").Trim());
                }
                
            }
            res.Close();
            res.Dispose();

            if (result.Count > 0 )
            {
                return result.ToArray();
            } else
            {
                return null;
            }
        }
    }

    class DACSettings
    {
        public string Introduction;
        public string DomainAvailableText;
        public string DomainUnavailableText;
        public string ErroneousDomainText;
        public string FinishText;
        public bool PrintErrors;

        public static DACSettings GetDefaultSettings()
        {
            DACSettings result = new DACSettings
            {
                Introduction = "Please type a domain and press Enter. If you want to quit, write nothing and press Enter.",
                DomainAvailableText = "Domain is available.",
                DomainUnavailableText = "Domain is not available.",
                ErroneousDomainText = "Error.",
                PrintErrors = true,
                FinishText = "Bye."
            };
            return result;
        }
    }

    class DACSessionResult
    {
        private string[] _availDomains;
        private string[] _unavailDomains;
        private string[] _errDomains;

        private DateTime _sessionStarted;
        private DateTime _sessionFinished;

        public string[] AvailableDomains { get => _availDomains; set => _availDomains = value; }
        public string[] UnavailableDomains { get => _unavailDomains; set => _unavailDomains = value; }
        public string[] ErroneousDomains { get => _errDomains; set => _errDomains = value; }
        public DateTime SessionStarted { get => _sessionStarted; set => _sessionStarted = value; }
        public DateTime SessionFinished { get => _sessionFinished; set => _sessionFinished = value; }
    }

    class NoSettingsException : Exception
    {
        public NoSettingsException()
        {
        }

        public NoSettingsException(string message) : base(message)
        {
        }

        public NoSettingsException(string message, Exception inner) : base(message, inner)
        {
        }
    }

}
