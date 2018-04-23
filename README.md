# DAC

DAC is a CLI or console application for testing multiple domain names for their availability. DAC is an abbreviation of DomainAvailabilityChecker.

DAC is currently targeted to .NET Framework 4.6.1 and to be run in Windows command line, but I see no reason why it couldn't be ported to .NET Core and release it to Linux and MacOS.

### History

My friend started a business and needs a web store. Of course he needs a solid domain name for it, so I wanted to help him by providing a list of at least 20 domain names that are 100% available. What I didn't want was to repeatedly navigate to some online whois-service, write a domain name, press enter, and according to its availability to copy and paste the domain name to a separate file, over and over again. So I wrote an app where the user can just punch in the domains, see results from each domain immediately and when finished, get a summary of all the domains that were 1) available, 2) unavailable or 3) erroneous.

### How to use

1. Download [DomainAvailabilityChecker.exe](https://github.com/dahrari/DAC/raw/master/DomainAvailabilityChecker/bin/Debug/DomainAvailabilityChecker.exe) and place it anywhere you wish.
2. Run the executable.
3. ???
4. Profit.

DAC is most preferably ran from `cmd` so you'll be able to see the summary after you've done typing in the domain names. Otherwise, if you just doubleclick the exe in Windows, the summary will just flash on the screen and quickly disappear as the window closes.

### How does DAC work

When you punch in the domain name, it first checks if the TLD (=top-level domain) is correct by downloading a list of TLDs from [here](https://github.com/umpirsky/tld-list/raw/master/data/en/tld.txt) and checking if the TLD your domain contains is contained in that list. If not, then that domain is marked as *erroneous domain*. If it is contained, then it tries to get a HostEntry with it. If there is a HostEntry, then the domain is marked as *unavailable domain*. If there isn't, then the domain is marked as *available domain*. If the GetHostEntry() throws an error, the domain is marked as *erroneous domain*.

### Terminology

Available domain = the domain is available; go ahead and register it to yourself

Unavailable domain = someone already owns it

Erroneous domain = either it has an unknown TLD or something else happened while trying to resolve it. One of the reasons might be a connectivity problem with your computer

