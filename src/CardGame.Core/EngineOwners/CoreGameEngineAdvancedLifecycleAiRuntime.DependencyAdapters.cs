#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CorePlayerViewProjectionRuntime))]
internal sealed class CoreAdvancedLifecycleAiCorePlayerViewProjectionAdapter(Func<CorePlayerViewProjectionRuntime> targetFactory) : ICoreAdvancedLifecycleAiCorePlayerViewProjectionOperations
{
    private readonly Func<CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}

[Core.ProgramCapability(typeof(CoreGameEngineEquipmentContentRuntime))]
internal sealed class CoreAdvancedLifecycleAiCoreEquipmentContentAdapter(Func<CoreGameEngineEquipmentContentRuntime> targetFactory) : ICoreAdvancedLifecycleAiCoreEquipmentContentOperations
{
    private readonly Func<CoreGameEngineEquipmentContentRuntime> _targetFactory = targetFactory;
    public EquipmentContentRegistry EquipmentRules { get => this._targetFactory().EquipmentRules; }
}
