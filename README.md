# Natsu Framework 🌸

Natsu, successor to [Yoru Engine](https://github.com/nebula-developer/yoru-engine), is a versatile 2D game and app framework written in C#. It's lightweight, modular, and adaptable for various needs like software, games, and simulations.

### Features

- **📐 Interface-driven**: Natsu makes it easy to work across different platforms. It uses interfaces to hide the platform-specific details, so developers can focus on building their app without needing to worry about the underlying platform, whether it's web, mobile, desktop, or backend.

- **🧩 Extensibility**: The modular design of Natsu means you can easily customize and extend it. Developers can add new features or tweak existing ones, tailoring the framework to suit the specific needs of each project. This flexibility lets you create just the right set of tools for your app or game - you could even build your own framework on top of Natsu!

- **⚡ Performance**: Performance is important for any app or game, and the simplicity of Natsu's design means that optimization is left in the hands of the developer, rather than the framework. Developers can optimize their code as needed to achieve the desired results.


### Platforms

- **Desktop**: Through the use of [OpenTK](https://opentk.net/), Natsu can be used to build high-performance desktop applications for Windows, macOS, and Linux.
- **Mobile** (coming soon): Natsu can be used to build mobile applications for iOS and Android using [MAUI](https://dotnet.microsoft.com/en-us/apps/maui).
- **Web** (coming soon): Web applications can make Natsu even more flexible through the use of [Blazor](https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor), [WebAssembly](https://webassembly.org/), and [SkiaSharp](https://github.com/mono/SkiaSharp).


### Benefits compared to Yoru Engine

Natsu was rewritten from scratch to break past the limitations of Yoru Engine. Here are some of the key improvements:

- **Skia-reliant**: Yoru Engine was completely reliant on SkiaSharp for rendering, which made it difficult to port to other platforms. Natsu, on the other hand, primarily uses SkiaSharp for rendering and logic, but abstracts it away through interfaces making it easier to port to other platforms.

- **Transforms**: Yoru Engine made it difficult to work with custom transforms, which belonged to an assigned `Transform` class. Natsu simplifies this by placing all `Element` transformations directly in the `Element` class, and makes it easier to work with animations and custom transforms.


- **Everything else**: Natsu is a complete rewrite of Yoru Engine, so it's hard to list all the improvements. Just know that Natsu is more flexible, more powerful, and more efficient than Yoru Engine ever was - and it's constantly improving! 🚀

### Acknowledgements

Natsu was built with inspiration from the following projects:

- 🌟 [osu!framework](https://github.com/ppy/osu-framework)
- ⭐ [SkiaSharp](https://github.com/mono/SkiaSharp)
- [MonoGame](https://github.com/MonoGame/MonoGame)
- [Avalonia](https://github.com/AvaloniaUI/Avalonia)
