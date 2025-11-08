using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.Windows.ApplicationModel.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.InteropServices;
using Windows.ApplicationModel;
using Windows.ApplicationModel.DataTransfer;
using Windows.Graphics;
using Windows.Storage;
using Windows.UI;
using Microsoft.UI.Windowing;

namespace Crayon
{
    public sealed partial class MainWindow : Window
	{
        private ResourceLoader rl = new ResourceLoader();
        public ObservableCollection<ColorItem> AvailableColors { get; } = new ObservableCollection<ColorItem>();
        const int SHCNE_ASSOCCHANGED = 0x08000000;
        const int SHCNE_UPDATEDIR = 0x00001000;
        const uint SHCNF_PATHW = 0x0005;
        const uint SHCNF_FLUSH = 0x1000;
        const uint SHCNF_IDLIST = 0x0000;
        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        static extern void SHChangeNotify(int wEventId, uint uFlags, string dwItem1, string dwItem2);

        const int HWND_BROADCAST = 0xffff;
        const int WM_SETTINGCHANGE = 0x001A;
        const int SMTO_ABORTIFHUNG = 0x0002;
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        static extern IntPtr SendMessageTimeout(IntPtr hWnd, int Msg, IntPtr wParam, string lParam,
            uint fuFlags, uint uTimeout, out IntPtr lpdwResult);


        private const int WM_GETMINMAXINFO = 0x0024;

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MINMAXINFO
        {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        }

        [DllImport("comctl32.dll", SetLastError = true)]
        public static extern bool SetWindowSubclass(IntPtr hWnd, SubclassProc pfnSubclass, uint uIdSubclass, IntPtr dwRefData);

        [DllImport("comctl32.dll", SetLastError = true)]
        public static extern bool RemoveWindowSubclass(IntPtr hWnd, SubclassProc pfnSubclass, uint uIdSubclass);

        [DllImport("comctl32.dll", SetLastError = true)]
        public static extern IntPtr DefSubclassProc(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

        // 🔹 اینجا public شده تا ارور برطرف شه
        public delegate IntPtr SubclassProc(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam, IntPtr uIdSubclass, IntPtr dwRefData);

        private readonly int minWidth = 862;
        private readonly int minHeight = 529;


        public static void RefreshFolder(string folderPath)
        {
            // 1) Notify updatedir
            SHChangeNotify(SHCNE_UPDATEDIR, SHCNF_PATHW | SHCNF_FLUSH, folderPath, null);

            // 2) force a global setting change broadcast (some shell components listen to this)
            IntPtr result;
            SendMessageTimeout((IntPtr)HWND_BROADCAST, WM_SETTINGCHANGE, IntPtr.Zero, "ShellState",
                SMTO_ABORTIFHUNG, 1000, out result);

            // 3) also send assocchanged to force icon/association refresh
            SHChangeNotify(SHCNE_ASSOCCHANGED, 0, null, null);
        }

        public class ColorItem
		{
			public Color Color { get; set; }
			public string Name { get; set; }
            public string FileName { get; set; }
            public string FileNameAlt { get; set; }

            public ColorItem(Color color, string name, string fileName, string fileNameAlt)
			{
				Color = color;
				Name = name;
                FileName = fileName;
                FileNameAlt = fileNameAlt;
            }
		}

        public MainWindow()
		{
			this.InitializeComponent();

            IntPtr hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            SetWindowSubclass(hwnd, WndProc, 0, IntPtr.Zero);

            var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hwnd);
            AppWindow appWindow = AppWindow.GetFromWindowId(windowId);
            appWindow.Resize(new SizeInt32(minWidth, minHeight));

            ExtendsContentIntoTitleBar = true;
			this.SetTitleBar(AppTitleBar);
            this.Title = rl.GetString("AppName");

            //AvailableColors.Add(new ColorItem(Color.FromArgb(255, 0, 0, 0), "Drive", "drive-windows11-drive.ico"),
            //AvailableColors.Add(new ColorItem(Color.FromArgb(255, 0, 0, 0), "Windows", "drive-windows11-windows.ico"),
            AvailableColors.Add(new ColorItem(Color.FromArgb(255, 255, 206, 60), rl.GetString("ColorYellow"), "folder-windows11-yellow.ico", "folder-kde25-yellow.svg"));
            AvailableColors.Add(new ColorItem(Color.FromArgb(255, 228, 60, 41), rl.GetString("ColorDarkRed"), "folder-windows11-darkred.ico", "folder-kde25-darkred.svg"));
            AvailableColors.Add(new ColorItem(Color.FromArgb(255, 237, 111, 15), rl.GetString("ColorDarkOrange"), "folder-windows11-darkorange.ico", "folder-kde25-darkorange.svg"));
            AvailableColors.Add(new ColorItem(Color.FromArgb(255, 62, 158, 74), rl.GetString("ColorDarkGreen"), "folder-windows11-darkgreen.ico", "folder-kde25-darkgreen.svg"));
            AvailableColors.Add(new ColorItem(Color.FromArgb(255, 39, 147, 142), rl.GetString("ColorDarkTeal"), "folder-windows11-darkteal.ico", "folder-kde25-darkteal.svg"));
            AvailableColors.Add(new ColorItem(Color.FromArgb(255, 31, 132, 208), rl.GetString("ColorDarkBlue"), "folder-windows11-darkblue.ico", "folder-kde25-darkblue.svg"));
            AvailableColors.Add(new ColorItem(Color.FromArgb(255, 153, 96, 198), rl.GetString("ColorDarkPurple"), "folder-windows11-darkpurple.ico", "folder-kde25-darkpurple.svg"));
            AvailableColors.Add(new ColorItem(Color.FromArgb(255, 206, 85, 185), rl.GetString("ColorDarkPink"), "folder-windows11-darkpink.ico", "folder-kde25-darkpink.svg"));
            AvailableColors.Add(new ColorItem(Color.FromArgb(255, 176, 183, 186), rl.GetString("ColorGrey"), "folder-windows11-grey.ico", "folder-kde25-grey.svg"));
            AvailableColors.Add(new ColorItem(Color.FromArgb(255, 255, 188, 178), rl.GetString("ColorLightRed"), "folder-windows11-lightred.ico", "folder-kde25-lightred.svg"));
            AvailableColors.Add(new ColorItem(Color.FromArgb(255, 255, 191, 132), rl.GetString("ColorLightOrange"), "folder-windows11-lightorange.ico", "folder-kde25-lightorange.svg"));
            AvailableColors.Add(new ColorItem(Color.FromArgb(255, 142, 210, 144), rl.GetString("ColorLightGreen"), "folder-windows11-lightgreen.ico", "folder-kde25-lightgreen.svg"));
            AvailableColors.Add(new ColorItem(Color.FromArgb(255, 122, 209, 205), rl.GetString("ColorLightTeal"), "folder-windows11-lightteal.ico", "folder-kde25-lightteal.svg"));
            AvailableColors.Add(new ColorItem(Color.FromArgb(255, 134, 200, 247), rl.GetString("ColorLightBlue"), "folder-windows11-lightblue.ico", "folder-kde25-lightblue.svg"));
            AvailableColors.Add(new ColorItem(Color.FromArgb(255, 212, 175, 246), rl.GetString("ColorLightPurple"), "folder-windows11-lightpurple.ico", "folder-kde25-lightpurple.svg"));
            AvailableColors.Add(new ColorItem(Color.FromArgb(255, 247, 170, 231), rl.GetString("ColorLightPink"), "folder-windows11-lightpink.ico", "folder-kde25-lightpink.svg"));
            //AvailableColors.Add(new ColorItem(Color.FromArgb(255, 0, 0, 0), "Documents", "library-windows11-documents.ico"),
            //AvailableColors.Add(new ColorItem(Color.FromArgb(255, 0, 0, 0), "Music", "library-windows11-music.ico"),
            //AvailableColors.Add(new ColorItem(Color.FromArgb(255, 0, 0, 0), "Pictures", "library-windows11-pictures.ico"),
            //AvailableColors.Add(new ColorItem(Color.FromArgb(255, 0, 0, 0), "TV", "library-windows11-tv.ico"),
            //AvailableColors.Add(new ColorItem(Color.FromArgb(255, 0, 0, 0), "Videos", "library-windows11-videos.ico"),
            //AvailableColors.Add(new ColorItem(Color.FromArgb(255, 0, 0, 0), "Drive", "drive-windows10-drive.ico"),
            //AvailableColors.Add(new ColorItem(Color.FromArgb(255, 0, 0, 0), "Windows", "drive-windows10-windows.ico"),
            //AvailableColors.Add(new ColorItem(Color.FromArgb(255, 255,206,60), "Yellow", "folder-windows10-yellow.ico"),
            //AvailableColors.Add(new ColorItem(Color.FromArgb(255, 228,60,41), "Dark Red", "folder-windows10-darkred.ico"),
            //AvailableColors.Add(new ColorItem(Color.FromArgb(255, 237,111,15), "Dark Orange", "folder-windows10-darkorange.ico"),
            //AvailableColors.Add(new ColorItem(Color.FromArgb(255, 62,158,74), "Dark Green", "folder-windows10-darkgreen.ico"),
            //AvailableColors.Add(new ColorItem(Color.FromArgb(255, 39,147,142), "Dark Teal", "folder-windows10-darkteal.ico"),
            //AvailableColors.Add(new ColorItem(Color.FromArgb(255, 31,132,208), "Dark Blue", "folder-windows10-darkblue.ico"),
            //AvailableColors.Add(new ColorItem(Color.FromArgb(255, 153,96,198), "Dark Purple", "folder-windows10-darkpurple.ico"),
            //AvailableColors.Add(new ColorItem(Color.FromArgb(255, 206,85,185), "Dark Pink", "folder-windows10-darkpink.ico"),
            //AvailableColors.Add(new ColorItem(Color.FromArgb(255, 176,183,186), "Grey", "folder-windows10-grey.ico"),
            //AvailableColors.Add(new ColorItem(Color.FromArgb(255, 255,188,178), "Light Red", "folder-windows10-lightred.ico"),
            //AvailableColors.Add(new ColorItem(Color.FromArgb(255, 255,191,132), "Light Orange", "folder-windows10-lightorange.ico"),
            //AvailableColors.Add(new ColorItem(Color.FromArgb(255, 142,210,144), "Light Green", "folder-windows10-lightgreen.ico"),
            //AvailableColors.Add(new ColorItem(Color.FromArgb(255, 122,209,205), "Light Teal", "folder-windows10-lightteal.ico"),
            //AvailableColors.Add(new ColorItem(Color.FromArgb(255, 134,200,247), "Light Blue", "folder-windows10-lightblue.ico"),
            //AvailableColors.Add(new ColorItem(Color.FromArgb(255, 212,175,246), "Light Purple", "folder-windows10-lightpurple.ico"),
            //AvailableColors.Add(new ColorItem(Color.FromArgb(255, 247,170,231), "Light Pink", "folder-windows10-lightpink.ico"),
            //AvailableColors.Add(new ColorItem(Color.FromArgb(255, 0, 0, 0), "Documents", "library-windows10-documents.ico"),
            //AvailableColors.Add(new ColorItem(Color.FromArgb(255, 0, 0, 0), "Music", "library-windows10-music.ico"),
            //AvailableColors.Add(new ColorItem(Color.FromArgb(255, 0, 0, 0), "Pictures", "library-windows10-pictures.ico"),
            //AvailableColors.Add(new ColorItem(Color.FromArgb(255, 0, 0, 0), "TV", "library-windows10-tv.ico"),
            //AvailableColors.Add(new ColorItem(Color.FromArgb(255, 0, 0, 0), "Videos", "library-windows10-videos.ico")
        }

        private IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam, IntPtr uIdSubclass, IntPtr dwRefData)
        {
            switch (msg)
            {
                case WM_GETMINMAXINFO:
                    unsafe
                    {
                        MINMAXINFO* info = (MINMAXINFO*)lParam;
                        info->ptMinTrackSize.x = minWidth;
                        info->ptMinTrackSize.y = minHeight;
                    }
                    break;
            }
            return DefSubclassProc(hWnd, msg, wParam, lParam);
        }

        private void Color_Checked(object sender, RoutedEventArgs e)
		{
			if (sender is RadioButton rb && rb.DataContext is ColorItem item)
			{
				SelectedText.Text = $"You've selected the color—{item.Name}.";
			}
			SetIcon();

        }

        private RadioButton FindSelectedRadioButton(ItemsControl itemsControl)
		{
			foreach (var item in itemsControl.Items)
			{
				var container = itemsControl.ContainerFromItem(item) as FrameworkElement;
				if (container != null)
				{
					var rb = FindChild<RadioButton>(container);
					if (rb != null && rb.IsChecked == true)
						return rb;
				}
			}
			return null;
		}

		private T FindChild<T>(DependencyObject parent) where T : DependencyObject
		{
			int count = VisualTreeHelper.GetChildrenCount(parent);
			for (int i = 0; i < count; i++)
			{
				var child = VisualTreeHelper.GetChild(parent, i);
				if (child is T tChild)
					return tChild;

				var result = FindChild<T>(child);
				if (result != null)
					return result;
			}
			return null;
		}

		private void SetIcon()
		{
            MyInfoBar.IsOpen = false;

            var selectedRadio = FindSelectedRadioButton(ColorsList);
            string iconFile = string.Empty;
            string iconFileAlt = string.Empty;
            if (selectedRadio?.DataContext is ColorItem colorItem)
            {
                iconFile = $"{colorItem.FileName}";
                iconFileAlt = $"{colorItem.FileNameAlt}";
            }
            else
            {
                MyInfoBar.Title = "Error 1";
                MyInfoBar.Message = "No Icon Selected!";
                MyInfoBar.IsOpen = true;
                return;
            }

            string[] cmdLine = Environment.GetCommandLineArgs();
			string pathAddr = string.Empty;
			if (cmdLine.Length > 1)
			{
				pathAddr = Environment.GetCommandLineArgs()[1];
			}
            else
            {
                MyInfoBar.Title = "Error 2";
                MyInfoBar.Message = "No Path Selected!";
                MyInfoBar.IsOpen = true;
				return;
            }

            string iconPath = System.IO.Path.Combine(Package.Current.InstalledLocation.Path, "Assets", "Icons", iconFile);
            string dskFile = System.IO.Path.Combine(pathAddr, "desktop.ini");
            string copyPath = System.IO.Path.Combine(pathAddr, ".icon.ico");

            string iconPathAlt = System.IO.Path.Combine(Package.Current.InstalledLocation.Path, "Assets", "Icons", iconFileAlt);
            string copyPathAlt = System.IO.Path.Combine(pathAddr, ".icon.svg");

            try
            {
                if (System.IO.File.Exists(dskFile))
				{
					System.IO.File.Delete(dskFile);
				}
                System.IO.File.WriteAllText(dskFile, string.Format("[.ShellClassInfo]\r\nLocalizedResourceName=Folder\r\nIconResource=.icon.ico,0\r\nIconFile=.icon.ico\r\nIconIndex=0\r\nConfirmFileOp=0\r\nNoSharing=0\r\nInfoTip=Custom Folder\r\n\r\n[ViewState]\r\nMode=\r\nVid=\r\nFolderType=Generic"));
                System.IO.FileAttributes attrs1 = System.IO.File.GetAttributes(dskFile);
                attrs1 |= System.IO.FileAttributes.Hidden | System.IO.FileAttributes.System | System.IO.FileAttributes.Archive;
                System.IO.File.SetAttributes(dskFile, attrs1);
                System.IO.File.SetLastWriteTime(dskFile, DateTime.Now);

                if (System.IO.File.Exists(copyPath))
                {
                    System.IO.File.Delete(copyPath);
                }
                System.IO.File.Copy(iconPath, copyPath);
                System.IO.FileAttributes attrs2 = System.IO.File.GetAttributes(copyPath);
                attrs2 |= System.IO.FileAttributes.Hidden | System.IO.FileAttributes.System;
                System.IO.File.SetAttributes(copyPath, attrs2);

                if (System.IO.File.Exists(copyPathAlt))
                {
                    System.IO.File.Delete(copyPathAlt);
                }
                System.IO.File.Copy(iconPathAlt, copyPathAlt);
                System.IO.FileAttributes attrs4 = System.IO.File.GetAttributes(copyPath);
                attrs4 |= System.IO.FileAttributes.Hidden | System.IO.FileAttributes.System;
                System.IO.File.SetAttributes(copyPathAlt, attrs4);

                string hdnFile = System.IO.Path.Combine(pathAddr, ".hidden");
                if (System.IO.File.Exists(hdnFile))
                {
                    System.IO.File.Delete(hdnFile);
                }
                System.IO.File.WriteAllText(hdnFile, string.Format("desktop.ini\r\nautorun.inf"));
                System.IO.FileAttributes attrs3 = System.IO.File.GetAttributes(hdnFile);
                attrs3 |= System.IO.FileAttributes.Hidden | System.IO.FileAttributes.System;
                System.IO.File.SetAttributes(hdnFile, attrs3);

                string dirFile = System.IO.Path.Combine(pathAddr, ".directory");
                if (System.IO.File.Exists(dirFile))
                {
                    System.IO.File.Delete(dirFile);
                }
                System.IO.File.WriteAllText(dirFile, string.Format("[Desktop Entry]\r\nIcon=.icon.svg\r\nName=Folder\r\nComment=Custom Folder\r\n\r\n[Dolphin]\r\nViewMode=1\r\n"));
                System.IO.FileAttributes attrs5 = System.IO.File.GetAttributes(dirFile);
                attrs5 |= System.IO.FileAttributes.Hidden | System.IO.FileAttributes.System;
                System.IO.File.SetAttributes(dirFile, attrs5);

                string arFile = System.IO.Path.Combine(pathAddr, "autorun.inf");
                if (System.IO.File.Exists(arFile))
                {
                    System.IO.File.Delete(arFile);
                }
                System.IO.File.WriteAllText(arFile, string.Format("[autorun]\r\nicon=.icon.ico,0\r\nlabel=Folder"));
                System.IO.FileAttributes attrs6 = System.IO.File.GetAttributes(arFile);
                attrs6 |= System.IO.FileAttributes.Hidden | System.IO.FileAttributes.System;
                System.IO.File.SetAttributes(arFile, attrs6);

                var dirInfo = new System.IO.DirectoryInfo(pathAddr);
                dirInfo.Attributes |= System.IO.FileAttributes.ReadOnly;
                System.IO.Directory.SetLastWriteTime(pathAddr, DateTime.Now);

				RefreshFolder(pathAddr);
            }
            catch (Exception ex)
			{
                MyInfoBar.Title = "Error 3";
                MyInfoBar.Message = ex.Message;
                MyInfoBar.IsOpen = true;
                return;
            }
        }

		private void ClearButton_Click(object sender, RoutedEventArgs e)
		{
            Close();
        }

    }
}