#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineHpConditionalSkillGrantsEntries
{
    internal bool CanContinueHpConditionalProgram(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CanContinueHpConditionalProgram(frame);
    internal global::CardGame.Core.ProgramSkillFrame CaptureHpConditionalProgramSource(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CaptureHpConditionalProgramSource(frame);
    internal bool HpConditionalSourceQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant source) => _resolve().HpConditionalSourceQualified(owner, source);
    internal bool IsHpConditionalGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant) => _resolve().IsHpConditionalGrantQualified(owner, grant);
    internal void SynchronizeHpConditionalSkillGrants() => _resolve().SynchronizeHpConditionalSkillGrants();
    internal global::System.Collections.Generic.IReadOnlySet<string> HpConditionalCapabilitySkillIds { get => _resolve().HpConditionalCapabilitySkillIds; }
}
