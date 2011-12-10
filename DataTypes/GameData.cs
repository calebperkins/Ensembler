
namespace DataTypes
{
    public enum Screens
    {
        Title,
        SelectLevel,
        WorldMap,
        Pause,
        PlayLevel,
        FinishLevel,
        Initial,
        Dialog
    }

    public struct LevelSummary
    {
        public string Title;
        public byte Difficulty;
        public string AssetName;
        public int HighScore;
        public int DeveloperHighScore;
        public int HighCombo;
        public int DeveloperHighCombo;
    }

    public class GameData
    {
        public int Height;
        public int Width;
        public Screens Screen;
        public LevelSummary[] Levels;
    }
}
