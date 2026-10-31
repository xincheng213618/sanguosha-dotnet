#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreGameEngineEquipmentContentRuntime))]
internal sealed class CharacterMaChaoAnniversaryGodCoreEquipmentContentAdapter(Func<CoreGameEngineEquipmentContentRuntime> targetFactory) : ICharacterMaChaoAnniversaryGodCoreEquipmentContentOperations
{
    private readonly Func<CoreGameEngineEquipmentContentRuntime> _targetFactory = targetFactory;
    public EquipmentContentRegistry EquipmentRules { get => this._targetFactory().EquipmentRules; }
}

[Core.ProgramCapability(typeof(CoreProgramCardLegalityRuntime))]
internal sealed class CharacterMaChaoAnniversaryGodCoreProgramCardLegalityAdapter(Func<CoreProgramCardLegalityRuntime> targetFactory) : ICharacterMaChaoAnniversaryGodCoreProgramCardLegalityOperations
{
    private readonly Func<CoreProgramCardLegalityRuntime> _targetFactory = targetFactory;
    public bool IsProgramEntityPhysicalRestricted(int actorSeat, int cardId) => this._targetFactory().IsProgramEntityPhysicalRestricted(actorSeat, cardId);
    public bool IsProgramMaterialUseForbidden(int actorSeat, CardKind kind, IEnumerable<int> cardIds, bool response = false) => this._targetFactory().IsProgramMaterialUseForbidden(actorSeat, kind, cardIds, response);
    public bool IsProgramViewAsAllowed(CharacterState owner, CardKind kind, IReadOnlyList<int> cardIds, CardConversionSource source, bool forResponse, bool dyingUse = false, int? requestedUseTarget = default) => this._targetFactory().IsProgramViewAsAllowed(owner, kind, cardIds, source, forResponse, dyingUse, requestedUseTarget);
    public ProgramCardLegality CardLegality { get => this._targetFactory().CardLegality; }
}
