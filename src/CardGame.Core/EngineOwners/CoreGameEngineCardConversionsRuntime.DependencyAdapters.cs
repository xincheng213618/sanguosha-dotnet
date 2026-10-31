#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreProgramCardLegalityRuntime))]
internal sealed class CoreCardConversionsCoreProgramCardLegalityAdapter(Func<CoreProgramCardLegalityRuntime> targetFactory) : ICoreCardConversionsCoreProgramCardLegalityOperations
{
    private readonly Func<CoreProgramCardLegalityRuntime> _targetFactory = targetFactory;
    public bool IsProgramMaterialUseForbidden(int actorSeat, CardKind kind, IEnumerable<int> cardIds, bool response = false) => this._targetFactory().IsProgramMaterialUseForbidden(actorSeat, kind, cardIds, response);
    public ProgramCardLegality CardLegality { get => this._targetFactory().CardLegality; }

    public bool IsProgramViewAsAllowed(CharacterState owner, CardKind kind, IReadOnlyList<int> cardIds, CardConversionSource source, bool forResponse, bool dyingUse = false, int? requestedUseTarget = default) => this._targetFactory().IsProgramViewAsAllowed(owner, kind, cardIds, source, forResponse, dyingUse, requestedUseTarget);
    public bool IsProgramEntityPhysicalRestricted(int actorSeat, int cardId) => this._targetFactory().IsProgramEntityPhysicalRestricted(actorSeat, cardId);
}

[Core.ProgramCapability(typeof(CoreGameEngineEquipmentContentRuntime))]
internal sealed class CoreCardConversionsCoreEquipmentContentAdapter(Func<CoreGameEngineEquipmentContentRuntime> targetFactory) : ICoreCardConversionsCoreEquipmentContentOperations
{
    private readonly Func<CoreGameEngineEquipmentContentRuntime> _targetFactory = targetFactory;
    public EquipmentContentRegistry EquipmentRules { get => this._targetFactory().EquipmentRules; }
}

[Core.ProgramCapability(typeof(CoreProgramDistanceRulesRuntime))]
internal sealed class CoreCardConversionsCoreProgramDistanceRulesAdapter(Func<CoreProgramDistanceRulesRuntime> targetFactory) : ICoreCardConversionsCoreProgramDistanceRulesOperations
{
    private readonly Func<CoreProgramDistanceRulesRuntime> _targetFactory = targetFactory;
    public ProgramDistanceRules DistanceRules { get => this._targetFactory().DistanceRules; }
}
