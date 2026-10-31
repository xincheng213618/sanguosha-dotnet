#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterMaChaoAnniversaryGodCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}

internal interface ICharacterMaChaoAnniversaryGodCoreProgramCardLegalityOperations : IProgramRuntimeOperations
{
    bool IsProgramEntityPhysicalRestricted(int actorSeat, int cardId);
    bool IsProgramMaterialUseForbidden(int actorSeat, CardKind kind, IEnumerable<int> cardIds, bool response = false);
    bool IsProgramViewAsAllowed(CharacterState owner, CardKind kind, IReadOnlyList<int> cardIds, CardConversionSource source, bool forResponse, bool dyingUse = false, int? requestedUseTarget = default);
    ProgramCardLegality CardLegality { get; }
}
