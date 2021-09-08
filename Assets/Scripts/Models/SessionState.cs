internal class SessionState
{
    public HighScoresList hsList { get; set; }

    public SessionState Init()
    {   // 'manual' initialization when not created by Json.NET
        hsList = new HighScoresList();
        return this;
    }
}