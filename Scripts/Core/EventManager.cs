// EventManager.cs
using System;

public static class EventManager
{
    // --- Объявление всех доступных для модов событий ---
    
    // Событие урона (отменяемое)
    public static event Action<PawnDamagedEventArgs> OnPawnTakingDamage;

    // Событие смерти
    public static event Action<PawnDeathEventArgs> OnPawnDied;
    
    // Событие смены состояния (сон, еда и т.д.)
    public static event Action<PawnStateChangedEventArgs> OnPawnStateChanged;

    // --- Методы для вызова событий из игрового кода ---

    public static void FirePawnTakingDamage(PawnDamagedEventArgs args)
    {
        OnPawnTakingDamage?.Invoke(args);
    }

    public static void FirePawnDied(PawnDeathEventArgs args)
    {
        OnPawnDied?.Invoke(args);
    }
    
    public static void FirePawnStateChanged(PawnStateChangedEventArgs args)
    {
        OnPawnStateChanged?.Invoke(args);
    }
}