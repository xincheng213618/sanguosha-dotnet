#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSequentialTrickTargetsEntries
{
    internal IReadOnlyList<int> FreezeSequentialTrickTarget(long frameId, IReadOnlyList<int> targets, LegalActionKind kind, int? firstTargetCardId, CardKind? requiredCardKind) => _resolve().FreezeSequentialTrickTarget(frameId, targets, kind, firstTargetCardId, requiredCardKind);
    internal bool HasRemainingSequentialTrickTargets(long frameId) => _resolve().HasRemainingSequentialTrickTargets(frameId);
    internal void MoveFinishedTrickCard(long frameId, Card card) => _resolve().MoveFinishedTrickCard(frameId, card);
    internal bool TryContinueSequentialTrick(long frameId, bool afterAttack = false) => _resolve().TryContinueSequentialTrick(frameId, afterAttack);
}
