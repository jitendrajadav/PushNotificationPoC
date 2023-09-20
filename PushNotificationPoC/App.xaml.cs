using Plugin.FirebasePushNotification;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PushNotificationPoC
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override async void OnStart()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationAlways>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.LocationAlways>();
            }


            // Check if the app has battery optimization exemption
            var status1 = await Permissions.CheckStatusAsync<Permissions.Battery>();

            // If not granted, request it
            if (status1 != PermissionStatus.Granted)
            {
                status1 = await Permissions.RequestAsync<Permissions.Battery>();
            }
            CrossFirebasePushNotification.Current.OnTokenRefresh += Current_OnTokenRefresh;

            //CrossFirebasePushNotification.Current.OnNotificationReceived += async (s, p) =>
            //{
            //    Device.BeginInvokeOnMainThread(async () =>
            //    {
            //        if (await LocalNotificationCenter.Current.AreNotificationsEnabled() == false)
            //        {
            //            await LocalNotificationCenter.Current.RequestNotificationPermission();
            //        }

            //        var notification = new NotificationRequest
            //        {
            //            NotificationId = 100,
            //            Title = "Test",
            //            Description = "I am implemented Push Notification provision with 2 buttons I have to work on image and some other condition will update you more on tomorrow please check attached screen shot, I am implemented Push Notification provision with 2 buttons I have to work on image and some other condition will update you more on tomorrow please check attached screen shot",
            //            ReturningData = "Dummy data", // Returning data when tapped on notification.
            //            CategoryType = Plugin.LocalNotification.NotificationCategoryType.Status,
            //            Android =
            //            {
            //                IconSmallName =
            //                {
            //                    ResourceName = "baby",
            //                },
            //                IconLargeName =
            //                {
            //                     ResourceName = "baby",
            //                },
            //            },
            //        };
            //        await LocalNotificationCenter.Current.Show(notification);
            //    });
            //};

            CrossFirebasePushNotification.Current.OnNotificationAction += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("Notification action tapped");


                if (!string.IsNullOrEmpty(p.Identifier))
                {
                    System.Diagnostics.Debug.WriteLine($"ActionId: {p.Identifier}");
                }

                foreach (var data in p.Data)
                {
                    System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
                }
            };


        }

        private void Current_OnTokenRefresh(object source, FirebasePushNotificationTokenEventArgs e)
        {
            if (e == null)
            {
                var token = e.Token;
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
