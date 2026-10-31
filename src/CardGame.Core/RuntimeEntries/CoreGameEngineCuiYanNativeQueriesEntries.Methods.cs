#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCuiYanNativeQueriesEntries
{
    internal bool CanSupplyCuiYanFactionSlashVariant(global::CardGame.Core.NativeOwners.FactionCardRequestHandle request, global::CardGame.Core.CharacterState provider, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> materials, global::CardGame.Core.CardConversionSource? source = default) => _resolve().CanSupplyCuiYanFactionSlashVariant(request, provider, kind, materials, source);
    internal bool CuiYanDyingConversionForbidden(global::CardGame.Core.ProgramViewAsSourceQuery query) => _resolve().CuiYanDyingConversionForbidden(query);
    internal bool HasCuiYanAllowedFactionSlashVariant(global::CardGame.Core.NativeOwners.FactionCardRequestHandle request, global::CardGame.Core.CharacterState provider, global::CardGame.Core.Card card) => _resolve().HasCuiYanAllowedFactionSlashVariant(request, provider, card);
    internal bool IsCuiYanFactionSlashChoiceForbidden(global::CardGame.Core.NativeOwners.FactionCardRequestHandle request, global::CardGame.Core.PromptChoice choice) => _resolve().IsCuiYanFactionSlashChoiceForbidden(request, choice);
    internal bool IsCuiYanFactionSlashUseForbidden(global::CardGame.Core.NativeOwners.FactionCardRequestHandle request, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> materials, global::CardGame.Core.CardConversionSource? conversion = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversions = default) => _resolve().IsCuiYanFactionSlashUseForbidden(request, kind, materials, conversion, additionalConversions);
    internal bool IsCuiYanSingleUseForbidden(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.CardConversionSource? conversion = default) => _resolve().IsCuiYanSingleUseForbidden(actor, card, conversion);
    internal bool IsCuiYanSlashChoiceForbidden(int actorSeat, global::CardGame.Core.PromptChoice choice) => _resolve().IsCuiYanSlashChoiceForbidden(actorSeat, choice);
}
