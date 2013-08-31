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
using FirstFloor.ModernUI.Windows.Controls;
namespace Fish.MovieManager.testUI.Content
{
	/// <summary>
	/// Interaction logic for About.xaml
	/// </summary>
	public partial class About : UserControl
	{
		public About()
		{
			InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			ModernDialog md = new ModernDialog(1234)
			{
				Title = "hhhh"
			};
			md.ShowDialog();
		}
	}
}
