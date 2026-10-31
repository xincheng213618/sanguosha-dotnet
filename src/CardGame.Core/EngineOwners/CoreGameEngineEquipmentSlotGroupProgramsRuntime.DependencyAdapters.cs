#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramCapability(typeof(CoreGameEngineEquipmentContentRuntime))]
internal sealed class CoreEquipmentSlotGroupProgramsCoreEquipmentContentAdapter(Func<CoreGameEngineEquipmentContentRuntime> targetFactory) : ICoreEquipmentSlotGroupProgramsCoreEquipmentContentOperations
{
    private readonly Func<CoreGameEngineEquipmentContentRuntime> _targetFactory = targetFactory;
    public EquipmentContentRegistry EquipmentRules { get => this._targetFactory().EquipmentRules; }
}

[Core.ProgramCapability(typeof(ProgramTargetSelectionRuntime))]
internal sealed class CoreEquipmentSlotGroupProgramsProgramTargetSelectionAdapter(Func<ProgramTargetSelectionRuntime> targetFactory) : ICoreEquipmentSlotGroupProgramsProgramTargetSelectionOperations
{
    private readonly Func<ProgramTargetSelectionRuntime> _targetFactory = targetFactory;
    public ProgramTargetSelectionRuntime Get() => this._targetFactory();
}
