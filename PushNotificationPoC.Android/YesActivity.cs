using Android.App;
using Android.OS;

namespace PushNotificationPoC.Droid
{
    [Activity(Label = "Yes Activity")]
    public class YesActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Handle the "Yes" button action here, for example, displaying a confirmation message.
            // You can also perform any other desired operations in response to the "Yes" button click.

            //SetContentView(Resource.Layout.yes_activity_layout); // Replace with your layout if needed
        }
    }
}