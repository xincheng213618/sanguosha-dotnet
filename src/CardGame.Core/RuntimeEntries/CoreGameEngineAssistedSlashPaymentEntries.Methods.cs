#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAssistedSlashPaymentEntries
{
    internal (IReadOnlyList<Card> Cards, CardKind Kind, CardConversionSource? Conversion) ReadAssistedSlashPayment(CharacterState actor, PromptChoice selected) => _resolve().ReadAssistedSlashPayment(actor, selected);
}
