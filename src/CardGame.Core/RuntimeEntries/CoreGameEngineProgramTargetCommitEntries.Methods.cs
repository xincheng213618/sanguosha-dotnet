#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramTargetCommitEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginRelativeZoneDemand(global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().BeginRelativeZoneDemand(effect, frame);
    internal global::CardGame.Core.SkillProgramStepOutcome DrawFirstProgramTargetReward(global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().DrawFirstProgramTargetReward(effect, frame);
    internal bool IsProgramTargetContextValid(global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsProgramTargetContextValid(context);
    internal bool ProgramTargetSourceValid(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ProgramTargetSourceValid(frame);
    internal void PublishRelativeZoneTarget(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishRelativeZoneTarget(frame);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> RelativeZoneChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().RelativeZoneChoices(frame);
    internal void ResolveRelativeZoneTarget(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveRelativeZoneTarget(selected);
    internal bool TryResumeProgramTargetCommit(long id) => _resolve().TryResumeProgramTargetCommit(id);
}
