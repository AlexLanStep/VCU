using LabCarDeskTop.Views;
using Prism.Ioc;
using System.Windows;

namespace LabCarDeskTop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<LabCarDT>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}
