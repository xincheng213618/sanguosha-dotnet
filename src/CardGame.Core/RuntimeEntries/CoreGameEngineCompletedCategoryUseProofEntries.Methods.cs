#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCompletedCategoryUseProofEntries
{
    internal bool ActualHandGainMutationBinding(long frameId, string skill, int owner, SkillProgramTriggerWindow window, SkillProgramEffectOp operation) => _resolve().ActualHandGainMutationBinding(frameId, skill, owner, window, operation);
    internal bool HasExactAcceptedActualHandGainUse(CardUseFrame use, CardActionContext completed) => _resolve().HasExactAcceptedActualHandGainUse(use, completed);
    internal bool IsBingzhongAcceptedUseTargetFact(CardUseFrame use, BingzhongTargetsAppliedEvent fact, IReadOnlyList<int> precedingTargets, int actor, int provider, IReadOnlyList<IGameEvent> history, scoped out IReadOnlyList<int> resultTargets) => _resolve().IsBingzhongAcceptedUseTargetFact(use, fact, precedingTargets, actor, provider, history, out resultTargets);
    internal bool IsWeifuAcceptedUseTargetFact(CardUseFrame use, WeifuTargetAddedEvent fact, IReadOnlyList<int> precedingTargets, int actor, int provider, IReadOnlyList<IGameEvent> history) => _resolve().IsWeifuAcceptedUseTargetFact(use, fact, precedingTargets, actor, provider, history);
    internal bool RebuildAcceptedActualHandGainUse(CardUseFrame use, CardActionContext accepted, CardActionContext completed, IReadOnlyList<IGameEvent> history, int acceptedIndex) => _resolve().RebuildAcceptedActualHandGainUse(use, accepted, completed, history, acceptedIndex);
}
