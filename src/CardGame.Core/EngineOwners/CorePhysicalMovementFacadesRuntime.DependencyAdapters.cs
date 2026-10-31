#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramCapability(typeof(MovementRecordRuntime))]
internal sealed class CorePhysicalMovementFacadesMovementRecordAdapter(Func<MovementRecordRuntime> targetFactory) : ICorePhysicalMovementFacadesMovementRecordOperations
{
    private readonly Func<MovementRecordRuntime> _targetFactory = targetFactory;
    public MovementRecordRuntime Get() => this._targetFactory();
}

[Core.ProgramCapability(typeof(MovementEquipmentEffectsRuntime))]
internal sealed class CorePhysicalMovementFacadesMovementEquipmentEffectsAdapter(Func<MovementEquipmentEffectsRuntime> targetFactory) : ICorePhysicalMovementFacadesMovementEquipmentEffectsOperations
{
    private readonly Func<MovementEquipmentEffectsRuntime> _targetFactory = targetFactory;
    public MovementEquipmentEffectsRuntime Get() => this._targetFactory();
}
