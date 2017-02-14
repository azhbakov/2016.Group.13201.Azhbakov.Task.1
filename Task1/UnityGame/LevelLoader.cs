using System;
using System.Collections.Generic;
using System.IO;

namespace Task1.UnityGame {
    internal sealed class LevelLoader {
        private readonly string[] _levels;
        private int _nextLevel;
        internal bool AllRead => _nextLevel == _levels.Length;

        internal LevelLoader () {
            var levelsFolder = Path.Combine (Environment.CurrentDirectory, @".\Data\Levels");
            _levels = Directory.GetFiles (levelsFolder, "*.txt");
        }

        internal void Reset () {
            _nextLevel = 0;
        }

        internal LevelData ReadNextLevel () {
            var ld = ReadLevelFromTxt (_levels[_nextLevel]);
            _nextLevel++;
            return ld;
        }

        private static LevelData ReadLevelFromTxt (string path) {
            if (path == null) {
                throw new ArgumentNullException ("Path cannot"+" be null");
            }
            var lines = File.ReadAllLines (path);

            var levelData = new LevelData ();

            var width = lines[0].Length;
            levelData.FieldWidth = width;
            var height = lines.Length;
            levelData.FieldHeight = height;

            for (var y = 0; y < height; y++) {
                for (var x = 0; x < width; x++) {
                    EntityTypes? t = null;
                    switch (lines[y][x]) {
                        case ' ':
                            break;

                        case '.':
                            t = EntityTypes.Pill;
                            break;

                        case '*':
                            t = EntityTypes.SuperPill;
                            break;

                        case '#':
                            t = EntityTypes.Wall;
                            break;

                        case '%':
                            t = EntityTypes.Fruit;
                            break;

                        case '\\':
                            t = EntityTypes.Player;
                            break;

                        case '=':
                            t = EntityTypes.Ghost;
                            break;

                        default:
                            throw new NotSupportedException ("Unknown symbol " + lines[y][x] + " in " + path);
                    }
                    if (t == null) { continue;}
                    var type = (EntityTypes) t;
                    levelData.Entities.Add (new Entity (type, x, y));
                }
            }
            return levelData;
        }
    }

    internal sealed class LevelData {
        internal int FieldWidth { get; set; }
        internal int FieldHeight { get; set; }

        internal List<Entity> Entities { get; }

        internal LevelData () {
            Entities = new List<Entity> ();
        }
    }

        internal sealed class Entity {
            internal EntityTypes Type { get; }
            internal int GridX { get; }
            internal int GridY { get; }

            internal Entity (EntityTypes type, int gridX, int gridY) {
                Type = type;
                GridX = gridX;
                GridY = gridY;
            }
        }
        
    internal enum EntityTypes { Wall, Pill, SuperPill, Fruit, Player, Ghost}
}