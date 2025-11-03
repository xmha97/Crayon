using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.UI;

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
					new ColorItem(Color.FromArgb(255, 228,60,41), "Dark Red", "folder-windows11-dark-red.ico"),
					new ColorItem(Color.FromArgb(255, 237,111,15), "Dark Orange", "folder-windows11-dark-orange.ico"),
					new ColorItem(Color.FromArgb(255, 62,158,74), "Dark Green", "folder-windows11-dark-green.ico"),
					new ColorItem(Color.FromArgb(255, 39,147,142), "Dark Teal", "folder-windows11-dark-teal.ico"),
					new ColorItem(Color.FromArgb(255, 31,132,208), "Dark Blue", "folder-windows11-dark-blue.ico"),
					new ColorItem(Color.FromArgb(255, 153,96,198), "Dark Purple", "folder-windows11-dark-purple.ico"),
					new ColorItem(Color.FromArgb(255, 206,85,185), "Dark Pink", "folder-windows11-dark-pink.ico"),
					new ColorItem(Color.FromArgb(255, 176,183,186), "Grey", "folder-windows11-grey.ico"),
					new ColorItem(Color.FromArgb(255, 255,188,178), "Light Red", "folder-windows11-light-red.ico"),
					new ColorItem(Color.FromArgb(255, 255,191,132), "Light Orange", "folder-windows11-light-orange.ico"),
					new ColorItem(Color.FromArgb(255, 142,210,144), "Light Green", "folder-windows11-light-green.ico"),
					new ColorItem(Color.FromArgb(255, 122,209,205), "Light Teal", "folder-windows11-light-teal.ico"),
					new ColorItem(Color.FromArgb(255, 134,200,247), "Light Blue", "folder-windows11-light-blue.ico"),
					new ColorItem(Color.FromArgb(255, 212,175,246), "Light Purple", "folder-windows11-light-purple.ico"),
					new ColorItem(Color.FromArgb(255, 247,170,231), "Light Pink", "folder-windows11-light-pink.ico"),
					//new ColorItem(Color.FromArgb(255, 0, 0, 0), "Documents", "library-windows11-documents.ico"),
					//new ColorItem(Color.FromArgb(255, 0, 0, 0), "Music", "library-windows11-music.ico"),
					//new ColorItem(Color.FromArgb(255, 0, 0, 0), "Pictures", "library-windows11-pictures.ico"),
					//new ColorItem(Color.FromArgb(255, 0, 0, 0), "TV", "library-windows11-tv.ico"),
					//new ColorItem(Color.FromArgb(255, 0, 0, 0), "Videos", "library-windows11-videos.ico"),
					//new ColorItem(Color.FromArgb(255, 0, 0, 0), "Drive", "drive-windows10-drive.ico"),
					//new ColorItem(Color.FromArgb(255, 0, 0, 0), "Windows", "drive-windows10-windows.ico"),
                    //new ColorItem(Color.FromArgb(255, 255,206,60), "Yellow", "folder-windows10-yellow.ico"),
                    //new ColorItem(Color.FromArgb(255, 228,60,41), "Dark Red", "folder-windows10-dark-red.ico"),
                    //new ColorItem(Color.FromArgb(255, 237,111,15), "Dark Orange", "folder-windows10-dark-orange.ico"),
                    //new ColorItem(Color.FromArgb(255, 62,158,74), "Dark Green", "folder-windows10-dark-green.ico"),
                    //new ColorItem(Color.FromArgb(255, 39,147,142), "Dark Teal", "folder-windows10-dark-teal.ico"),
                    //new ColorItem(Color.FromArgb(255, 31,132,208), "Dark Blue", "folder-windows10-dark-blue.ico"),
                    //new ColorItem(Color.FromArgb(255, 153,96,198), "Dark Purple", "folder-windows10-dark-purple.ico"),
                    //new ColorItem(Color.FromArgb(255, 206,85,185), "Dark Pink", "folder-windows10-dark-pink.ico"),
                    //new ColorItem(Color.FromArgb(255, 176,183,186), "Grey", "folder-windows10-grey.ico"),
                    //new ColorItem(Color.FromArgb(255, 255,188,178), "Light Red", "folder-windows10-light-red.ico"),
                    //new ColorItem(Color.FromArgb(255, 255,191,132), "Light Orange", "folder-windows10-light-orange.ico"),
                    //new ColorItem(Color.FromArgb(255, 142,210,144), "Light Green", "folder-windows10-light-green.ico"),
                    //new ColorItem(Color.FromArgb(255, 122,209,205), "Light Teal", "folder-windows10-light-teal.ico"),
                    //new ColorItem(Color.FromArgb(255, 134,200,247), "Light Blue", "folder-windows10-light-blue.ico"),
                    //new ColorItem(Color.FromArgb(255, 212,175,246), "Light Purple", "folder-windows10-light-purple.ico"),
                    //new ColorItem(Color.FromArgb(255, 247,170,231), "Light Pink", "folder-windows10-light-pink.ico"),
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
			var selectedRadio = FindSelectedRadioButton(ColorsList);
			string iconFile = string.Empty;
			if (selectedRadio?.DataContext is ColorItem colorItem)
			{
				iconFile = $"{colorItem.IconPath}";
			}

			string[] cmdLine = Environment.GetCommandLineArgs();
			string pathAddr = string.Empty;
			if (cmdLine.Length > 1)
			{
				pathAddr = Environment.GetCommandLineArgs()[1];
			}

			string output = string.Empty;

			if (iconFile == string.Empty)
			{
				output += "NoIcon\n";
			}
			else
			{
				output += $"{iconFile}\n";
			}

			if (pathAddr == string.Empty)
			{
				output += "NoAddress\n";
			}
			else
			{
				output += $"{pathAddr}\n";
			}

			string dskFile = System.IO.Path.Combine(pathAddr, "desktop.ini");

			if (System.IO.File.Exists(dskFile))
			{
				output += System.IO.File.ReadAllText(dskFile) + "\n";
			}
			else
			{
				output += "NoFile\n";
			}

			SelectedText.Text = output;
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