using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.IO;
using Windows.ApplicationModel;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.UI;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Crayon
{
	public sealed partial class MainWindow : Window
	{
		public class ColorItem
		{
			public Color Color { get; set; }
			public string Name { get; set; }
			public string IconPath { get; set; }

			public ColorItem(Color color, string name, string iconPath)
			{
				Color = color;
				Name = name;
				IconPath = iconPath;
			}
		}

		public List<ColorItem> AvailableColors { get; } = new()
				{
					//new ColorItem(Color.FromArgb(255, 0, 0, 0), "Drive", "drive-windows11-drive.ico"),
					//new ColorItem(Color.FromArgb(255, 0, 0, 0), "Windows", "drive-windows11-windows.ico"),
					new ColorItem(Color.FromArgb(255, 255,206,60), "Yellow", "folder-windows11-yellow.ico"),
					new ColorItem(Color.FromArgb(255, 228,60,41), "Dark Red", "folder-windows11-darkred.ico"),
					new ColorItem(Color.FromArgb(255, 237,111,15), "Dark Orange", "folder-windows11-darkorange.ico"),
					new ColorItem(Color.FromArgb(255, 62,158,74), "Dark Green", "folder-windows11-darkgreen.ico"),
					new ColorItem(Color.FromArgb(255, 39,147,142), "Dark Teal", "folder-windows11-darkteal.ico"),
					new ColorItem(Color.FromArgb(255, 31,132,208), "Dark Blue", "folder-windows11-darkblue.ico"),
					new ColorItem(Color.FromArgb(255, 153,96,198), "Dark Purple", "folder-windows11-darkpurple.ico"),
					new ColorItem(Color.FromArgb(255, 206,85,185), "Dark Pink", "folder-windows11-darkpink.ico"),
					new ColorItem(Color.FromArgb(255, 176,183,186), "Grey", "folder-windows11-grey.ico"),
					new ColorItem(Color.FromArgb(255, 255,188,178), "Light Red", "folder-windows11-lightred.ico"),
					new ColorItem(Color.FromArgb(255, 255,191,132), "Light Orange", "folder-windows11-lightorange.ico"),
					new ColorItem(Color.FromArgb(255, 142,210,144), "Light Green", "folder-windows11-lightgreen.ico"),
					new ColorItem(Color.FromArgb(255, 122,209,205), "Light Teal", "folder-windows11-lightteal.ico"),
					new ColorItem(Color.FromArgb(255, 134,200,247), "Light Blue", "folder-windows11-lightblue.ico"),
					new ColorItem(Color.FromArgb(255, 212,175,246), "Light Purple", "folder-windows11-lightpurple.ico"),
					new ColorItem(Color.FromArgb(255, 247,170,231), "Light Pink", "folder-windows11-lightpink.ico"),
					//new ColorItem(Color.FromArgb(255, 0, 0, 0), "Documents", "library-windows11-documents.ico"),
					//new ColorItem(Color.FromArgb(255, 0, 0, 0), "Music", "library-windows11-music.ico"),
					//new ColorItem(Color.FromArgb(255, 0, 0, 0), "Pictures", "library-windows11-pictures.ico"),
					//new ColorItem(Color.FromArgb(255, 0, 0, 0), "TV", "library-windows11-tv.ico"),
					//new ColorItem(Color.FromArgb(255, 0, 0, 0), "Videos", "library-windows11-videos.ico"),
					//new ColorItem(Color.FromArgb(255, 0, 0, 0), "Drive", "drive-windows10-drive.ico"),
					//new ColorItem(Color.FromArgb(255, 0, 0, 0), "Windows", "drive-windows10-windows.ico"),
                    //new ColorItem(Color.FromArgb(255, 255,206,60), "Yellow", "folder-windows10-yellow.ico"),
                    //new ColorItem(Color.FromArgb(255, 228,60,41), "Dark Red", "folder-windows10-darkred.ico"),
                    //new ColorItem(Color.FromArgb(255, 237,111,15), "Dark Orange", "folder-windows10-darkorange.ico"),
                    //new ColorItem(Color.FromArgb(255, 62,158,74), "Dark Green", "folder-windows10-darkgreen.ico"),
                    //new ColorItem(Color.FromArgb(255, 39,147,142), "Dark Teal", "folder-windows10-darkteal.ico"),
                    //new ColorItem(Color.FromArgb(255, 31,132,208), "Dark Blue", "folder-windows10-darkblue.ico"),
                    //new ColorItem(Color.FromArgb(255, 153,96,198), "Dark Purple", "folder-windows10-darkpurple.ico"),
                    //new ColorItem(Color.FromArgb(255, 206,85,185), "Dark Pink", "folder-windows10-darkpink.ico"),
                    //new ColorItem(Color.FromArgb(255, 176,183,186), "Grey", "folder-windows10-grey.ico"),
                    //new ColorItem(Color.FromArgb(255, 255,188,178), "Light Red", "folder-windows10-lightred.ico"),
                    //new ColorItem(Color.FromArgb(255, 255,191,132), "Light Orange", "folder-windows10-lightorange.ico"),
                    //new ColorItem(Color.FromArgb(255, 142,210,144), "Light Green", "folder-windows10-lightgreen.ico"),
                    //new ColorItem(Color.FromArgb(255, 122,209,205), "Light Teal", "folder-windows10-lightteal.ico"),
                    //new ColorItem(Color.FromArgb(255, 134,200,247), "Light Blue", "folder-windows10-lightblue.ico"),
                    //new ColorItem(Color.FromArgb(255, 212,175,246), "Light Purple", "folder-windows10-lightpurple.ico"),
                    //new ColorItem(Color.FromArgb(255, 247,170,231), "Light Pink", "folder-windows10-lightpink.ico"),
                    //new ColorItem(Color.FromArgb(255, 0, 0, 0), "Documents", "library-windows10-documents.ico"),
					//new ColorItem(Color.FromArgb(255, 0, 0, 0), "Music", "library-windows10-music.ico"),
					//new ColorItem(Color.FromArgb(255, 0, 0, 0), "Pictures", "library-windows10-pictures.ico"),
					//new ColorItem(Color.FromArgb(255, 0, 0, 0), "TV", "library-windows10-tv.ico"),
					//new ColorItem(Color.FromArgb(255, 0, 0, 0), "Videos", "library-windows10-videos.ico")
                };

		public MainWindow()
		{
			this.InitializeComponent();

			ExtendsContentIntoTitleBar = true;
			this.SetTitleBar(AppTitleBar);
		}

		private void Color_Checked(object sender, RoutedEventArgs e)
		{
			if (sender is RadioButton rb && rb.DataContext is ColorItem item)
			{
				SelectedText.Text = $"You've selected the color—{item.Name}.";
			}
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



		private void ApplyButton_Click(object sender, RoutedEventArgs e)
		{
            MyInfoBar.IsOpen = false;

            var selectedRadio = FindSelectedRadioButton(ColorsList);
			string iconFile = string.Empty;
			if (selectedRadio?.DataContext is ColorItem colorItem)
			{
				iconFile = $"{colorItem.IconPath}";
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

			try
			{
                System.IO.File.WriteAllText(dskFile, string.Format("[.ShellClassInfo]\r\nIconResource=.icon.ico,0\r\nIconFile=.icon.ico\r\nIconIndex=0\r\n"));
                System.IO.FileAttributes attrs1 = System.IO.File.GetAttributes(dskFile);
                attrs1 |= System.IO.FileAttributes.Hidden | System.IO.FileAttributes.System;
                System.IO.File.SetAttributes(dskFile, attrs1);

                System.IO.File.Copy(iconPath, copyPath);
                System.IO.FileAttributes attrs2 = System.IO.File.GetAttributes(copyPath);
                attrs2 |= System.IO.FileAttributes.Hidden | System.IO.FileAttributes.System;
                System.IO.File.SetAttributes(copyPath, attrs2);

                var dirInfo = new System.IO.DirectoryInfo(pathAddr);
                dirInfo.Attributes |= System.IO.FileAttributes.ReadOnly;
                System.IO.Directory.SetLastWriteTime(pathAddr, DateTime.Now);
            }
            catch (Exception ex)
			{
                MyInfoBar.Title = "Error 3";
                MyInfoBar.Message = ex.Message;
                MyInfoBar.IsOpen = true;
                return;
            }
            Close();
        }

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
            Close();
		}

		private async void Grid_Drop(object sender, DragEventArgs e)
		{
			// Check if the dropped data contains files
			if (e.DataView.Contains(StandardDataFormats.StorageItems))
			{
				// Get the file(s) from the DataPackage
				var items = await e.DataView.GetStorageItemsAsync();
				if (items.Count > 0)
				{
					// Display the first file's name
					var storageFile = items[0] as StorageFile;
					if (storageFile != null)
					{
						SelectedText.Text = $"File: {storageFile.Name}";
					}
				}
			}
		}

		private void Grid_DragOver(object sender, DragEventArgs e)
		{
			// This is not being called
			e.AcceptedOperation = DataPackageOperation.Copy;
		}

		int previousSelectedIndex;
        System.Type pageType;
        private void MySelectorBar_SelectionChanged(SelectorBar sender, SelectorBarSelectionChangedEventArgs args)
        {
            SelectorBarItem selectedItem = sender.SelectedItem;
            int currentSelectedIndex = sender.Items.IndexOf(selectedItem);
            Tab1Content.Visibility = currentSelectedIndex == 0 ? Visibility.Visible : Visibility.Collapsed;
            Tab2Content.Visibility = currentSelectedIndex == 1 ? Visibility.Visible : Visibility.Collapsed;
        }

    }
}