#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCurrentTurnSkillSuppressionsEntries
{
    internal long CaptureCurrentTurnQualificationStamp() => _resolve().CaptureCurrentTurnQualificationStamp();
    internal void ExpireCurrentTurnNonLockedSkillSuppressions(int turnNumber, int turnOwnerSeat) => _resolve().ExpireCurrentTurnNonLockedSkillSuppressions(turnNumber, turnOwnerSeat);
    internal bool IsCurrentTurnSkillGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant) => _resolve().IsCurrentTurnSkillGrantQualified(owner, grant);
    internal void IssueCurrentTurnNonLockedSkillSuppression(global::CardGame.Core.ProgramSkillFrame frame, int targetSeat) => _resolve().IssueCurrentTurnNonLockedSkillSuppression(frame, targetSeat);
}
