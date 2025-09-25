// EventArgs.cs
using System;

// Базовый класс, чтобы всегда знать, какая пешка вызвала событие
public class PawnEventArgs : EventArgs
{
    public Pawn SourcePawn { get; }
    public PawnEventArgs(Pawn sourcePawn) { SourcePawn = sourcePawn; }
}

public class PawnDamagedEventArgs : PawnEventArgs
{
    public float DamageAmount { get; set; } // Сделал 'set' публичным для модов

    // Позволяет модам отменить урон (например, из-за силового поля)
    public bool IsCancelled { get; set; } = false;

    public PawnDamagedEventArgs(Pawn sourcePawn, float damageAmount) : base(sourcePawn)
    {
        DamageAmount = damageAmount;
    }
}

public class PawnDeathEventArgs : PawnEventArgs
{
    public PawnDeathEventArgs(Pawn sourcePawn) : base(sourcePawn) { }
}

public class PawnStateChangedEventArgs : PawnEventArgs
{
    public PawnState OldState { get; }
    public PawnState NewState { get; }

    public PawnStateChangedEventArgs(Pawn sourcePawn, PawnState oldState, PawnState newState) : base(sourcePawn)
    {
        OldState = oldState;
        NewState = newState;
    }
}