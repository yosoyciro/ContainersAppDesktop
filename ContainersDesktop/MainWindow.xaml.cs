using System.Threading;
using ContainersDesktop.Helpers;
using Windows.UI.ViewManagement;

namespace ContainersDesktop;

public sealed partial class MainWindow : WindowEx
{
    private Microsoft.UI.Dispatching.DispatcherQueue dispatcherQueue;
    private Timer timer;

    private UISettings settings;

    public MainWindow()
    {
        InitializeComponent();

        AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/WindowIcon.ico"));
        Content = null;
        Title = "AppDisplayName".GetLocalized();

        // Theme change code picked from https://github.com/microsoft/WinUI-Gallery/pull/1239
        dispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();
        settings = new UISettings();
        settings.ColorValuesChanged += Settings_ColorValuesChanged; // cannot use FrameworkElement.ActualThemeChanged event

        timer = new Timer(timerCallback, null, (int)TimeSpan.FromSeconds(15).TotalMilliseconds, Timeout.Infinite);
    }

    // this handles updating the caption button colors correctly when indows system theme is changed
    // while the app is open
    private void Settings_ColorValuesChanged(UISettings sender, object args)
    {
        // This calls comes off-thread, hence we will need to dispatch it to current app's thread
        dispatcherQueue.TryEnqueue(() =>
        {
            TitleBarHelper.ApplySystemThemeToCaptionButtons();
        });
    }

    private void timerCallback(object state)
    {
        // do some work not connected with UI

        dispatcherQueue.TryEnqueue(
            () => {
                // do some work on UI here;
                Console.WriteLine("timer");
            });
    }
}
