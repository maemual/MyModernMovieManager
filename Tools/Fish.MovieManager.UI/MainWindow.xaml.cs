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
using System.IO;

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
		public static Accent currentAccent = ThemeManager.DefaultAccents.First(x => x.Name == "Blue");
		public string currentColor = "blue";
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
			ima.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/blue.PNG"));
			ima.Stretch = Stretch.Fill;
			this.window_Grid.Background = ima;
			
			//list
			for (int i = 0; i < numTag;i++ )
				this.movieTagList.Items.Add(movieTagstrShow[i]);

			//update
			datagrid_Update(Fish.MovieManager.VideoControl.Class1.Instance.GetAllFileInfo());

			//row number
			movieGrid.LoadingRow += new EventHandler<DataGridRowEventArgs>(dataGrid_LoadingRow);
			
			//open
			movieGrid.MouseDoubleClick += new MouseButtonEventHandler(Open_Click);

			//loadtext
			LoadText();

			//about us flyout
			this.MyFlyoutCheck.IsChecked = false;

			//lock
			this.LockFlyoutCheck.IsChecked = false;
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
				if (tmp.userRating != -1)
				{
					view.userRating = tmp.userRating;
				}
				else
				{
					view.userRating = 0;
				}
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
				datagrid_Update(Fish.MovieManager.VideoControl.Class1.Instance.GetAllFileInfo());
				return;
			}
			string str = movieTagstr[this.movieTagList.SelectedIndex];
			if (str == movieTagstr[0])
			{
				datagrid_Update(Fish.MovieManager.VideoControl.Class1.Instance.GetAllFileInfo());
			}
			else
			{
				var tmp = Fish.MovieManager.TagControl.Class1.Instance.GetMovieByTag(str);
				datagrid_Update(tmp);
			}
		}

		///row number
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
				this.text_tag.Text = Fish.MovieManager.DoubanControl.Class1.Instance.GetMovieTag(doubantmp.doubanId);
				this.text_year.Text = doubantmp.year.ToString();
				this.text_countries.Text = doubantmp.countries;
				this.text_directors.Text = Fish.MovieManager.DoubanControl.Class1.Instance.GetDirectorName(doubantmp.doubanId);
				this.text_actors.Text = Fish.MovieManager.DoubanControl.Class1.Instance.GetActorName(doubantmp.doubanId);
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

				var direct_tmp = Fish.MovieManager.DoubanControl.Class1.Instance.GetDirectorInfo(doubantmp.directors);
				bitmap = new BitmapImage();
				bitmap.BeginInit();
				bitmap.UriSource = new Uri(direct_tmp.avatars);
				bitmap.CacheOption = BitmapCacheOption.OnLoad;
				this.diretorImage.Source = bitmap;
				bitmap.EndInit();
				this.direct_name.Text = direct_tmp.name;
				this.direct_nameEn.Text = direct_tmp.nameEn;
				this.direct_gender.Text = direct_tmp.gender;
				this.direct_bornPlace.Text = direct_tmp.bornPlace;

				var people = Fish.MovieManager.ActorControl.Class1.Instance.GetActorByID(doubantmp.doubanId);
				int actorNum = 0;
				foreach (var ptmp in people)
				{
						actorNum++;
						if (actorNum == 1)
						{
							bitmap = new BitmapImage();
							bitmap.BeginInit();
							bitmap.UriSource = new Uri(ptmp.avatars);
							bitmap.CacheOption = BitmapCacheOption.OnLoad;
							this.actor1Image.Source = bitmap;
							bitmap.EndInit();
							this.actor1Item.Header = ptmp.name;
							this.actor1_name.Text = ptmp.name;
							this.actor1_nameEn.Text = ptmp.nameEn;
							this.actor1_gender.Text = ptmp.gender;
							this.actor1_bornPlace.Text = ptmp.bornPlace;
						}
						else if (actorNum == 2)
						{
							bitmap = new BitmapImage();
							bitmap.BeginInit();
							bitmap.UriSource = new Uri(ptmp.avatars);
							bitmap.CacheOption = BitmapCacheOption.OnLoad;
							this.actor2Image.Source = bitmap;
							bitmap.EndInit();
							this.actor2Item.Header = ptmp.name;
							this.actor2_name.Text = ptmp.name;
							this.actor2_nameEn.Text = ptmp.nameEn;
							this.actor2_gender.Text = ptmp.gender;
							this.actor2_bornPlace.Text = ptmp.bornPlace;
						}
						else if (actorNum == 3)
						{
							bitmap = new BitmapImage();
							bitmap.BeginInit();
							bitmap.UriSource = new Uri(ptmp.avatars);
							bitmap.CacheOption = BitmapCacheOption.OnLoad;
							this.actor3Image.Source = bitmap;
							bitmap.EndInit();
							this.actor3Item.Header = ptmp.name;
							this.actor3_name.Text = ptmp.name;
							this.actor3_nameEn.Text = ptmp.nameEn;
							this.actor3_gender.Text = ptmp.gender;
							this.actor3_bornPlace.Text = ptmp.bornPlace;
						}
						else 
						{
							bitmap = new BitmapImage();
							bitmap.BeginInit();
							bitmap.UriSource = new Uri(ptmp.avatars);
							bitmap.CacheOption = BitmapCacheOption.OnLoad;
							this.actor4Image.Source = bitmap;
							bitmap.EndInit();
							this.actor4Item.Header = ptmp.name;
							this.actor4_name.Text = ptmp.name;
							this.actor4_nameEn.Text = ptmp.nameEn;
							this.actor4_gender.Text = ptmp.gender;
							this.actor4_bornPlace.Text = ptmp.bornPlace;
						}
					}
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
			if (showIndex < 0) return null;
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
			if (showIndex < 0) return null;
			var t = movieGrid.Items[showIndex] as DataListView;
			VideoFileInfo.Storage.VideoFileInfo tmp = Fish.MovieManager.VideoControl.Class1.Instance.GetFileInfo(t.id);
			return tmp;
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
			LockFun();
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
			UnLockFun();
		}
		private void MenuItem_AddFile_Click(object sender, RoutedEventArgs e)
		{
			System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
			LockFun();
			if (fbd.ShowDialog().ToString() == "OK")
			{
				Fish.MovieManager.VideoControl.Class1.Instance.ImportFiles(fbd.SelectedPath.ToString());
				datagrid_Update_Fun();
			}
			UnLockFun();
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
			ImageBrush ima = new ImageBrush();
			ima.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/" + currentColor + ".PNG"));
			ima.Stretch = Stretch.Fill;
			this.window_Grid.Background = ima;
			ThemeManager.ChangeTheme(this, currentAccent, Theme.Light);
		}
		private void Dark_Click(object sender, RoutedEventArgs e)
		{
			currentTheme = Theme.Dark;
			this.window_Grid.Background = null;
			ThemeManager.ChangeTheme(this, currentAccent, Theme.Dark);
		}
		private void ChangeAccent(string accentName)
		{
			currentAccent = ThemeManager.DefaultAccents.First(x => x.Name == accentName);
			ThemeManager.ChangeTheme(this, currentAccent, currentTheme);
		}
		private void AccentRed(object sender, RoutedEventArgs e)
		{
			currentColor = "red";
			this.ChangeAccent("Red");
			if (currentTheme == Theme.Dark) return;
			ImageBrush ima = new ImageBrush();
			ima.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/red.PNG"));
			ima.Stretch = Stretch.Fill;
			this.window_Grid.Background = ima;
		}
		private void AccentGreen(object sender, RoutedEventArgs e)
		{
			currentColor = "green";
			this.ChangeAccent("Green");
			if (currentTheme == Theme.Dark) return;
			ImageBrush ima = new ImageBrush();
			ima.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/green.PNG"));
			ima.Stretch = Stretch.Fill;
			this.window_Grid.Background = ima;
		}
		private void AccentBlue(object sender, RoutedEventArgs e)
		{
			currentColor = "blue";
			this.ChangeAccent("Blue");
			if (currentTheme == Theme.Dark) return;
			ImageBrush ima = new ImageBrush();
			ima.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/blue.PNG"));
			ima.Stretch = Stretch.Fill;
			this.window_Grid.Background = ima;
		}
		private void AccentPurple(object sender, RoutedEventArgs e)
		{
			this.ChangeAccent("Purple");
			currentColor = "purple";
			if (currentTheme == Theme.Dark) return;
			ImageBrush ima = new ImageBrush();
			ima.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/purple.PNG"));
			ima.Stretch = Stretch.Fill;
			this.window_Grid.Background = ima;
		}
		private void AccentOrange(object sender, RoutedEventArgs e)
		{
			this.ChangeAccent("Orange");
			currentColor = "orange";
			if (currentTheme == Theme.Dark) return;
			ImageBrush ima = new ImageBrush();
			ima.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/orange.PNG"));
			ima.Stretch = Stretch.Fill;
			this.window_Grid.Background = ima;
		}

		//cal md5
		private void md5_Click(object sender, RoutedEventArgs e)
		{
			MessageBoxResult confirmToCal = MessageBox.Show("计算电影md5值需要一定时间，是否确认进行计算？", "提示", MessageBoxButton.YesNo);
			if (confirmToCal == MessageBoxResult.Yes)
			{
				var filetmp = getSelectFileInfo();
				if (filetmp == null) return;
				string md5tmp = Fish.MovieManager.VideoControl.Class1.Instance.SetMd5(filetmp.id);
				if (md5tmp != null)
				{
					this.text_md5.Text = md5tmp;
				}
			}
		}

		//About us
		private void AboutUsFlyoutCheck(object sender, RoutedEventArgs e)
		{
			if (this.MyFlyoutCheck.IsChecked == false)
				this.MyFlyoutCheck.IsChecked = true;
			else
				this.MyFlyoutCheck.IsChecked = false;
		}
		private void hyperlink_github_Click(object sender, RoutedEventArgs e)
		{
			System.Diagnostics.Process.Start("https://github.com/maemual/MyModernMovieManager/");
		}

		//loading text
		public void LoadText()
		{
			string textFile = "LICENSE.txt";
			FileStream fs;
			if (File.Exists(textFile))
			{
				fs = new FileStream(textFile, FileMode.Open, FileAccess.Read);
				using (fs)
				{
					TextRange text = new TextRange(Agreement.Document.ContentStart, Agreement.Document.ContentEnd);
					text.Load(fs, DataFormats.Text);
				}
			}
		}

		//Score
		private void ScoreMovie(int score)
		{
			var tmp = getSelectFileInfo();
			Fish.MovieManager.VideoControl.Class1.Instance.SetUserStar(tmp.id, score);
			datagrid_Update_Fun();
			
		}
		private void MenuItem_Score1(object sender, RoutedEventArgs e)
		{
			ScoreMovie(1);
		}
		private void MenuItem_Score2(object sender, RoutedEventArgs e)
		{
			ScoreMovie(2);
		}
		private void MenuItem_Score3(object sender, RoutedEventArgs e)
		{
			ScoreMovie(3);
		}
		private void MenuItem_Score4(object sender, RoutedEventArgs e)
		{
			ScoreMovie(4);
		}
		private void MenuItem_Score5(object sender, RoutedEventArgs e)
		{
			ScoreMovie(5);
		}

		//lock
		private void LockFun()
		{
			this.LockFlyoutCheck.IsChecked = true;
		}
		private void UnLockFun()
		{
			this.LockFlyoutCheck.IsChecked = false;
		}
	}
}