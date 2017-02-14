using System;
using System.Collections.Generic;
using System.IO;

namespace Task1.ScoreSystem {
    internal sealed class ScoreSystem {

        private const int HighScoresLength = 10;
        private Func<string> _nameRequest;

        private const string RelativePath = @".\Data\highscores.txt";

        internal void SubscribeToNameRequest (Func<string> nameRequest) {
            if (nameRequest == null) {
                throw new ArgumentNullException("Cannot subscribe with "+"null function!");
            }
            _nameRequest = nameRequest;
        }


        public void UpdateHighScores (int newScore) {
            if (newScore < 0) {
                throw new ArgumentOutOfRangeException("Score cannot be "+"negative");
            }
            var highScores = ReadHighscores ();

            if (_nameRequest == null) {
                throw new InvalidOperationException ("Wrong View implementation. Username request is not processed.");
            }
            var newName = _nameRequest ();

            highScores.Add (new KeyValuePair<string, int> (newName, newScore));
            highScores.Sort ((x, y) => x.Value.CompareTo (y.Value));
            highScores.Reverse ();
            if (highScores.Count > HighScoresLength) {
                highScores.RemoveRange (HighScoresLength, highScores.Count - HighScoresLength);
            }

            var path = Path.Combine (Environment.CurrentDirectory, RelativePath);
            using (var streamWriter = new StreamWriter (path, false)) {
                foreach (var pair in highScores) {
                    streamWriter.WriteLine (pair.Key + " " + pair.Value);
                }
            }
        }

        internal static List <KeyValuePair<string, int>> ReadHighscores () {
            var highScores = new List<KeyValuePair<string, int>> ();
            var path = Path.Combine (Environment.CurrentDirectory, RelativePath);

            if (!File.Exists (path)) { return highScores; }

            try {
                using (var streamReader = new StreamReader (path)) {
                    for (var i = 0; i < HighScoresLength; i++) {
                        if (streamReader.EndOfStream) { break; }
                        var line = streamReader.ReadLine ();
                        if (line == null) {
                            throw new NullReferenceException ("Error while reading file");
                        }
                        var words = line.Split (' ');
                        var name = words[0];
                        var score = int.Parse (words[1]);
                        highScores.Add (new KeyValuePair<string, int> (name, score));
                    }
                }
            } catch (IndexOutOfRangeException ex) {
                Console.WriteLine (ex.StackTrace);
            }

            return highScores;
        }
    }
}
