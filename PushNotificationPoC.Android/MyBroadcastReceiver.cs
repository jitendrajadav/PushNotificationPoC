using Android.App;
using Android.Content;
using AndroidX.Core.App;

namespace PushNotificationPoC.Droid
{
    [BroadcastReceiver(Enabled = true)]
    [IntentFilter(new[] { "my_custom_action" })]
    public class MyBroadcastReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            string channelId = "my_channel_id";
            string channelName = "My Channel";

            var channel = new NotificationChannel(channelId, channelName, NotificationImportance.Default);
            var notificationManager = context.GetSystemService(Context.NotificationService) as NotificationManager;
            notificationManager.CreateNotificationChannel(channel);

            // Define and add notification actions
            var actionIntent = new Intent(context, typeof(MyBroadcastReceiver));
            actionIntent.PutExtra("action", "my_custom_action");
            var pendingActionIntent = PendingIntent.GetBroadcast(context, 0, actionIntent, PendingIntentFlags.UpdateCurrent);

            var builder = new NotificationCompat.Builder(context, channelId)
                .SetSmallIcon(Resource.Drawable.baby)
                .SetContentTitle("Notification Title")
                .SetContentText("Notification Text")
                .AddAction(Resource.Drawable.heart, "My Action", pendingActionIntent);

            // Build and display the notification
            var notification = builder.Build();
            notificationManager.Notify(0, notification);
        }
    }

}