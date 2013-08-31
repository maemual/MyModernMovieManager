using System;
using System.Collections.Generic;
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
using System.Collections.ObjectModel;

namespace Fish.MovieManager.UI
{
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow : Window
	{
		const int numTag = 31; 
		string[] movieTagstr = new string[numTag]
		{
			"全部类型",
			"剧情片",			"喜剧片",
			"动作片",			"爱情片",
			"科幻片",			"动画片",
			"悬疑片",			"惊悚片",
			"恐怖片",			"记录片",
			"短片" ,			"情色片",
			"同性片",			"音乐片",
			"歌舞片",			"家庭片",
			"儿童片",			"传记片",
			"历史片",			"战争片",
			"犯罪片",			"西部片",
			"奇幻片",			"冒险片",
			"灾难片",			"武侠片",
			"古装片",			"鬼怪片",
			"运动片",			"戏曲片"};
		public MainWindow()
		{
			InitializeComponent();
			//image
			ImageBrush ima = new ImageBrush();
			ima.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/backgroud.PNG"));
			ima.Stretch = Stretch.Fill;
			this.Background = ima;
			
			//list
			for (int i = 0; i < numTag;i++ )
				this.movieTagList.Items.Add(movieTagstr[i]);
			
			//getData
			ObservableCollection<DoubanMovieInfo.Storage.DoubanMovieInfo> moviedata = GetData();

			//Bind the DataGrid to the customer data
			movieGrid.DataContext = moviedata;
			movieGrid.LoadingRow += new EventHandler<DataGridRowEventArgs>(dataGrid_LoadingRow);
			movieGrid.MouseDoubleClick += new MouseButtonEventHandler(Open_Click);
			//movieGrid.MouseLeftButtonUp += new MouseButtonEventHandler(Movie_Update); 
		}

		//loading in datagrid
		private ObservableCollection<DoubanMovieInfo.Storage.DoubanMovieInfo> GetData()
		{
			var movie = new ObservableCollection<DoubanMovieInfo.Storage.DoubanMovieInfo>();
			movie.Add(new DoubanMovieInfo.Storage.DoubanMovieInfo { doubanId = 1, title = "hello", originalTitle = "hi", rating = 8.0, ratingsCount = 100 });
			movie.Add(new DoubanMovieInfo.Storage.DoubanMovieInfo { doubanId = 2, title = "jiang", originalTitle = "yicheng", rating = 9.0, ratingsCount = 20 });
			return movie;
		}
		public void dataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
		{
			e.Row.Header = e.Row.GetIndex() + 1;
		}

		private void movieGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			DoubanMovieInfo.Storage.DoubanMovieInfo tmp = movieGrid.SelectedItem as DoubanMovieInfo.Storage.DoubanMovieInfo;
			string strfile = "C:\\Users\\withwind\\Pictures\\test\\" + tmp.doubanId + ".jpg";
			BitmapImage image = new BitmapImage(new Uri(strfile, UriKind.Absolute));
			
		}

		private void movieDirectors_Click(object sender, RoutedEventArgs e)
		{

		}

		private void movieActors_Click(object sender, RoutedEventArgs e)
		{

		}

		private void Open_Click(object sender, RoutedEventArgs e)
		{

		}
		private void MenuItem_Add_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				string strfile = "";
				Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
				dlg.Title = "选择文件";
				dlg.Filter = "Video Files (*.wmv)|*.wmv";
				if (dlg.ShowDialog() == true)
				{
					strfile = dlg.FileName;
				}
				if (strfile != "")
				{
					MessageBox.Show("the path : ", strfile);
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
			MessageBox.Show("to del id :" + tmp.title.ToString());
		}

	}
}
