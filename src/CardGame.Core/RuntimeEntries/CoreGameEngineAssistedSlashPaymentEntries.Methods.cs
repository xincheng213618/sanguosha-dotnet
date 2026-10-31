#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAssistedSlashPaymentEntries
{
    internal (global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> Cards, global::CardGame.Core.CardKind Kind, global::CardGame.Core.CardConversionSource? Conversion) ReadAssistedSlashPayment(global::CardGame.Core.CharacterState actor, global::CardGame.Core.PromptChoice selected) => _resolve().ReadAssistedSlashPayment(actor, selected);
}
