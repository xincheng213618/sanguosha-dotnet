#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSourceFactionPreventionsEntries
{
    internal bool CanRunSourceFactionPrevention(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanRunSourceFactionPrevention(candidate, trigger, context);
    internal bool HasSourceFactionYieldDying(long beforeDamageId) => _resolve().HasSourceFactionYieldDying(beforeDamageId);
    internal bool HasSourceFactionYieldObserver(long beforeDamageId) => _resolve().HasSourceFactionYieldObserver(beforeDamageId);
    internal bool IsSourceFactionYieldProgramDying() => _resolve().IsSourceFactionYieldProgramDying();
    internal bool IsValidSourceFactionPrevention(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.BeforeDamageProgramWindowFrame parent) => _resolve().IsValidSourceFactionPrevention(root, parent);
    internal global::CardGame.Core.SkillProgramStepOutcome PreventDamageAndConsumeSourceFaction(global::CardGame.Core.ProgramSkillFrame frame, string stateId) => _resolve().PreventDamageAndConsumeSourceFaction(frame, stateId);
    internal global::CardGame.Core.ProgramSkillFrame? SourceFactionYieldObserverRoot(long beforeDamageId) => _resolve().SourceFactionYieldObserverRoot(beforeDamageId);
}
