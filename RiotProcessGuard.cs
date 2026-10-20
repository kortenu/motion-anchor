using System.Diagnostics;

namespace MotionAnchor;

internal static class RiotProcessGuard
{
    private static readonly string[] ProcessNames =
    [
        "League of Legends",
        "LeagueClient",
        "LeagueClientUx",
        "LeagueClientUxRender",
        "RiotClientUx",
        "RiotClientUxRender"
    ];

    internal static bool IsRiotClientRunning()
    {
        foreach (var processName in ProcessNames)
        {
            var processes = Process.GetProcessesByName(processName);
            try
            {
                if (processes.Length > 0)
                {
                    return true;
                }
            }
            finally
            {
                foreach (var process in processes)
                {
                    process.Dispose();
                }
            }
        }

        return false;
    }
}
