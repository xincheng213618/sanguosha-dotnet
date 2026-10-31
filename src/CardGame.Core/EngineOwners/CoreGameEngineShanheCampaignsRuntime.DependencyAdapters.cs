#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramCapability(typeof(CoreGameEngineEquipmentContentRuntime))]
internal sealed class CoreShanheCampaignsCoreEquipmentContentAdapter(Func<CoreGameEngineEquipmentContentRuntime> targetFactory) : ICoreShanheCampaignsCoreEquipmentContentOperations
{
    private readonly Func<CoreGameEngineEquipmentContentRuntime> _targetFactory = targetFactory;
    public EquipmentContentRegistry EquipmentRules { get => this._targetFactory().EquipmentRules; }
}

[Core.ProgramCapability(typeof(CoreTrustedCardZoneDiagnosticsRuntime))]
internal sealed class CoreShanheCampaignsCoreTrustedCardZoneDiagnosticsAdapter(Func<CoreTrustedCardZoneDiagnosticsRuntime> targetFactory) : ICoreShanheCampaignsCoreTrustedCardZoneDiagnosticsOperations
{
    private readonly Func<CoreTrustedCardZoneDiagnosticsRuntime> _targetFactory = targetFactory;
    public IReadOnlyList<CardZoneDiagnostic> CreateCardZoneDiagnostics() => this._targetFactory().CreateCardZoneDiagnostics();
}
