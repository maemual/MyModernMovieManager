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

namespace Fish.MovieManager.UI.dialog
{
	/// <summary>
	/// about_Dialog.xaml 的交互逻辑
	/// </summary>
	public partial class about_Dialog : MetroWindow
	{
		public about_Dialog()
		{
			InitializeComponent();
			ThemeManager.ChangeTheme(this, MainWindow.currentAccent, MainWindow.currentTheme);
		}
	}
}
