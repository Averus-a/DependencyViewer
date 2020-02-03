namespace Orc.DependencyViewer
{
    using Catel.Logging;
    using Catel.Reflection;
    using Orchestra;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
#if DEBUG
            LogManager.AddDebugListener(false);
#endif
            InitializeFonts();
            StyleHelper.CreateStyleForwardersForDefaultStyles();
            this.ApplyTheme();
            base.OnStartup(e);
        }

        private void InitializeFonts()
        {
            Orc.Controls.FontImage.RegisterFont("FontAwesome", new FontFamily(new Uri("pack://application:,,,/Orc.DependencyViewer;component/Resources/Fonts/", UriKind.RelativeOrAbsolute), "./#FontAwesome"));
            Orc.Controls.FontImage.DefaultFontFamily = "FontAwesome";
            Orc.Controls.FontImage.DefaultBrush = new SolidColorBrush(Color.FromArgb(255, 87, 87, 87));
        }
    }
}
