namespace MotionAnchor;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        if (RiotProcessGuard.IsRiotClientRunning())
        {
            return;
        }

        Application.Run(new OverlayForm());
    }
}
