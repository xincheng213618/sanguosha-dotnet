#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreGameEngineEquipmentContentRuntime))]
internal sealed class CoreFriendTrioCoreEquipmentContentAdapter(Func<CoreGameEngineEquipmentContentRuntime> targetFactory) : ICoreFriendTrioCoreEquipmentContentOperations
{
    private readonly Func<CoreGameEngineEquipmentContentRuntime> _targetFactory = targetFactory;
    public EquipmentContentRegistry EquipmentRules { get => this._targetFactory().EquipmentRules; }
}

[Core.ProgramCapability(typeof(CoreGameEngineModeAllegianceRuntime))]
internal sealed class CoreFriendTrioCoreModeAllegianceAdapter(Func<CoreGameEngineModeAllegianceRuntime> targetFactory) : ICoreFriendTrioCoreModeAllegianceOperations
{
    private readonly Func<CoreGameEngineModeAllegianceRuntime> _targetFactory = targetFactory;
    public IMatchModeAllegiancePolicy ModeAllegiancePolicy { get => this._targetFactory().ModeAllegiancePolicy; }
}

[Core.ProgramCapability(typeof(CoreTrustedCardZoneDiagnosticsRuntime))]
internal sealed class CoreFriendTrioCoreTrustedCardZoneDiagnosticsAdapter(Func<CoreTrustedCardZoneDiagnosticsRuntime> targetFactory) : ICoreFriendTrioCoreTrustedCardZoneDiagnosticsOperations
{
    private readonly Func<CoreTrustedCardZoneDiagnosticsRuntime> _targetFactory = targetFactory;
    public IReadOnlyList<CardZoneDiagnostic> CreateCardZoneDiagnostics() => this._targetFactory().CreateCardZoneDiagnostics();
}
