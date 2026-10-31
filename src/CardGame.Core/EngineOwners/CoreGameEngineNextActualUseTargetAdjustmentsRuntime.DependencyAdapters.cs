#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreGameEngineEquipmentContentRuntime))]
internal sealed class CoreNextActualUseTargetAdjustmentsCoreEquipmentContentAdapter(Func<CoreGameEngineEquipmentContentRuntime> targetFactory) : ICoreNextActualUseTargetAdjustmentsCoreEquipmentContentOperations
{
    private readonly Func<CoreGameEngineEquipmentContentRuntime> _targetFactory = targetFactory;
    public EquipmentContentRegistry EquipmentRules { get => this._targetFactory().EquipmentRules; }
}

[Core.ProgramCapability(typeof(CoreTrustedMovementDiagnosticsRuntime))]
internal sealed class CoreNextActualUseTargetAdjustmentsCoreTrustedMovementDiagnosticsAdapter(Func<CoreTrustedMovementDiagnosticsRuntime> targetFactory) : ICoreNextActualUseTargetAdjustmentsCoreTrustedMovementDiagnosticsOperations
{
    private readonly Func<CoreTrustedMovementDiagnosticsRuntime> _targetFactory = targetFactory;
    public IReadOnlyList<CardMovementRecord> CardMovements { get => this._targetFactory().CardMovements; }
}
