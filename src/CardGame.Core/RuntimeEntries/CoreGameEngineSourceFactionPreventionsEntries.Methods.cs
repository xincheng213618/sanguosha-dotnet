#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSourceFactionPreventionsEntries
{
    internal bool CanRunSourceFactionPrevention(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanRunSourceFactionPrevention(candidate, trigger, context);
    internal bool HasSourceFactionYieldDying(long beforeDamageId) => _resolve().HasSourceFactionYieldDying(beforeDamageId);
    internal bool IsSourceFactionYieldProgramDying() => _resolve().IsSourceFactionYieldProgramDying();
    internal bool IsValidSourceFactionPrevention(ProgramSkillFrame root, BeforeDamageProgramWindowFrame parent) => _resolve().IsValidSourceFactionPrevention(root, parent);
    internal SkillProgramStepOutcome PreventDamageAndConsumeSourceFaction(ProgramSkillFrame frame, string stateId) => _resolve().PreventDamageAndConsumeSourceFaction(frame, stateId);
    internal ProgramSkillFrame? SourceFactionYieldObserverRoot(long beforeDamageId) => _resolve().SourceFactionYieldObserverRoot(beforeDamageId);
}
