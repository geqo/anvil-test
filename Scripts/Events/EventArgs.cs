// EventArgs.cs
using System;

// ������� �����, ����� ������ �����, ����� ����� ������� �������
public class PawnEventArgs : EventArgs
{
    public Pawn SourcePawn { get; }
    public PawnEventArgs(Pawn sourcePawn) { SourcePawn = sourcePawn; }
}

public class PawnDamagedEventArgs : PawnEventArgs
{
    public float DamageAmount { get; set; } // ������ 'set' ��������� ��� �����

    // ��������� ����� �������� ���� (��������, ��-�� �������� ����)
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