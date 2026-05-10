using FinalProject_Jenotan.UI;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace FinalProject_Jenotan
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Frame MainFrame { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            MainFrame = MainFrameControl;
            MainFrame.Navigate(new TitlePage());
        }
    }
}