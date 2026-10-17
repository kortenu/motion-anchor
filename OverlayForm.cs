using System.ComponentModel;
using System.Runtime.InteropServices;

namespace MotionAnchor;

internal sealed class OverlayForm : Form
{
    private const int WmHotkey = 0x0312;
    private const int ToggleHotkeyId = 1;
    private const int ExitHotkeyId = 2;
    private const uint ModAlt = 0x0001;
    private const uint ModControl = 0x0002;
    private const int WsExTransparent = 0x00000020;
    private const int WsExToolWindow = 0x00000080;
    private const int WsExLayered = 0x00080000;
    private const int WsExNoActivate = 0x08000000;
    private const uint SwpNoSize = 0x0001;
    private const uint SwpNoMove = 0x0002;
    private const uint SwpShowWindow = 0x0040;
    private const uint SwpNoActivate = 0x0010;
    private static readonly IntPtr HwndTopmost = new(-1);
    private readonly ContextMenuStrip _trayMenu;
    private readonly NotifyIcon _trayIcon;

    private const int DotSpacing = 48;
    private const float DotDiameter = 4;
    private const double OverlayOpacity = 0.35;
    private static readonly Color TransparentColor = Color.Magenta;

    public OverlayForm()
    {
        BackColor = TransparentColor;
        TransparencyKey = TransparentColor;
        Opacity = OverlayOpacity;
        FormBorderStyle = FormBorderStyle.None;
        ShowInTaskbar = false;
        TopMost = true;
        StartPosition = FormStartPosition.Manual;
        Bounds = Screen.PrimaryScreen?.Bounds
            ?? throw new InvalidOperationException("No primary display is available.");
        DoubleBuffered = true;

        _trayMenu = new ContextMenuStrip();
        _trayMenu.Items.Add("Show/Hide overlay", null, (_, _) => ToggleOverlay());
        _trayMenu.Items.Add("Exit Motion Anchor", null, (_, _) => Close());
        _trayIcon = new NotifyIcon
        {
            ContextMenuStrip = _trayMenu,
            Icon = SystemIcons.Application,
            Text = "Motion Anchor",
            Visible = true
        };
        _trayIcon.DoubleClick += (_, _) => ToggleOverlay();
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _trayIcon.Visible = false;
        _trayIcon.Dispose();
        _trayMenu.Dispose();
        base.OnFormClosed(e);
    }

    protected override bool ShowWithoutActivation => true;

    protected override CreateParams CreateParams
    {
        get
        {
            var createParams = base.CreateParams;
            createParams.ExStyle |= WsExTransparent | WsExToolWindow | WsExLayered | WsExNoActivate;
            return createParams;
        }
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);

        if (!SetWindowPos(
                Handle,
                HwndTopmost,
                0,
                0,
                0,
                0,
                SwpNoMove | SwpNoSize | SwpNoActivate | SwpShowWindow))
        {
            throw new Win32Exception(Marshal.GetLastWin32Error(), "Could not keep the overlay above other windows.");
        }
    }

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);

        if (!RegisterHotKey(Handle, ToggleHotkeyId, ModControl | ModAlt, (uint)Keys.O))
        {
            ShowHotkeyError("Ctrl+Alt+O");
            return;
        }

        if (!RegisterHotKey(Handle, ExitHotkeyId, ModControl | ModAlt, (uint)Keys.Q))
        {
            UnregisterHotKey(Handle, ToggleHotkeyId);
            ShowHotkeyError("Ctrl+Alt+Q");
        }
    }

    protected override void OnHandleDestroyed(EventArgs e)
    {
        if (IsHandleCreated)
        {
            UnregisterHotKey(Handle, ToggleHotkeyId);
            UnregisterHotKey(Handle, ExitHotkeyId);
        }

        base.OnHandleDestroyed(e);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        using var brush = new SolidBrush(Color.White);
        var offset = DotSpacing / 2;
        for (var y = offset; y < ClientSize.Height; y += DotSpacing)
        {
            for (var x = offset; x < ClientSize.Width; x += DotSpacing)
            {
                e.Graphics.FillEllipse(brush, x, y, DotDiameter, DotDiameter);
            }
        }
    }

    protected override void WndProc(ref Message message)
    {
        if (message.Msg == WmHotkey)
        {
            switch (message.WParam.ToInt32())
            {
                case ToggleHotkeyId:
                    ToggleOverlay();
                    break;
                case ExitHotkeyId:
                    Close();
                    break;
            }

            return;
        }

        base.WndProc(ref message);
    }

    private void ToggleOverlay()
    {
        Visible = !Visible;
    }

    private void ShowHotkeyError(string shortcut)
    {
        MessageBox.Show(
            $"Motion Anchor could not register {shortcut}. The operating system may have refused the shortcut or another application may already be using it.",
            "Motion Anchor",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error);
        BeginInvoke(new Action(Close));
    }

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool RegisterHotKey(IntPtr windowHandle, int id, uint modifiers, uint virtualKey);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnregisterHotKey(IntPtr windowHandle, int id);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetWindowPos(
        IntPtr windowHandle,
        IntPtr insertAfter,
        int x,
        int y,
        int width,
        int height,
        uint flags);
}
