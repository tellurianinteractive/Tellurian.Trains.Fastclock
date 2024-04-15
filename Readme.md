# Fastclock App 4
**Version 4 is an almost complete rewrite of the Fastclock app.** 
> NOTE: THIS VERSIOn IS WORD IN PROGRESS AND IS NOT RELEASED.
> The source code for the current Fastclock app is found [here](https://github.com/tellurianinteractive/Tellurian.Trains.ModuleMeetingApp).

> NOTE: Version 4 API is incompatible with version 3 API. 
> API version 3 is not supported when version 4 is ready for production.
> In order to use the API, you must upgrade to the API version 4.

## Background
The fastclock app was initially started in 2018 as a prototype when Blazor still was in preview. 
Although refined and upgraded to .NET 8, there is a need to refresh and simplify the app.
The new take also helps to make the app more maintalable and adds a few new features.

## What is new?
- First and foremost, the app is getting a new user interface based on the [**Fluent UI** design system](https://developer.microsoft.com/en-us/fluentui#/) made by Microsoft.
- The API is upgraded for better structure and new features. 
- The server app code is greatly simplified and with better testing.

### Added features
- Stopping the clock for pauses at a real time for lunch etc. was supported earlier.
Now it is also possible to set a fast time when the clock should stop for a break.
This is especially useful when sessions are split in parts with a pause in between.
- Synching of locally running clock to slave cloud clock, which give users access to local clock without connecting to local WLAN and thereby loosing Internet access.
- Synching of cloud clock to locally running slave clock, which gives other local computers and users access without accessing Internet.
- Real time is now configurable to be in a named time zone.

