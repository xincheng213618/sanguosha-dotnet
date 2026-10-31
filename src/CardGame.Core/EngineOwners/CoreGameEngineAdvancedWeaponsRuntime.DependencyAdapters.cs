#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreTrustedCardZoneDiagnosticsRuntime))]
internal sealed class CoreAdvancedWeaponsCoreTrustedCardZoneDiagnosticsAdapter(Func<CoreTrustedCardZoneDiagnosticsRuntime> targetFactory) : ICoreAdvancedWeaponsCoreTrustedCardZoneDiagnosticsOperations
{
    private readonly Func<CoreTrustedCardZoneDiagnosticsRuntime> _targetFactory = targetFactory;
    public IReadOnlyList<CardZoneDiagnostic> CreateCardZoneDiagnostics() => this._targetFactory().CreateCardZoneDiagnostics();
}

[Core.ProgramCapability(typeof(CoreGameEngineEquipmentContentRuntime))]
internal sealed class CoreAdvancedWeaponsCoreEquipmentContentAdapter(Func<CoreGameEngineEquipmentContentRuntime> targetFactory) : ICoreAdvancedWeaponsCoreEquipmentContentOperations
{
    private readonly Func<CoreGameEngineEquipmentContentRuntime> _targetFactory = targetFactory;
    public EquipmentContentRegistry EquipmentRules { get => this._targetFactory().EquipmentRules; }
}
