namespace _Logic.Gameplay.SurvivalMode.Session
{
    public class SessionService
    {
        private SessionData _sessionData;
        
        public SessionService()
        {
            _sessionData = new SessionData();
        }

        public SessionData GetData()
        {
            return _sessionData;
        }
    }
}