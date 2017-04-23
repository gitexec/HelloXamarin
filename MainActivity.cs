using System;
using Android.App;
using Android.Widget;
using Android.Content;
using Android.OS;
using System.Collections.Generic;

namespace HelloXamarin
{
    [Activity(Label = "Phone Word", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        static readonly List<string> phoneNumbers = new List<string>();
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            //Code go here:
            EditText phoneNumberText = FindViewById<EditText>(Resource.Id.PhoneNumberText);
            Button translateButton = FindViewById<Button>(Resource.Id.TranslateButton);
            Button callButton = FindViewById<Button>(Resource.Id.CallButton);
            Button callHistoryButton = FindViewById<Button>(Resource.Id.CallHistoryButton);

            callButton.Enabled = false;

            string translatedNumber = string.Empty;
            translateButton.Click += (object sender, EventArgs e) =>
            {
                translatedNumber = Core.PhoneTranslator.ToNumber(phoneNumberText.ToString());
                Console.WriteLine("Clicked!! " + translatedNumber);

                if (String.IsNullOrWhiteSpace(translatedNumber))
                {
                    callButton.Text = "Call";
                    callButton.Enabled = false;
                }
                else
                {
                    callButton.Text = "Call" + translatedNumber;
                    callButton.Enabled = true;
                }
            };
            callButton.Click += (object sender, EventArgs e) =>
            {
                var callDialog = new AlertDialog.Builder(this);
                callDialog.SetMessage("Call" + translatedNumber + "?");
                callDialog.SetNeutralButton("Call", delegate
                {
                    phoneNumbers.Add(translatedNumber);
                    callHistoryButton.Enabled = true;
                    var callIntent = new Intent(Intent.ActionCall);
                    callIntent.SetData(Android.Net.Uri.Parse("tel:" + translatedNumber));
                    StartActivity(callIntent);
                });
                callDialog.SetNegativeButton("Cancel", delegate {});
                callDialog.Show();

            };
            callHistoryButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(CallHistoryActivity));
                intent.PutStringArrayListExtra("phone_numbers", phoneNumbers);
                StartActivity(intent);
            };
        }
    }
}