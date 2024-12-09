using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Windows;
using VLM_OCR_Demo.Views;

namespace VLM_OCR_Demo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<VLMChat>();
        }

    }
}
