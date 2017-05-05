using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Preferences;
using Android.Graphics;
using Android.Util;

namespace BusinessSpecial.Droid.Helpers
{
    public class AppPreferences
    {
        private ISharedPreferences nameSharedPrefs;
        private ISharedPreferencesEditor namePrefsEditor; //Declare Context,Prefrences name and Editor name  
        private Context mContext;
        private static String PREFERENCE_ACCESS_KEY = "PREFERENCE_ACCESS_KEY"; //Value Access Key Name  
        public static String NAME = "NAME"; //Value Variable Name  
        private static String PREFERENCE_ACCESS_List; //Value Access Key Name  
        public static List<String> NAMES; //Value Variable Name  
        public AppPreferences(Context context)
        {
            this.mContext = context;
            nameSharedPrefs = PreferenceManager.GetDefaultSharedPreferences(mContext);
            namePrefsEditor = nameSharedPrefs.Edit();
        }
        public void saveAccessKey(string key) // Save data Values  
        {
            namePrefsEditor.PutString(PREFERENCE_ACCESS_KEY, key);
            namePrefsEditor.Commit();
        }
        public void saveAccessList(List<string> key) // Save data Values  
        {
            namePrefsEditor.PutStringSet(PREFERENCE_ACCESS_List, key);
            namePrefsEditor.Commit();
        }
        public string getAccessKey() // Return Get the Value  
        {
            return nameSharedPrefs.GetString(PREFERENCE_ACCESS_KEY, "");
        }
        public List<string> getAccessList() // Return Get the Value  
        {
            return nameSharedPrefs.GetStringSet(PREFERENCE_ACCESS_List, null).ToList();
        }

        public Bitmap StringToBitMap(String encodedString)
        {
            try
            {
                byte[] encodeByte = Base64.Decode(encodedString, Base64.Default);
                Bitmap bitmap = BitmapFactory.DecodeByteArray(encodeByte, 0, encodeByte.Length);

                    int targetWidth = 60;
                    int targetHeight = 60;
                    Bitmap targetBitmap = Bitmap.CreateBitmap(targetWidth,
                                        targetHeight, Bitmap.Config.Argb8888);

                    Canvas canvas = new Canvas(targetBitmap);
                    Path path = new Path();
                    path.AddCircle(((float)targetWidth - 1) / 2,
                        ((float)targetHeight - 1) / 2,
                        (Math.Min(((float)targetWidth),
                        ((float)targetHeight)) / 2),
                        Path.Direction.Ccw);

                    canvas.ClipPath(path);
                    Bitmap sourceBitmap = bitmap;
                    canvas.DrawBitmap(sourceBitmap,
                        new Rect(0, 0, sourceBitmap.Width,
                        sourceBitmap.Height),
                        new Rect(0, 0, targetWidth, targetHeight), null);
                return targetBitmap;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}