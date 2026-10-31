#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSequentialTrickTargetsEntries
{
    internal global::System.Collections.Generic.IReadOnlyList<int> FreezeSequentialTrickTarget(long frameId, global::System.Collections.Generic.IReadOnlyList<int> targets, global::CardGame.Core.LegalActionKind kind, int? firstTargetCardId, global::CardGame.Core.CardKind? requiredCardKind) => _resolve().FreezeSequentialTrickTarget(frameId, targets, kind, firstTargetCardId, requiredCardKind);
    internal bool HasRemainingSequentialTrickTargets(long frameId) => _resolve().HasRemainingSequentialTrickTargets(frameId);
    internal void MoveFinishedTrickCard(long frameId, global::CardGame.Core.Card card) => _resolve().MoveFinishedTrickCard(frameId, card);
    internal bool TryContinueSequentialTrick(long frameId, bool afterAttack = false) => _resolve().TryContinueSequentialTrick(frameId, afterAttack);
}
