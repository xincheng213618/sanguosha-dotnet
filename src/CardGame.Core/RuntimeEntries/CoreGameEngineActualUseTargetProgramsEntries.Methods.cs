#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActualUseTargetProgramsEntries
{
    internal void AdvanceActualUseTargetCandidate(ActualUseTargetWindowFrame parent, bool activated, bool completed) => _resolve().AdvanceActualUseTargetCandidate(parent, activated, completed);
    internal bool CanRunActualUseTarget(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().CanRunActualUseTarget(candidate, context);
    internal void CompleteActualUseTargetBinding(ProgramSkillFrame child, bool completed) => _resolve().CompleteActualUseTargetBinding(child, completed);
    internal void ContinueActualUseTargetWindowCore() => _resolve().ContinueActualUseTargetWindowCore();
    internal void ContinueAfterActualUseTargets(ActualUseTargetWindowFrame parent) => _resolve().ContinueAfterActualUseTargets(parent);
    internal ActualUseTargetIdentity? FreezeActualUseTarget(CardUseFrame use, int target) => _resolve().FreezeActualUseTarget(use, target);
    internal bool MatchesActualUseTarget(ActualUseTargetIdentity identity) => _resolve().MatchesActualUseTarget(identity);
    internal bool StartActualUseTargetWindow(CardUseFrame use, IReadOnlyList<int> targets, ActualUseTargetReturnKind kind, ActualUseTargetTrickReturn? trick) => _resolve().StartActualUseTargetWindow(use, targets, kind, trick);
    internal bool TryBeginActualUseTargetPrograms(NativeOwners.CardAttackHandle attack, ActualUseTargetReturnKind kind) => _resolve().TryBeginActualUseTargetPrograms(attack, kind);
    internal bool TryBeginActualUseTargetPrograms2(NativeOwners.JizhiResolution pending) => _resolve().TryBeginActualUseTargetPrograms(pending);
    internal bool TracksPaidOwnTargets { get => _resolve().TracksPaidOwnTargets; }
}
