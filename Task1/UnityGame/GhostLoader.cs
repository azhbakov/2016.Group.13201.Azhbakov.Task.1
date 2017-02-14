using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Task1.UnityGame {
    internal static class GhostLoader {

        private static readonly byte[] ExpectedKey = { 38, 127, 244, 147, 109, 17, 182, 227 };
        private static readonly string GhostsFolder = Environment.CurrentDirectory;

        internal static List<Type> GetGhostTypes () {
            var ghostTypes = new List<Type> ();
            var dlls = Directory.GetFiles (GhostsFolder, "*.dll");
            foreach (var d in dlls) {
                try {
                    var s = Assembly.LoadFrom (d);
                    if (!s.GetName ().GetPublicKeyToken ().SequenceEqual (ExpectedKey)) {
                        continue;
                    }
                    foreach (var t in s.GetExportedTypes ()) {
                        if (typeof(IComponent).IsAssignableFrom (t)) {
                            ghostTypes.Add(t);
                        }
                    }
                }
                catch (FileLoadException) {
                }
            }

            if (ghostTypes.Count == 0) {
                throw new FileNotFoundException ("No valid ghost dll detected!");
            }
            return ghostTypes;
        }

    }
}
