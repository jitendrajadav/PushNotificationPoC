using Plugin.FirebasePushNotification;
using System;
using Xamarin.Forms;

namespace PushNotificationPoC
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnClickSendNotificationHandler(object sender, EventArgs e)
        {
            var Token = await CrossFirebasePushNotification.Current.GetTokenAsync();
            token.Text = Token.ToString();
        }
    }
}
