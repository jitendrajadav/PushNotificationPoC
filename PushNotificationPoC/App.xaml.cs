using Plugin.FirebasePushNotification;
using Plugin.LocalNotification;
using System.Collections.Generic;
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

            LocalNotificationCenter.Current.RegisterCategoryList(new HashSet<NotificationCategory>(new List<NotificationCategory>()
            {
                new NotificationCategory(Plugin.LocalNotification.NotificationCategoryType.Status)
                {
                    ActionList = new HashSet<NotificationAction>( new List<NotificationAction>()
                    {
                        new NotificationAction(100)
                        {
                            Title = "Confirm",
                            Android =
                            {
                                LaunchAppWhenTapped = true,
                                IconName =
                                {
                                    ResourceName = "icon.png"
                                }
                            },

                        },
                        new NotificationAction(101)
                        {
                            Title = "Close",
                            Android =
                            {
                                LaunchAppWhenTapped = false,
                                IconName =
                                {
                                    ResourceName = "icon.png"
                                }
                            }
                        }
                    })
                }
            }));

            // Local Notification tap event listener
            LocalNotificationCenter.Current.NotificationActionTapped += Current_NotificationActionTapped;

        }

        private void Current_NotificationActionTapped(Plugin.LocalNotification.EventArgs.NotificationActionEventArgs e)
        {
            //TODO : Needs to handle Tap event for local notification
            if (!string.IsNullOrEmpty(e.Request.ReturningData))
            {
                // your code goes here
                return;
            }

            //if (e.IsTapped)
            //{
            //    // your code goes here
            //    return;
            //}
            // if Notification Action are setup
            switch (e.Request.NotificationId)
            {
                // your code goes here
            }
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


            var Token = await CrossFirebasePushNotification.Current.GetTokenAsync();

            System.Diagnostics.Debug.WriteLine($"TOKEN : {Token}");

            CrossFirebasePushNotification.Current.OnNotificationReceived += async (s, p) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (await LocalNotificationCenter.Current.AreNotificationsEnabled() == false)
                    {
                        await LocalNotificationCenter.Current.RequestNotificationPermission();
                    }

                    var notification = new NotificationRequest
                    {
                        NotificationId = 100,
                        Title = "Test",
                        Description = "I am implemented Push Notification provision with 2 buttons I have to work on image and some other condition will update you more on tomorrow please check attached screen shot, I am implemented Push Notification provision with 2 buttons I have to work on image and some other condition will update you more on tomorrow please check attached screen shot",
                        ReturningData = "Dummy data", // Returning data when tapped on notification.
                        CategoryType = Plugin.LocalNotification.NotificationCategoryType.Status,
                        Android =
                        {
                            IconSmallName =
                            {
                                ResourceName = "baby",
                            },
                            IconLargeName =
                            {
                                 ResourceName = "baby",
                            },
                        },
                    };
                    await LocalNotificationCenter.Current.Show(notification);
                });
            };

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

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
