using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using MVVMPhotoApp.Notifications;
using MVVMPhotoApp.ViewModel;

namespace MVVMPhotoApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                Messenger.Default.Register<NotificationMessageAction<bool>>(this, message =>
                {
                    if (message.Notification == MessengerMessage.OPEN_ADD_PHOTO_FORM)
                    {
                        AddPhotoWindow apw = new AddPhotoWindow();
                        message.Execute(apw.ShowDialog()??false);
                    }
                });
            };
            Closing += (s, e) => ViewModelLocator.Cleanup();
            
        }
    }
}