using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using System;
using System.IO;
using Android.Util;
using Android.Media;
using Android.Content;
using Android.Provider;
using static Android.Provider.MediaStore.Audio;
//using Java.IO;

namespace Ringtones_1
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
		
		private string TAG = "   =============== "; // tag for Log msgs
		Ringtone r;
		
		ContentResolver cr;

		protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

			string ringPath = Android.OS.Environment.DirectoryRingtones;
			Log.Info(TAG, ringPath);

			cr = this.ContentResolver;

			Button btn1 = FindViewById<Button>(Resource.Id.button1);
			Button btn2 = FindViewById<Button>(Resource.Id.button2);
			Button btn3 = FindViewById<Button>(Resource.Id.button3);

			btn1.Click += Btn1_Click;
			btn2.Click += Btn2_Click;
			btn3.Click += Btn3_Click;
		}

		private void Btn1_Click(object sender, EventArgs e)
		{
			Log.Info(TAG, "btn1 was clicked!");

			setAsRingtone("bepohello");
		}

		private void Btn2_Click(object sender, EventArgs e)
		{
			Log.Info(TAG, "btn2 was clicked!");

			setAsRingtone("beporooster");

		}



		/* JAVA:
        if(ringtone != null && ringtone.isPlaying())
            ringtone.stop();

        Uri uri = RingtoneManager.getDefaultUri(RingtoneManager.TYPE_RINGTONE);
        //Uri uri = RingtoneManager.getActualDefaultRingtoneUri(this, RingtoneManager.TYPE_RINGTONE);
        //Uri uri = RingtoneManager.getActualDefaultRingtoneUri(getApplicationContext(), RingtoneManager.TYPE_RINGTONE);
        ringtone = RingtoneManager.getRingtone(getApplicationContext(), uri);

        if(ringtone != null)
            ringtone.play();
		 */


		private void setAsRingtone(string audioName)
		{
			string folderName = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/" + Android.OS.Environment.DirectoryRingtones + "/";
			string audioFile = audioName + ".mp3";

			Log.Info(TAG, "Ringtones folder: " + folderName);
			Log.Info(TAG, "audio file: " + audioFile);


			var soundFilePath = Path.Combine(folderName, audioFile);
			Log.Info(TAG, "full path: " + soundFilePath);

			using (FileStream writeStream = File.OpenWrite(soundFilePath)) {
				//var readStream = context.Resources.OpenRawResource(soundId[pos]);
				//var readStream = Resources.OpenRawResource(audioName);

				var readStream = Assets.Open(audioFile);

				BinaryWriter writer = new BinaryWriter(writeStream);

				// create a buffer to hold the bytes 
				byte[] buffer = new Byte[1024];
				int bytesRead;

				// while the read method returns bytes keep writing them to the output stream
				while ((bytesRead = readStream.Read(buffer, 0, 1024)) > 0) {
					writeStream.Write(buffer, 0, bytesRead);
				}
			}

			//We now create a new content values object to store all the information about the ringtone.
			ContentValues values = new ContentValues();
			//values.Put(MediaStore.MediaColumns.Data, fi2.DirectoryName);
			values.Put(MediaStore.MediaColumns.Data, soundFilePath);

			if (audioName == "bepohello")
				values.Put(MediaStore.MediaColumns.Title, "Bepo Hello");
			else if (audioName == "beporooster")
				values.Put(MediaStore.MediaColumns.Title, "Bepo rooster");

			//values.Put(MediaStore.MediaColumns.Title, "Lelux");
			//values.Put(MediaStore.MediaColumns.Size, fi2.Length);
			values.Put(MediaStore.MediaColumns.MimeType, "audio/mp3");
			values.Put(AudioColumns.Artist, "MARCELO");
			values.Put(AudioColumns.IsRingtone, true);
			values.Put(AudioColumns.IsNotification, false);
			values.Put(AudioColumns.IsAlarm, false);
			values.Put(AudioColumns.IsMusic, false);

			// We put in the DDBB of MediaStore
			//Uri uri = MediaStore.Audio.Media.getContentUriForPath(f.getAbsolutePath());
			//Uri newUri = getBaseContext().getContentResolver().insert(uri, values);
			// Set as default
			//RingtoneManager.setActualDefaultRingtoneUri(getBaseContext(), RingtoneManager.TYPE_RINGTONE, newUri);

			//Android.Net.Uri uri = Media.GetContentUriForPath(soundFilePath);
			//Android.Net.Uri newUri = ContentResolver.Insert(uri, values);

			var uri = Media.GetContentUriForPath(soundFilePath);
			//var uri = MediaStore.Audio.Media.GetContentUriForPath(soundFilePath);

			//var cr = this.ContentResolver;
			var newUri = cr.Insert(uri, values);

			//this.ContentResolver.Insert(Android.Provider.MediaStore.Audio.Media.ExternalContentUri, values);

			//Android.Net.Uri newUri = this.ContentResolver.Insert(uri, values);

			//Android.Net.Uri newUri = BaseContext.ContentResolver.Insert(uri, values);
			RingtoneManager.SetActualDefaultRingtoneUri(this, RingtoneType.Ringtone, newUri);


			Log.Info(TAG, "uri: " + uri);
			Log.Info(TAG, "newUri: " + newUri);
		}



		private void Btn3_Click(object sender, EventArgs e)
		{
			Log.Info(TAG, "btn3 was clicked!");

			//using (r = RingtoneManager.GetRingtone(this, RingtoneManager.GetDefaultUri(RingtoneType.Ringtone))) 
			//{ r.Play(); }

			if (r != null && r.IsPlaying)
				r.Stop();

			//r = RingtoneManager.GetRingtone(this, RingtoneManager.GetDefaultUri(RingtoneType.Ringtone));
			Android.Net.Uri uri = RingtoneManager.GetDefaultUri(RingtoneType.Ringtone);
			r = RingtoneManager.GetRingtone(this, uri);
			r.Play();

			Console.WriteLine("+++++ default ring attributes ==================== " + r.AudioAttributes);
			Console.WriteLine("+++++ default ring URI ==================== " + uri.Path);
		}


	}
}

