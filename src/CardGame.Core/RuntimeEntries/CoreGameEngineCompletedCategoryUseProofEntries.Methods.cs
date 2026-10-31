#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCompletedCategoryUseProofEntries
{
    internal bool ActualHandGainMutationBinding(long frameId, string skill, int owner, global::CardGame.Core.SkillProgramTriggerWindow window, global::CardGame.Core.SkillProgramEffectOp operation) => _resolve().ActualHandGainMutationBinding(frameId, skill, owner, window, operation);
    internal bool HasExactAcceptedActualHandGainUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardActionContext completed) => _resolve().HasExactAcceptedActualHandGainUse(use, completed);
    internal bool IsBingzhongAcceptedUseTargetFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.BingzhongTargetsAppliedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> precedingTargets, int actor, int provider, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.IGameEvent> history, scoped out global::System.Collections.Generic.IReadOnlyList<int> resultTargets) => _resolve().IsBingzhongAcceptedUseTargetFact(use, fact, precedingTargets, actor, provider, history, out resultTargets);
    internal bool IsWeifuAcceptedUseTargetFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.WeifuTargetAddedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> precedingTargets, int actor, int provider, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.IGameEvent> history) => _resolve().IsWeifuAcceptedUseTargetFact(use, fact, precedingTargets, actor, provider, history);
    internal bool RebuildAcceptedActualHandGainUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardActionContext accepted, global::CardGame.Core.CardActionContext completed, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.IGameEvent> history, int acceptedIndex) => _resolve().RebuildAcceptedActualHandGainUse(use, accepted, completed, history, acceptedIndex);
}
