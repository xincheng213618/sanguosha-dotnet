#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineExtendedViewAsEntries
{
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ExtendedViewAsResponseChoices(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind kind) => _resolve().ExtendedViewAsResponseChoices(owner, kind);
    internal bool IsArmorIneffectiveForTurn(global::CardGame.Core.CharacterState target) => _resolve().IsArmorIneffectiveForTurn(target);
    internal int ProgramConversionDamageBonus(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().ProgramConversionDamageBonus(attack);
    internal void ResolveExtendedViewAsResponse(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => _resolve().ResolveExtendedViewAsResponse(owner, selection);
    internal void ResolveExtendedViewAsResponse2(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveExtendedViewAsResponse(choice);
    internal bool TryResolveExtendedAiResponse(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind) => _resolve().TryResolveExtendedAiResponse(owner, card, kind);
}
