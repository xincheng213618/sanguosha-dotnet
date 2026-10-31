#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramTargetCommitEntries
{
    internal SkillProgramStepOutcome BeginRelativeZoneDemand(SkillProgramEffect effect, ProgramSkillFrame frame) => _resolve().BeginRelativeZoneDemand(effect, frame);
    internal SkillProgramStepOutcome DrawFirstProgramTargetReward(SkillProgramEffect effect, ProgramSkillFrame frame) => _resolve().DrawFirstProgramTargetReward(effect, frame);
    internal bool IsProgramTargetContextValid(ProgramSkillWindowContext context) => _resolve().IsProgramTargetContextValid(context);
    internal bool ProgramTargetSourceValid(ProgramSkillFrame frame) => _resolve().ProgramTargetSourceValid(frame);
    internal void PublishRelativeZoneTarget(ProgramSkillFrame frame) => _resolve().PublishRelativeZoneTarget(frame);
    internal IReadOnlyList<PromptChoice> RelativeZoneChoices(ProgramSkillFrame frame) => _resolve().RelativeZoneChoices(frame);
    internal void ResolveRelativeZoneTarget(PromptChoice selected) => _resolve().ResolveRelativeZoneTarget(selected);
    internal bool TryResumeProgramTargetCommit(long id) => _resolve().TryResumeProgramTargetCommit(id);
}
