using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using AndroidX.Core.App;
using Plugin.CurrentActivity;
using Plugin.FirebasePushNotification;
using System.Collections.Generic;

namespace PushNotificationPoC.Droid
{
    [Activity(Label = "PushNotificationPoC", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        internal static readonly string channelId = "notificatioinpoc";
        internal static readonly int notificationId = 100;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            // Define a notification channel (Add the code here)
            string channelId = "my_channel_id";
            string channelName = "My Channel";

            var channel = new NotificationChannel(channelId, channelName, NotificationImportance.Default);
            var notificationManager = GetSystemService(NotificationService) as NotificationManager;
            notificationManager.CreateNotificationChannel(channel);

            //Set the default notification channel for your app when running Android Oreo
            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                //Change for your default notification channel id here
                FirebasePushNotificationManager.DefaultNotificationChannelId = "FirebasePushNotificationChannel";

                //Change for your default notification channel name here
                FirebasePushNotificationManager.DefaultNotificationChannelName = "General";
            }

            //If debug you should reset the token each time.
#if DEBUG
            FirebasePushNotificationManager.Initialize(this, true);
#else
              FirebasePushNotificationManager.Initialize(this,false);
#endif


            LoadApplication(new App());

            FirebasePushNotificationManager.ProcessIntent(this, Intent);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            //LocalNotificationCenter.NotifyNotificationTapped(intent);
            FirebasePushNotificationManager.ProcessIntent(this, intent);
        }

        void SendNotification(string messageBody, IDictionary<string, string> data)
        {
            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            if (data != null)
                foreach (var key in data.Keys)
                {
                    intent.PutExtra(key, data[key]);
                }

            //var pendingIntent = PendingIntent.GetActivity(this, MainActivity.notificationId, intent, PendingIntentFlags.Immutable);

            //var notificationBuilder = new NotificationCompat.Builder(this, MainActivity.channelId)
            //                          .SetSmallIcon(Resource.Drawable.common_google_signin_btn_icon_dark_focused)
            //                          .SetContentTitle("Hello")
            //                          .SetContentText(messageBody)
            //                          .SetAutoCancel(true)
            //                          .SetContentIntent(pendingIntent);

            //var notificationManager = NotificationManagerCompat.From(this);
            //notificationManager.Notify(MainActivity.notificationId, notificationBuilder.Build());

            var yesIntent = new Intent(this, typeof(YesActivity)); // Replace with the appropriate activity for "Yes" action
            var yesPendingIntent = PendingIntent.GetActivity(this, 0, yesIntent, 0);

            var noIntent = new Intent(this, typeof(NoActivity)); // Replace with the appropriate activity for "No" action
            var noPendingIntent = PendingIntent.GetActivity(this, 0, noIntent, 0);

            var notificationBuilder = new NotificationCompat.Builder(this, MainActivity.channelId)
                .SetSmallIcon(Resource.Drawable.common_google_signin_btn_icon_dark_focused)
                .SetContentTitle("Hello")
                .SetContentText(messageBody)
                .SetAutoCancel(true)
                .SetContentIntent(yesPendingIntent)
                .SetContentIntent(noPendingIntent)
                .AddAction(Resource.Drawable.chat, "Yes", yesPendingIntent) // Replace "yes_icon" with the resource ID of your "Yes" button icon
                .AddAction(Resource.Drawable.heart, "No", noPendingIntent);    // Replace "no_icon" with the resource ID of your "No" button icon

            var notificationManager = NotificationManagerCompat.From(this);
            notificationManager.Notify(MainActivity.notificationId, notificationBuilder.Build());

        }

        private bool isApplicationInTheBackground()
        {
            bool isInBackground;

            ActivityManager.RunningAppProcessInfo myProcess = new ActivityManager.RunningAppProcessInfo();
            ActivityManager.GetMyMemoryState(myProcess);
            isInBackground = myProcess.Importance != Android.App.Importance.Foreground;

            return isInBackground;
        }

    }
}