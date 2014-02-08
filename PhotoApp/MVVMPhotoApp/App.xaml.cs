using System.Windows;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Threading;

namespace MVVMPhotoApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            DispatcherHelper.Initialize();
        }
        

        protected override void OnStartup(StartupEventArgs e)
        {
            Application.Current.DispatcherUnhandledException +=
              new DispatcherUnhandledExceptionEventHandler(DispatcherUnhandledExceptionHandler);

            base.OnStartup(e);
        }

        void DispatcherUnhandledExceptionHandler(object sender, DispatcherUnhandledExceptionEventArgs args)
        {
            args.Handled = true;

            MessageBox.Show(args.Exception.Message, "Uncaught Thread Exception",
                            MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
