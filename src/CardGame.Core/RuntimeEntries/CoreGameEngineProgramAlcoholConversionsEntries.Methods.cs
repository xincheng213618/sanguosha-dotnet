#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramAlcoholConversionsEntries
{
    internal void ValidateAlcoholConversion(CharacterState owner, Card card, CardConversionSource? source, bool forResponse) => _resolve().ValidateAlcoholConversion(owner, card, source, forResponse);
}
