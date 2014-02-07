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
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using MVVMPhotoApp.Notifications;

namespace MVVMPhotoApp
{
    /// <summary>
    /// Interaction logic for AddPhotoWindow.xaml
    /// </summary>
    public partial class AddPhotoWindow : Window
    {
        public AddPhotoWindow()
        {
            InitializeComponent();

            Messenger.Default.Register<NotificationMessage<bool>>(this, message =>
            {
                if (message.Notification == MessengerMessage.CLOSE_ADD_PHOTO_FORM)
                {
                    DialogResult = message.Content;
                    this.Close();
                }
            });

            //Messenger.Default.Register<string>(this, message =>
            //{
            //    if (message == MessengerMessage.OPEN_FILE_DIALOG_FORM)
            //    {
            //        var ofd = new OpenFileDialog { Filter = "All files (*.*)|*.*", Multiselect = false };
            //        if (ofd.ShowDialog() == true)
            //        {
            //        }

            //    }
            //});

            Messenger.Default.Register<NotificationMessageAction<string>>(this, (message) => SendImageSourse(message));
        }

        private void SendImageSourse(NotificationMessageAction<string> messageAction)
        {
            if (messageAction.Notification == MessengerMessage.OPEN_FILE_DIALOG_FORM)
            {
                var ofd = new OpenFileDialog { Filter = "All files (*.*)|*.*", Multiselect = false };

                if (ofd.ShowDialog() == true)
                {
                    messageAction.Execute(ofd.FileName);
                }

            }
        }
    }
}
