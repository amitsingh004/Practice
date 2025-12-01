using System;

public static class GameEvents
{
    public static event Action OnGameRestart;

    public static void TriggerGameRestart()
    {
        OnGameRestart?.Invoke();
    }
}