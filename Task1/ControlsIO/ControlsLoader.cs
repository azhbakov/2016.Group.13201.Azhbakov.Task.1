using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;


namespace Task1.ControlsIO {
    internal static class ControlsLoader {
        private const string RelativePath = @".\Data\controls.txt";
        private const int ControlsArrayMaxSize = 10;


        internal static List<KeyValuePair<string, Key>> ReadControls () {
            var controls = new List<KeyValuePair<string, Key>> ();
            var path = Path.Combine (Environment.CurrentDirectory, RelativePath);

            if (!File.Exists (path)) {
                throw new FileNotFoundException("controls.txt file not found!");
            }

            try {
                using (var sr = new StreamReader (path)) {
                    for (var i = 0; i < ControlsArrayMaxSize; i++) {
                        var l = sr.ReadLine ();
                        if (l == null) {
                            throw new NullReferenceException ("File IO error");
                        }
                        var w = l.Split (' ');
                        var action = w[0];
                        var key = StringToKey (w[1]);
                        if (key == null) { throw new ArgumentException(w[1] + " is not key!");}
                        controls.Add (new KeyValuePair<string, Key> (action, key.Value));
                        if (sr.EndOfStream) { break;}
                    }
                }
            } catch (IndexOutOfRangeException ex) {
                Console.WriteLine (ex.StackTrace);
            }

            return controls;
        }

        internal static void WriteControls (List<KeyValuePair<string, string>> controls) {
            if (controls == null) {
                throw new ArgumentNullException("Controls cannot be "+"null");
            }
            try {
                var path = Path.Combine (Environment.CurrentDirectory, RelativePath);
                using (var streamWriter = new StreamWriter (path, false)) {
                    foreach (var pair in controls) {
                        streamWriter.WriteLine (pair.Key + " " + pair.Value);
                    }
                }
            } catch (IndexOutOfRangeException ex) {
                Console.WriteLine (ex.StackTrace);
            }
        }

        private static Key? StringToKey (string s) {
            foreach (Key k in Enum.GetValues(typeof(Key))) {
                if (k.ToString ().Equals (s)) {
                    return k;
                }
            }
            return null;
        }
    }
}
