namespace CardGame.Core;
// Issued facts outlive their source. They qualify every present or future grant,
// rather than changing local grant enablement or retaining a restore list.
public sealed record CurrentTurnNonLockedSkillSuppression(int TurnNumber, int TurnOwnerSeat, long ParentFrameId, int EffectIndex, CardUseEffectSource Source, int TargetSeat);
public sealed record CurrentTurnNonLockedSkillSuppressionIssuedEvent(CurrentTurnNonLockedSkillSuppression Suppression) : IGameEvent;
public sealed record CurrentTurnNonLockedSkillSuppressionsExpiredEvent(int TurnNumber, int TurnOwnerSeat) : IGameEvent;
