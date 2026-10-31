#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineExtendedViewAsEntries
{
    internal IReadOnlyList<PromptChoice> ExtendedViewAsResponseChoices(CharacterState owner, CardKind kind) => _resolve().ExtendedViewAsResponseChoices(owner, kind);
    internal bool IsArmorIneffectiveForTurn(CharacterState target) => _resolve().IsArmorIneffectiveForTurn(target);
    internal int ProgramConversionDamageBonus(NativeOwners.CardAttackHandle attack) => _resolve().ProgramConversionDamageBonus(attack);
    internal void ResolveExtendedViewAsResponse(CharacterState owner, ProgramMultiCardViewAsSelection selection) => _resolve().ResolveExtendedViewAsResponse(owner, selection);
    internal void ResolveExtendedViewAsResponse2(PromptChoice choice) => _resolve().ResolveExtendedViewAsResponse(choice);
    internal bool TryResolveExtendedAiResponse(CharacterState owner, Card card, CardKind kind) => _resolve().TryResolveExtendedAiResponse(owner, card, kind);
}
