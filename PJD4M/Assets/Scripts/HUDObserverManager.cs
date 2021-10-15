using System;

public static class HUDObserverManager
{
    public static event Action<int> ONLivesChanged;

    public static void LivesChanged(int live)
    {
        ONLivesChanged?.Invoke(live);
    }

    public static event Action<float> ONPlayerEnergyChanged;

    public static void PlayerEnergyChanged(float energy)
    {
        ONPlayerEnergyChanged?.Invoke(energy);
    }

    public static event Action<bool> ONPlayerDied;

    public static void PlayerDied(bool state)
    {
        ONPlayerDied?.Invoke(state);
    }

    public static event Action<bool> ONPlayerVictory;

    public static void PlayerVictory(bool state)
    {
        ONPlayerVictory?.Invoke(state);
    }
}
