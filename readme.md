# Building and running this
* Run `dotnet restore`
* Run `dotnet run`

This should work automatically; if not you might have to install [.NET 5.0](https://dotnet.microsoft.com/download/dotnet/5.0) (or higher) and a maybe also a [MonoGame development environment](https://docs.monogame.net/articles/getting_started/0_getting_started.html) (the latter should be handled via NuGet in the `dotnet restore` tho).

## Screenshot:
![Alt text](/screenshot.png?raw=true "Screenshot")

# Functionality
* You can click-and-drag; this is a little laggy since everything gets re-generated during it but should work on any average-or-better GPU (tested on a GTX 670)
* You can scroll to change scale

# Known issues
* When using negative values, there are glitches extending from the zero point
* If `xOffset` and `yxOffset` are big (like `(int.Max / 2)` - example provided in code, but commented out), the generated noise loses a lot of definition
* The glitches seen around the zero point can also be visible in other areas of the map, especially when zoomed out with a fairly large offset
* With a scale over `5.0f` repeating patterns become visible