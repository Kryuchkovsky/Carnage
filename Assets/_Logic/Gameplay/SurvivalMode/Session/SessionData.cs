using System;

namespace _Logic.Gameplay.SurvivalMode.Session
{
    [Serializable]
    public class SessionData
    {
        public Difficulty Difficulty;
        public int WaveCount;
        public float GameTime;
        public float TimeBeforeWaweSpawn;
        
    }
}