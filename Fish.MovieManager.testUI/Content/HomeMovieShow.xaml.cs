using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
///
using System.Diagnostics;
using FirstFloor.ModernUI.Windows.Controls;

namespace Fish.MovieManager.testUI.Content
{
	/// <summary>
	/// Interaction logic for HomeMovieShow.xaml
	/// </summary>
	public partial class HomeMovieShow : UserControl
	{
		public HomeMovieShow()
		{
			InitializeComponent();

			ObservableCollection<DoubanMovieInfo.Storage.DoubanMovieInfo> moviedata = GetData();

			//Bind the DataGrid to the customer data
			movieGrid.DataContext = moviedata;
			movieGrid.LoadingRow += new EventHandler<DataGridRowEventArgs>(dataGrid_LoadingRow);
			movieGrid.MouseDoubleClick += new MouseButtonEventHandler(Open_Click);
			movieGrid.MouseLeftButtonUp += new MouseButtonEventHandler(Movie_Update); 
		}

		private void Movie_Update(object sender, MouseButtonEventArgs e)
		{
			DoubanMovieInfo.Storage.DoubanMovieInfo tmp = movieGrid.SelectedItem as DoubanMovieInfo.Storage.DoubanMovieInfo;
			string strfile = "C:\\Users\\withwind\\Pictures\\test\\" + tmp.doubanId + ".jpg"; 
			BitmapImage image = new BitmapImage(new Uri(strfile, UriKind.Absolute));
			this.movie_image.Source = image;
		}

		public void dataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
		{
			e.Row.Header = e.Row.GetIndex() + 1;
		}

		private ObservableCollection<DoubanMovieInfo.Storage.DoubanMovieInfo> GetData()
		{
			var movie = new ObservableCollection<DoubanMovieInfo.Storage.DoubanMovieInfo>();
			movie.Add(new DoubanMovieInfo.Storage.DoubanMovieInfo { doubanId = 1, title = "hello", originalTitle = "hi", rating = 8.0, ratingsCount = 100 });
			movie.Add(new DoubanMovieInfo.Storage.DoubanMovieInfo { doubanId = 2, title = "jiang", originalTitle = "yicheng", rating = 9.0, ratingsCount = 20 });
			return movie;
		}

		private void Open_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("how to open the movie");
		}

		private void MenuItem_Add_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				string strfile = "";
				Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
				dlg.Filter = "所有文本文件(*.jpg;*.gif;*.png)|*.jpg;*.gif;*.png";
				if (dlg.ShowDialog() == true)
				{
					strfile = dlg.FileName;
				}
				if (strfile != "")
				{
					BitmapImage image = new BitmapImage(new Uri(strfile, UriKind.Absolute));
					this.movie_image.Source = image;
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		private void MenuItem_Del_Click(object sender, RoutedEventArgs e)
		{
			DoubanMovieInfo.Storage.DoubanMovieInfo tmp = movieGrid.SelectedItem as DoubanMovieInfo.Storage.DoubanMovieInfo;

			MessageBoxResult confirmToDel = MessageBox.Show("确定要删除所选行吗？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question);
			if (confirmToDel == MessageBoxResult.Yes)
			{
				//System.Data.DataRowView drv = movieGrid.SelectedItem as System.Data.DataRowView;
				//if(drv != null)
				//	drv.Delete();
				//var a = movieGrid.SelectedItems;
				//a.Clear();
				/*foreach (var aa in a)
				{
					var b = aa as System.Data.DataRowView;
					b.Delete();
				}*/
				MessageBox.Show("how to del movie in sql the id : " + tmp.doubanId);
				ObservableCollection<DoubanMovieInfo.Storage.DoubanMovieInfo> moviedata = GetData();
			}
			else
			{
				return;
			}
		}
	}
}
