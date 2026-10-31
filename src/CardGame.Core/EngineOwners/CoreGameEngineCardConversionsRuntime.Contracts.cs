#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreCardConversionsCoreProgramCardLegalityOperations : IProgramRuntimeOperations
{
    bool IsProgramMaterialUseForbidden(int actorSeat, CardKind kind, IEnumerable<int> cardIds, bool response = false);
    ProgramCardLegality CardLegality { get; }

    bool IsProgramViewAsAllowed(CharacterState owner, CardKind kind, IReadOnlyList<int> cardIds, CardConversionSource source, bool forResponse, bool dyingUse = false, int? requestedUseTarget = default);
    bool IsProgramEntityPhysicalRestricted(int actorSeat, int cardId);
}

internal interface ICoreCardConversionsCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}

internal interface ICoreCardConversionsCoreProgramDistanceRulesOperations : IProgramRuntimeOperations
{
    ProgramDistanceRules DistanceRules { get; }
}
