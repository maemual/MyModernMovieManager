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
using NHibernate;
using NHibernate.Linq;
//
using MahApps;
using MahApps.Metro.Controls;
using MahApps.Metro;
using Microsoft.WindowsAPICodePack.Shell;
using Fish.MovieManager.UI;

namespace Fish.MovieManager.UI
{
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow
	{
		public static Theme currentTheme = Theme.Light;
		public static Accent currentAccent = ThemeManager.DefaultAccents.First(x => x.Name == "Green");
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

			//
			

			//image
			ImageBrush ima = new ImageBrush();
			ima.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/blue1.PNG"));
			ima.Stretch = Stretch.Fill;
			this.Background = ima;
			
			//list
			for (int i = 0; i < numTag;i++ )
				this.movieTagList.Items.Add(movieTagstr[i]);
			
			//datagrid_getData
			ObservableCollection<VideoFileInfo.Storage.VideoFileInfo> moviedata = GetData();
			movieGrid.DataContext = moviedata;
			movieGrid.LoadingRow += new EventHandler<DataGridRowEventArgs>(dataGrid_LoadingRow);/*row number*/
			
			//open
			movieGrid.MouseDoubleClick += new MouseButtonEventHandler(Open_Click);

			//cover view
			this.DataContext = new MainViewModel();
		}


		//loading in datagrid
		private ObservableCollection<VideoFileInfo.Storage.VideoFileInfo> GetData()
		{
			var movie = new ObservableCollection<VideoFileInfo.Storage.VideoFileInfo>();
			//movie.Add(new DoubanMovieInfo.Storage.DoubanMovieInfo { doubanId = 1, title = "hello", originalTitle = "hi", rating = 8.0, ratingsCount = 100, year = 1900 });
			//movie.Add(new DoubanMovieInfo.Storage.DoubanMovieInfo { doubanId = 2, title = "jiang", originalTitle = "yicheng", rating = 9.0, ratingsCount = 20, year = 2000 });
			
			using (var session = Fish.MovieManager.VideoFileInfo.Storage.StorageManager.Instance.OpenSession())
			{
				var tmp = session.Query<VideoFileInfo.Storage.VideoFileInfo>().ToList();
				foreach (var i in tmp)
				{
					movie.Add(i);
				}
			}
			return movie;
		}
		public void dataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
		{
			e.Row.Header = e.Row.GetIndex() + 1;
		}


		//tagListBox_MouseButtonUp
		private void tagListBox_MouseButtonUp(object sendeer, MouseButtonEventArgs e)
		{
			string str = this.movieTagList.SelectedItem.ToString();
			MessageBox.Show("only show :" + str);
		}
		private void movieGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			VideoFileInfo.Storage.VideoFileInfo t = movieGrid.SelectedItem as VideoFileInfo.Storage.VideoFileInfo;
			if (t != null)
			{
				this.file_audioBitRate.Text = t.audioBitRate.ToString();
				this.file_audioFormat.Text = t.audioFormat;
				this.file_extension.Text = t.extension;
			}
			DoubanMovieInfo.Storage.DoubanMovieInfo tmp = movieGrid.SelectedItem as DoubanMovieInfo.Storage.DoubanMovieInfo;
			if (tmp != null)
			{
			//string strfile = "C:\\Users\\withwind\\Pictures\\test\\" + tmp.doubanId + ".jpg";
			//	string strfile = tmp.image;
			//	BitmapImage image = new BitmapImage(new Uri(strfile, UriKind.Absolute));
			//	this.movieImage.Source = image;
			//导入
				//this.title.Text = tmp.title;
				//this.originalTitle.Text = tmp.originalTitle;
				//this.aka.Text = tmp.aka;
				//this.rating.Text = tmp.rating.ToString();
				//this.ratingCount.Text = tmp.ratingsCount.ToString();
				//this.doubanSite.Text = tmp.doubanSite;
				//this.year.Text = tmp.year.ToString();
				//this.countries.Text = tmp.countries;
				//this.summary.Text = tmp.summary;
				
			}
		}
		private void movieDirectors_Click(object sender, RoutedEventArgs e)
		{
			
		}

		private void movieActors_Click(object sender, RoutedEventArgs e)
		{
			var tmp = movieGrid.SelectedItem as VideoFileInfo.Storage.VideoFileInfo;
			if (tmp == null) return;
			Fish.MovieManager.UI.dialog.actorInfo_Dialog dlg = new dialog.actorInfo_Dialog(tmp.doubanId);
			dlg.Show();
		}

		private void Open_Click(object sender, RoutedEventArgs e)
		{
			VideoFileInfo.Storage.VideoFileInfo tmp = movieGrid.SelectedItem as VideoFileInfo.Storage.VideoFileInfo;
			if (tmp != null)
			{
				Fish.MovieManager.VideoFileInfo.Class1.Instance.PlayVideo(tmp.path);
			}
		}
		private void MenuItem_Add_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				string strfile = null;
				Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
				dlg.Title = "选择文件";
				dlg.Filter = "常见视频文件 |*.avi;*.wmv;*.wmp;*.asf;*.rm;*.ram;*.rmvb;*.ra;*rp;*.smi;*.mpg;*mpeg;*.dat;*.mp4;*.ts;*.vob;*.ifo;*ac3;*.dts | Windows Media 视频 (*.avi;*.wmv;*.wmp;*.asf)|*.avi;*.wmv;*.wmp;*.asf | Real (*.rm;*.ram;*.rmvb;*.ra;*rp;*.smi)|*.rm;*.ram;*.rmvb;*.ra;*rp;*.smi | MPEG 视频 (*.mpg;*mpeg;*.dat;*.mp4;*.ts)|*.mpg;*mpeg;*.dat;*.mp4;*.ts | DVD (*.vob;*.ifo;*ac3;*.dts)|*.vob;*.ifo;*ac3;*.dts";
				if (dlg.ShowDialog() == true)
				{
					strfile = dlg.FileName;
					MessageBox.Show("hello ~~");
				}
				if (strfile != null)
				{
					MessageBox.Show("the path : " + strfile);
					//Fish.MovieManager.MovieControl.Class1.Instance.ImportFiles(strfile);
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		private void MenuItem_AddFile_Click(object sender, RoutedEventArgs e)
		{
			System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
			fbd.ShowDialog();
			Fish.MovieManager.MovieControl.Class1.Instance.ImportFiles(fbd.SelectedPath.ToString());
		}
		private void MenuItem_Del_Click(object sender, RoutedEventArgs e)
		{
			var tmp = movieGrid.SelectedItem as VideoFileInfo.Storage.VideoFileInfo;
			MessageBox.Show("to del id :" + tmp.id.ToString());
		}

		//appearance
		private void Light_Click(object sender, RoutedEventArgs e)
		{
			currentTheme = Theme.Light;
			ThemeManager.ChangeTheme(this, currentAccent, Theme.Light);
		}
		private void Dark_Click(object sender, RoutedEventArgs e)
		{
			currentTheme = Theme.Dark;
			ThemeManager.ChangeTheme(this, currentAccent, Theme.Dark);
		}

		private void ChangeAccent(string accentName)
		{
			currentAccent = ThemeManager.DefaultAccents.First(x => x.Name == accentName);
			ThemeManager.ChangeTheme(this, currentAccent, currentTheme);
		}
		private void AccentRed(object sender, RoutedEventArgs e)
		{
			this.ChangeAccent("Red");
		}
		private void AccentGreen(object sender, RoutedEventArgs e)
		{
			this.ChangeAccent("Green");
			
		}
		private void AccentBlue(object sender, RoutedEventArgs e)
		{
			this.ChangeAccent("Blue");
		}
		private void AccentPurple(object sender, RoutedEventArgs e)
		{
			this.ChangeAccent("Purple");
		}
		private void AccentOrange(object sender, RoutedEventArgs e)
		{
			this.ChangeAccent("Orange");
		}

		private void About_Click(object sender, RoutedEventArgs e)
		{
			Fish.MovieManager.UI.dialog.about_Dialog dlg = new dialog.about_Dialog();
			dlg.Show();
		}
	}
}
