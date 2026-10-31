#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCurrentTurnSkillSuppressionsEntries
{
    internal long CaptureCurrentTurnQualificationStamp() => _resolve().CaptureCurrentTurnQualificationStamp();
    internal bool IsCurrentTurnSkillGrantQualified(CharacterState owner, SkillGrant grant) => _resolve().IsCurrentTurnSkillGrantQualified(owner, grant);
}
