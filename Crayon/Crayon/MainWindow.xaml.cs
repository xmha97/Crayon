using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
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
						new ColorItem(Colors.Yellow, "Yellow", "folder-yellow.ico"),
						new ColorItem(Colors.Red, "Red", "folder-red.ico"),
						new ColorItem(Colors.Green, "Green", "folder-green.ico"),
						new ColorItem(Colors.Blue, "Blue", "folder-blue.ico"),
						new ColorItem(Colors.Orange, "Orange", "folder-orange.ico"),
						new ColorItem(Colors.Purple, "Purple", "folder-purple.ico"),
						new ColorItem(Colors.Pink, "Pink", "folder-pink.ico"),
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
						if (selectedRadio?.DataContext is ColorItem colorItem)
						{
								SelectedText.Text = $"{colorItem.IconPath}";
						}
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
		}
}