#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCuiYanNativeQueriesEntries
{
    internal bool CanSupplyCuiYanFactionSlashVariant(NativeOwners.FactionCardRequestHandle request, CharacterState provider, CardKind kind, IReadOnlyList<Card> materials, CardConversionSource? source = default) => _resolve().CanSupplyCuiYanFactionSlashVariant(request, provider, kind, materials, source);
    internal bool CuiYanDyingConversionForbidden(ProgramViewAsSourceQuery query) => _resolve().CuiYanDyingConversionForbidden(query);
    internal bool HasCuiYanAllowedFactionSlashVariant(NativeOwners.FactionCardRequestHandle request, CharacterState provider, Card card) => _resolve().HasCuiYanAllowedFactionSlashVariant(request, provider, card);
    internal bool IsCuiYanFactionSlashChoiceForbidden(NativeOwners.FactionCardRequestHandle request, PromptChoice choice) => _resolve().IsCuiYanFactionSlashChoiceForbidden(request, choice);
    internal bool IsCuiYanFactionSlashUseForbidden(NativeOwners.FactionCardRequestHandle request, CardKind kind, IReadOnlyList<Card> materials, CardConversionSource? conversion = default, IReadOnlyList<CardConversionSource>? additionalConversions = default) => _resolve().IsCuiYanFactionSlashUseForbidden(request, kind, materials, conversion, additionalConversions);
    internal bool IsCuiYanSingleUseForbidden(CharacterState actor, Card card, CardConversionSource? conversion = default) => _resolve().IsCuiYanSingleUseForbidden(actor, card, conversion);
    internal bool IsCuiYanSlashChoiceForbidden(int actorSeat, PromptChoice choice) => _resolve().IsCuiYanSlashChoiceForbidden(actorSeat, choice);
}
