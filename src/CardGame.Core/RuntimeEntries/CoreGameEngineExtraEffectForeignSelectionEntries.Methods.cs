#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineExtraEffectForeignSelectionEntries
{
    internal bool AssertExtraForeignSelection(global::CardGame.Core.TargetCardSelectionFrame pending, global::CardGame.Core.PendingDecision? decision, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => _resolve().AssertExtraForeignSelection(pending, decision, processing);
    internal bool IsExtraForeignPublicAnswer(global::CardGame.Core.TargetCardSelectionFrame pending, global::CardGame.Core.PromptChoice selected) => _resolve().IsExtraForeignPublicAnswer(pending, selected);
    internal bool TryBeginExtraForeignSelection(global::CardGame.Core.NullificationWindowFrame pending, global::CardGame.Core.CharacterState target) => _resolve().TryBeginExtraForeignSelection(pending, target);
    internal bool TryResolveExtraForeignSelection(global::CardGame.Core.TargetCardSelectionFrame pending, int slot) => _resolve().TryResolveExtraForeignSelection(pending, slot);
}
