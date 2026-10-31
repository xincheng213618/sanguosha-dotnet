#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActualUseTargetProgramsEntries
{
    internal void AdvanceActualUseTargetCandidate(global::CardGame.Core.ActualUseTargetWindowFrame parent, bool activated, bool completed) => _resolve().AdvanceActualUseTargetCandidate(parent, activated, completed);
    internal bool CanRunActualUseTarget(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanRunActualUseTarget(candidate, context);
    internal void CompleteActualUseTargetBinding(global::CardGame.Core.ProgramSkillFrame child, bool completed) => _resolve().CompleteActualUseTargetBinding(child, completed);
    internal void ContinueActualUseTargetWindowCore() => _resolve().ContinueActualUseTargetWindowCore();
    internal void ContinueAfterActualUseTargets(global::CardGame.Core.ActualUseTargetWindowFrame parent) => _resolve().ContinueAfterActualUseTargets(parent);
    internal global::CardGame.Core.ActualUseTargetIdentity? FreezeActualUseTarget(global::CardGame.Core.CardUseFrame use, int target) => _resolve().FreezeActualUseTarget(use, target);
    internal bool MatchesActualUseTarget(global::CardGame.Core.ActualUseTargetIdentity identity) => _resolve().MatchesActualUseTarget(identity);
    internal bool StartActualUseTargetWindow(global::CardGame.Core.CardUseFrame use, global::System.Collections.Generic.IReadOnlyList<int> targets, global::CardGame.Core.ActualUseTargetReturnKind kind, global::CardGame.Core.ActualUseTargetTrickReturn? trick) => _resolve().StartActualUseTargetWindow(use, targets, kind, trick);
    internal bool TryBeginActualUseTargetPrograms(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.ActualUseTargetReturnKind kind) => _resolve().TryBeginActualUseTargetPrograms(attack, kind);
    internal bool TryBeginActualUseTargetPrograms2(global::CardGame.Core.NativeOwners.JizhiResolution pending) => _resolve().TryBeginActualUseTargetPrograms(pending);
    internal bool TracksPaidOwnTargets { get => _resolve().TracksPaidOwnTargets; }
}
