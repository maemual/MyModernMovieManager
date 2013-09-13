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
	public class DataListView
	{
		/// <summary>
		/// 豆瓣上的条目ID
		/// </summary>
		public int doubanId { get; set; }
		/// <summary>
		/// 文件自增长ID
		/// </summary>
		public int id { get; set; }
        /// <summary>
        /// 中文名
        /// </summary>
        public string title { get; set; }
		/// <summary>
        /// 视频文件时长
        /// </summary>
        public string duration { get; set; }
		/// <summary>
        /// 文件拓展名
        /// </summary>
        public string extension { get; set; }
        /// <summary>
        /// 豆瓣评分
        /// </summary>
		public double rating { get; set; }
         /// <summary>
        /// 文件存储路径
        /// </summary>
        public string path { get; set; }
        /// <summary>
        /// 用户的评分
        /// </summary>
        public int userRating { get; set; }
		/// <summary>
		/// 电影海报图存储的路径
		/// </summary>
		public virtual string image { get; set; }
	}
	public partial class MainWindow
	{
		public static Theme currentTheme = Theme.Light;
		public static Accent currentAccent = ThemeManager.DefaultAccents.First(x => x.Name == "Green");
		const int numTag = 31;
		string[] movieTagstr = new string[numTag]
		{
			"全部类型",
			"剧情",			"喜剧",
			"动作",			"爱情",
			"科幻",			"动画",
			"悬疑",			"惊悚",
			"恐怖",			"记录片",
			"短片" ,		"情色",
			"同性",			"音乐",
			"歌舞",			"家庭",
			"儿童",			"传记",
			"历史",			"战争",
			"犯罪",			"西部",
			"奇幻",			"冒险",
			"灾难",			"武侠",
			"古装",			"鬼怪",
			"运动",			"戏曲"};
		string[] movieTagstrShow = new string[numTag]
		{
			"全部类型",
			"剧情片",			"喜剧片",
			"动作片",			"爱情片",
			"科幻片",			"动画片",
			"悬疑片",			"惊悚片",
			"恐怖片",			"记录片",
			"短片",				"情色片",
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
			ima.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/blue1.PNG"));
			ima.Stretch = Stretch.Fill;
			this.Background = ima;
			
			//list
			for (int i = 0; i < numTag;i++ )
				this.movieTagList.Items.Add(movieTagstrShow[i]);

			//update
			datagrid_Update(GetData());

			//datagrid_getData
			//ObservableCollection<VideoFileInfo.Storage.VideoFileInfo> moviedata = GetData();
			//movieGrid.DataContext = moviedata;
			//cover view
			//this.DataContext = new MainViewModel();
			
			//row number
			movieGrid.LoadingRow += new EventHandler<DataGridRowEventArgs>(dataGrid_LoadingRow);
			
			//open
			movieGrid.MouseDoubleClick += new MouseButtonEventHandler(Open_Click);
		}

		//update
		public void datagrid_Update(List<Fish.MovieManager.VideoFileInfo.Storage.VideoFileInfo> filelist)
		{
			var datalist = new List<DataListView>();
			foreach (var tmp in filelist)
			{
				var douban_tmp = Fish.MovieManager.DoubanControl.Class1.Instance.GetDoubanMovieInfo(tmp.doubanId);
				var view = new DataListView();
				view.doubanId = douban_tmp.doubanId;
				view.title = douban_tmp.title;
				view.rating = douban_tmp.rating;
				view.image = douban_tmp.image;
				view.id = tmp.id;
				view.extension = tmp.extension; 
				view.duration = tmp.duration;
				view.path = tmp.path;
				
				datalist.Add(view);
			}
			movieList.DataContext = datalist;
			movieGrid.DataContext = datalist;
		}
		public void datagrid_Update_Fun()
		{
			if (this.movieTagList.SelectedItem == null || this.movieTagList.SelectedIndex == -1)
			{
				datagrid_Update(GetData());
				return;
			}
			string str = movieTagstr[this.movieTagList.SelectedIndex];
			if (str == movieTagstr[0])
			{
				datagrid_Update(GetData());
			}
			else
			{
				var tmp = Fish.MovieManager.TagControl.Class1.Instance.GetMovieByTag(str);
				datagrid_Update(tmp);
			}
		}

		//loading in datagrid
		private List<VideoFileInfo.Storage.VideoFileInfo> GetData()
		{
			//var movie = new ObservableCollection<VideoFileInfo.Storage.VideoFileInfo>();
			var movie = new List<VideoFileInfo.Storage.VideoFileInfo>();
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
		//line number
		public void dataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
		{
			e.Row.Header = e.Row.GetIndex() + 1;
		}

		//tagListBox_MouseButtonUp
		private void tagListBox_MouseButtonUp(object sendeer, MouseButtonEventArgs e)
		{
			datagrid_Update_Fun();
		}
		
		//show movie
		private void movieGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			var filetmp = getSelectFileInfo();
			var doubantmp = getSelectDoubanInfo();
			if (filetmp != null)
			{
				this.text_doubanid.Text = filetmp.doubanId.ToString();
				this.text_extension.Text = filetmp.extension;
				this.text_bitRate.Text = filetmp.bitRate.ToString();
				this.text_videoBitRate.Text = filetmp.videoBitRate.ToString();
				this.text_audioBitRate.Text = filetmp.audioBitRate.ToString();
				this.text_frameRate.Text = filetmp.frameRate.ToString();
				this.text_audioFormat.Text = filetmp.audioFormat;
				this.text_videoFormat.Text = filetmp.videoFormat;
				this.text_heigh.Text = filetmp.height.ToString();
				this.text_width.Text = filetmp.width.ToString();
				this.text_totalFrames.Text = filetmp.totalFrames.ToString();
				this.text_md5.Text = filetmp.md5;	
			}
			if (doubantmp != null)
			{
				this.text_title.Text = doubantmp.title;
				this.text_originalTitle.Text = doubantmp.originalTitle;
				this.text_aka.Text = doubantmp.aka;
				//!!!this.text_tag.Text = doubantmp
				this.text_year.Text = doubantmp.year.ToString();
				this.text_countries.Text = doubantmp.countries;
				//this.text_directors.Text = doubantmp.directors;
				//this.text_actors.Text = doubantmp.a
				this.text_rating.Text = string.Format("{0:N1}",doubantmp.rating);
				this.text_ratingCount.Text = doubantmp.ratingsCount.ToString();
				this.text_doubanSite.Text = doubantmp.doubanSite;
				this.text_summary.Text = doubantmp.summary;
				BitmapImage bitmap = new BitmapImage();
				bitmap.BeginInit();
				bitmap.UriSource = new Uri(doubantmp.image);
				bitmap.CacheOption = BitmapCacheOption.OnLoad;
				this.movie_ShowImage.Source = bitmap;
				bitmap.EndInit();				
			}
		}

		//get now tab select movie
		private DoubanMovieInfo.Storage.DoubanMovieInfo getSelectDoubanInfo() 
		{
			int now = movie_Tab.SelectedIndex;
			int showIndex = 0;
			if (now == 0)
			{
				showIndex = movieList.SelectedIndex;
			}
			else
			{
				showIndex = movieGrid.SelectedIndex;
			}
			var t = movieGrid.Items[showIndex] as DataListView;
			DoubanMovieInfo.Storage.DoubanMovieInfo tmp = Fish.MovieManager.DoubanControl.Class1.Instance.GetDoubanMovieInfo(t.doubanId);
			return tmp;
		}
		private VideoFileInfo.Storage.VideoFileInfo getSelectFileInfo()
		{
			int now = movie_Tab.SelectedIndex;
			int showIndex = 0;
			if (now == 0)
			{
				showIndex = movieList.SelectedIndex;
			}
			else
			{
				showIndex = movieGrid.SelectedIndex;
			}
			var t = movieGrid.Items[showIndex] as DataListView;
			VideoFileInfo.Storage.VideoFileInfo tmp = Fish.MovieManager.VideoControl.Class1.Instance.GetFileInfo(t.id);
			return tmp;
		}

		//button director actor 
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

		//open
		private void Open_Click(object sender, RoutedEventArgs e)
		{
			VideoFileInfo.Storage.VideoFileInfo tmp = getSelectFileInfo();
			if (tmp != null)
			{
				Fish.MovieManager.VideoFileInfo.Class1.Instance.PlayVideo(tmp.path);
			}
		}

		//add
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
				}
				if (strfile != null)
				{
					Fish.MovieManager.VideoControl.Class1.Instance.ImportFile(strfile);
					datagrid_Update_Fun();
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
			if (fbd.ShowDialog().ToString() == "OK")
			{
				MessageBox.Show(fbd.SelectedPath.ToString());
				Fish.MovieManager.VideoControl.Class1.Instance.ImportFiles(fbd.SelectedPath.ToString());
				datagrid_Update_Fun();
			}
		}

		//delete
		private void MenuItem_Del_Click(object sender, RoutedEventArgs e)
		{
			var tmp = getSelectFileInfo();
			if (tmp != null)
			{
				Fish.MovieManager.VideoControl.Class1.Instance.DeleteFile(tmp.id);
				datagrid_Update_Fun();
			}
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

		//cal md5
		private void md5_Click(object sender, RoutedEventArgs e)
		{
			MessageBoxResult confirmToCal = MessageBox.Show("计算电影md5值需要一定时间，是否确认计算？", "提示", MessageBoxButton.YesNo);
			if (confirmToCal == MessageBoxResult.Yes)
			{
				//var filetmp = getSelectFileInfo();
				//string md5tmp = Fish.MovieManager.VideoControl.Class1.Instance.SetMd5(filetmp.id);
				//if (md5tmp != null)
				//{
				//	this.text_md5.Text = md5tmp;
				//}
			}
		}
	}
}
