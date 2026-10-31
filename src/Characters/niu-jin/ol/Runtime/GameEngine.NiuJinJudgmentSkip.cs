namespace CardGame.Core;

public sealed record NiuJinJudgmentSkipConsumedEvent(
    int OwnerSeat, string SkillInstanceId, int ActualTurnNumber, bool AlreadySkipped) : IGameEvent;
