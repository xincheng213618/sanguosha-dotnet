#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramCardLegalityRuntime
{
    internal ProgramCardLegality CardLegality => _cardLegalityOperations._cardLegality ??= new(CoreProgramCardLegalityDefinitions.CardLegalityMetadata.Value, _kernelCoreProgramRuntimeModulesOperations.BindRuntimeHandler);

    internal bool IsProgramMaterialUseForbidden(int actorSeat, CardKind kind, IEnumerable<int> cardIds, bool response = false) => CardLegality.IsMaterialForbidden(new(actorSeat, kind, cardIds, response));
    internal bool IsProgramViewAsAllowed(CharacterState owner, CardKind kind, IReadOnlyList<int> cardIds, CardConversionSource source, bool forResponse, bool dyingUse = false, int? requestedUseTarget = null) => _characterYuanShaoBoundaryMobileOperations.Value.IsMobileYuanShaoViewAsAllowed(owner, cardIds, source) && !CardLegality.IsViewAsForbidden(new(owner, kind, cardIds, source, forResponse, dyingUse, requestedUseTarget));
    internal bool IsProgramHandPhysicalRestricted(int actorSeat, int cardId) => CardLegality.IsPhysicalForbidden(ProgramCardLegalityStage.HandPhysicalBan, actorSeat, cardId);
    internal bool IsProgramEntityPhysicalRestricted(int actorSeat, int cardId) => CardLegality.IsPhysicalForbidden(ProgramCardLegalityStage.EntityPhysicalBan, actorSeat, cardId);
    internal bool IsProgramNativeResponseAllowed(CharacterState provider, CardKind kind, IReadOnlyList<int> cardIds, CardConversionSource? source = null, bool dyingUse = false) => !CardLegality.IsNativeResponseForbidden(new(provider, kind, cardIds, source, dyingUse));
}
