using Natsu.Core;
using Natsu.Mathematics;
using Natsu.Platforms.Empty;
using Natsu.Platforms.Empty.Audio;
using Natsu.Platforms.Empty.Shaders;

namespace Natsu.Tests.Core;

public class ApplicationTests {
    internal Application CreateApp(bool load = true) {
        Application app = new();
        app.Renderer = new EmptyRenderer();
        app.Platform = new EmptyPlatform();
        app.ResourceLoader = new EmptyResourceLoader();
        app.AudioManager = new EmptyAudioManager();
        app.ShaderManager = new EmptyShaderManager();

        if (load) app.Load();
        return app;
    }

    [Fact]
    public void TestUpdate() {
        Application app = CreateApp();

        app.Update(0.1f);
        Assert.True(Precision.Approximately(0.1f, app.Time.Time));

        app.Time.TimeScale = 2;
        app.Update(0.1f);
        Assert.True(Precision.Approximately(0.3f, app.Time.Time));
        Assert.True(Precision.Approximately(0.2f, app.Time.RawTime));
    }

    [Fact]
    public void TestComposite() {
        Application app = CreateApp();

        Element parent = new();
        Element child = new();
        parent.Add(child);
        app.Add(parent);

        Assert.Contains(parent, app.Root.Children);
        Assert.Contains(child, parent.Children);

        child.Parent = app;
        Assert.Contains(child, app.Root.Children);
    }

    [Fact]
    public void TestRender() {
        Application app = CreateApp();

        Element element = new() {
            Parent = new() {
                Parent = app
            }
        };

        bool rendered = false;
        element.DoRender += () => rendered = true;
        app.Render(0f);

        Assert.True(rendered);
    }

    [Fact]
    public void TestLoad() {
        Application app = CreateApp(false);

        Element elementA = new();
        Element elementB = new();

        elementA.Add(elementB);
        Assert.False(elementA.Loaded);
        Assert.False(elementB.Loaded);

        elementA.Remove(elementB);
        app.Add(elementA);
        Assert.False(elementA.Loaded);
        Assert.False(elementB.Loaded);

        app.Load();
        Assert.True(elementA.Loaded);
        Assert.False(elementB.Loaded);

        app.Add(elementB);
        Assert.True(elementB.Loaded);
    }

    [Fact]
    public void TestBatchNativePlatform() {
        Application app = CreateApp();

        Element element = new();
        element.Platform.Title = "Test";

        Assert.NotEqual("Test", app.Platform.Title);
        app.Add(element);
        Assert.Equal("Test", app.Platform.Title);
    }
}
