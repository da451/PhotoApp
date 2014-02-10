using System.Collections.Generic;
using System.Linq;
using System.Windows;
using FluentNHibernate.Testing.Values;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
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

                Messenger.Default.Register<NotificationMessageAction<List<string>>>(this, (message) => SendImagesSourse(message));
            };
            Closing += (s, e) => ViewModelLocator.Cleanup();
        }

        private void SendImagesSourse(NotificationMessageAction<List<string>> messageAction)
        {
            if (messageAction.Notification == MessengerMessage.OPEN_FILE_DIALOG_FORM)
            {
                var ofd = new OpenFileDialog
                {
                    Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png",
                    Multiselect = true
                };

                if (ofd.ShowDialog() == true)
                {
                    messageAction.Execute(ofd.FileNames.ToList());
                }

            }
        }
    }
}