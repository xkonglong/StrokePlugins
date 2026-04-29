using System;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;

namespace Stroke
{
    public static class TrayIcon
    {
        private static NotifyIcon _icon;
        private static ToolStripMenuItem _pauseItem;
        private static bool _paused;
        private static bool _initialized;

        public static void SetVisibility(bool visible)
        {
            if (!_initialized)
            {
                bool zh = CultureInfo.CurrentUICulture.Name.StartsWith("zh");
                _icon = new NotifyIcon
                {
                    Icon = Icon.ExtractAssociatedIcon(Assembly.GetEntryAssembly().Location) ?? SystemIcons.Application,
                    Text = "Stroke"
                };

                var menu = new ContextMenuStrip();
                _pauseItem = new ToolStripMenuItem(zh ? "暂停" : "Pause");
                _pauseItem.Click += (_, __) =>
                {
                    if (_paused)
                    {
                        MouseHook.StartHook();
                        _pauseItem.Text = zh ? "暂停" : "Pause";
                    }
                    else
                    {
                        MouseHook.StopHook();
                        _pauseItem.Text = zh ? "恢复" : "Resume";
                    }

                    _paused = !_paused;
                };

                var exitItem = new ToolStripMenuItem(zh ? "退出" : "Exit");
                exitItem.Click += (_, __) =>
                {
                    _icon.Visible = false;
                    _icon.Dispose();
                    Application.Exit();
                };

                menu.Items.Add(_pauseItem);
                menu.Items.Add(exitItem);
                _icon.ContextMenuStrip = menu;

                _icon.MouseClick += (_, e) =>
                {
                    if (e.Button == MouseButtons.Left)
                        _pauseItem.PerformClick();
                };

                Application.ApplicationExit += (_, __) =>
                {
                    _icon.Visible = false;
                    _icon.Dispose();
                };

                _initialized = true;
            }

            if (_icon != null)
                _icon.Visible = visible;
        }
    }
}