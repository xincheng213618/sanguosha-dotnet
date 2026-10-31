#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineHpConditionalSkillGrantsEntries
{
    internal bool CanContinueHpConditionalProgram(ProgramSkillFrame frame) => _resolve().CanContinueHpConditionalProgram(frame);
    internal ProgramSkillFrame CaptureHpConditionalProgramSource(ProgramSkillFrame frame) => _resolve().CaptureHpConditionalProgramSource(frame);
    internal bool HpConditionalSourceQualified(CharacterState owner, SkillGrant source) => _resolve().HpConditionalSourceQualified(owner, source);
    internal bool IsHpConditionalGrantQualified(CharacterState owner, SkillGrant grant) => _resolve().IsHpConditionalGrantQualified(owner, grant);
    internal void SynchronizeHpConditionalSkillGrants() => _resolve().SynchronizeHpConditionalSkillGrants();
    internal IReadOnlySet<string> HpConditionalCapabilitySkillIds { get => _resolve().HpConditionalCapabilitySkillIds; }
}
