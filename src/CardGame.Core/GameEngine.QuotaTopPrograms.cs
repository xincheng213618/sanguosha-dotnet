namespace CardGame.Core;
public sealed partial class GameEngine
{
    private ref readonly global::System.Collections.Generic.Dictionary<(int Turn, int Target, string Skill), long> _firstTurnTargetActions => ref _quotaTopPrograms.ReferenceToFirstTurnTargetActions;
}

public sealed record ProgramQuotaLossIssuedEvent(long FrameId, int OwnerSeat, string SkillId, string SkillInstanceId, int OtherSeat) : IGameEvent;
