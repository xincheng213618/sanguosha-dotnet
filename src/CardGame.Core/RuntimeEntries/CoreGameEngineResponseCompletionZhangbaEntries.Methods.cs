#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineResponseCompletionZhangbaEntries
{
    internal IReadOnlyList<CardActionCost>? CaptureZhangbaResponseCompletionCosts(CharacterState responder, IReadOnlyList<Card> pair) => _resolve().CaptureZhangbaResponseCompletionCosts(responder, pair);
}
