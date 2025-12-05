using System;

namespace HighScores
{
    [Serializable]
    public class HighScores
    {
        public HighScore[] scores;
    }

    [Serializable]
    public class HighScore
    {
        public int id = 0;
        public string playerName = "";
        public float playerScore = 0;
        public string playTime = "";

    }
}