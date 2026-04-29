using System;
using System.Drawing;
using System.Drawing.Imaging;
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
        private static Icon _normalIcon;
        private static Icon _grayIcon;

        public static void SetVisibility(bool visible)
        {
            if (!_initialized)
            {
                bool zh = CultureInfo.CurrentUICulture.Name.StartsWith("zh");
                _normalIcon = Icon.ExtractAssociatedIcon(Assembly.GetEntryAssembly().Location) ?? SystemIcons.Application;
                _grayIcon = CreateGrayIcon(_normalIcon);

                _icon = new NotifyIcon
                {
                    Icon = _normalIcon,
                    Text = "Stroke"
                };

                var menu = new ContextMenuStrip();
                _pauseItem = new ToolStripMenuItem(zh ? "暂停" : "Pause");
                _pauseItem.Click += (_, __) =>
                {
                    if (_paused)
                    {
                        MouseHook.StartHook();
                        _icon.Icon = _normalIcon;
                        _pauseItem.Text = zh ? "暂停" : "Pause";
                    }
                    else
                    {
                        MouseHook.StopHook();
                        _icon.Icon = _grayIcon;
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

        private static Icon CreateGrayIcon(Icon original)
        {
            using (Bitmap originalBitmap = original.ToBitmap())
            {
                Bitmap grayBitmap = new Bitmap(originalBitmap.Width, originalBitmap.Height);
                using (Graphics graphics = Graphics.FromImage(grayBitmap))
                {
                    ColorMatrix colorMatrix = new ColorMatrix(new float[][]
                    {
                        new float[] {0.3f, 0.3f, 0.3f, 0, 0},
                        new float[] {0.59f, 0.59f, 0.59f, 0, 0},
                        new float[] {0.11f, 0.11f, 0.11f, 0, 0},
                        new float[] {0, 0, 0, 1, 0},
                        new float[] {0, 0, 0, 0, 1}
                    });
                    using (ImageAttributes attributes = new ImageAttributes())
                    {
                        attributes.SetColorMatrix(colorMatrix);
                        graphics.DrawImage(originalBitmap, new Rectangle(0, 0, grayBitmap.Width, grayBitmap.Height), 0, 0, originalBitmap.Width, originalBitmap.Height, GraphicsUnit.Pixel, attributes);
                    }
                }
                return Icon.FromHandle(grayBitmap.GetHicon());
            }
        }
    }
}