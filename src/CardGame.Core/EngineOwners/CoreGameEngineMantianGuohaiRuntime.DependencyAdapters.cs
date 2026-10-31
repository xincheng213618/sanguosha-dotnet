#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreTrustedCardZoneDiagnosticsRuntime))]
internal sealed class CoreMantianGuohaiCoreTrustedCardZoneDiagnosticsAdapter(Func<CoreTrustedCardZoneDiagnosticsRuntime> targetFactory) : ICoreMantianGuohaiCoreTrustedCardZoneDiagnosticsOperations
{
    private readonly Func<CoreTrustedCardZoneDiagnosticsRuntime> _targetFactory = targetFactory;
    public IReadOnlyList<CardZoneDiagnostic> CreateCardZoneDiagnostics() => this._targetFactory().CreateCardZoneDiagnostics();
}

[Core.ProgramCapability(typeof(CoreProgramTargetRulesRuntime))]
internal sealed class CoreMantianGuohaiCoreProgramTargetRulesAdapter(Func<CoreProgramTargetRulesRuntime> targetFactory) : ICoreMantianGuohaiCoreProgramTargetRulesOperations
{
    private readonly Func<CoreProgramTargetRulesRuntime> _targetFactory = targetFactory;
    public ProgramTargetRules TargetRules { get => this._targetFactory().TargetRules; }
}
