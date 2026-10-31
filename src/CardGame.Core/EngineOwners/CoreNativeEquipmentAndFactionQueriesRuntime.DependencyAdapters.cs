#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreGameEngineEquipmentContentRuntime))]
internal sealed class CoreNativeEquipmentAndFactionQueriesCoreEquipmentContentAdapter(Func<CoreGameEngineEquipmentContentRuntime> targetFactory) : ICoreNativeEquipmentAndFactionQueriesCoreEquipmentContentOperations
{
    private readonly Func<CoreGameEngineEquipmentContentRuntime> _targetFactory = targetFactory;
    public EquipmentContentRegistry EquipmentRules { get => this._targetFactory().EquipmentRules; }
}

[Core.ProgramCapability(typeof(CoreProgramDistanceRulesRuntime))]
internal sealed class CoreNativeEquipmentAndFactionQueriesCoreProgramDistanceRulesAdapter(Func<CoreProgramDistanceRulesRuntime> targetFactory) : ICoreNativeEquipmentAndFactionQueriesCoreProgramDistanceRulesOperations
{
    private readonly Func<CoreProgramDistanceRulesRuntime> _targetFactory = targetFactory;
    public ProgramDistanceRules DistanceRules { get => this._targetFactory().DistanceRules; }
}
