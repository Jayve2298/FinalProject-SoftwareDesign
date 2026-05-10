using FinalProject_Jenotan.Database;
using FinalProject_Jenotan.DatabaseSeed;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Data;
using System.Windows;

namespace FinalProject_Jenotan
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            using var context = new TinyCollegeDbContextSQLServer();

            context.Database.Migrate();

            SchoolSeeder.Seed(context);

            var mainWindow = new MainWindow();
            mainWindow.Show();

            this.Startup += (s, ev) =>
            {
                this.MainWindow = new MainWindow();
                this.MainWindow.Show();
            };
        }

        private void SetOwner(object sender, RoutedEventArgs e)
        {
            if (sender is Window window && window != Application.Current.MainWindow)
            {
                window.Owner = Application.Current.MainWindow;
            }
        }

    }
}
