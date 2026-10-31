namespace CardGame.Core;
// Issued facts outlive their source. They qualify every present or future grant,
// rather than changing local grant enablement or retaining a restore list.
public sealed record CurrentTurnNonLockedSkillSuppression(int TurnNumber, int TurnOwnerSeat, long ParentFrameId, int EffectIndex, CardUseEffectSource Source, int TargetSeat);
public sealed record CurrentTurnNonLockedSkillSuppressionIssuedEvent(CurrentTurnNonLockedSkillSuppression Suppression) : IGameEvent;
public sealed record CurrentTurnNonLockedSkillSuppressionsExpiredEvent(int TurnNumber, int TurnOwnerSeat) : IGameEvent;
public sealed partial class GameEngine
{
    private ref readonly global::System.Collections.Generic.List<global::CardGame.Core.CurrentTurnNonLockedSkillSuppression> _currentTurnSkillSuppressions => ref _currentTurnSkillSuppressions2.ReferenceToCurrentTurnSkillSuppressions;
    private ref long _currentTurnSkillSuppressionRevision => ref _currentTurnSkillSuppressions2.ReferenceToCurrentTurnSkillSuppressionRevision;
    private ref long _currentTurnQualificationStamp => ref _currentTurnSkillSuppressions2.ReferenceToCurrentTurnQualificationStamp;
    private ref (long Projection, long Suppression) _currentTurnQualificationDependencies => ref _currentTurnSkillSuppressions2.ReferenceToCurrentTurnQualificationDependencies;
}
