#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreTrustedMovementDiagnosticsRuntime))]
internal sealed class CoreNativeMaterialSlashCoreTrustedMovementDiagnosticsAdapter(Func<CoreTrustedMovementDiagnosticsRuntime> targetFactory) : ICoreNativeMaterialSlashCoreTrustedMovementDiagnosticsOperations
{
    private readonly Func<CoreTrustedMovementDiagnosticsRuntime> _targetFactory = targetFactory;
    public IReadOnlyList<CardMovementRecord> CardMovements { get => this._targetFactory().CardMovements; }
}

[Core.ProgramCapability(typeof(CoreProgramTargetRulesRuntime))]
internal sealed class CoreNativeMaterialSlashCoreProgramTargetRulesAdapter(Func<CoreProgramTargetRulesRuntime> targetFactory) : ICoreNativeMaterialSlashCoreProgramTargetRulesOperations
{
    private readonly Func<CoreProgramTargetRulesRuntime> _targetFactory = targetFactory;
    public ProgramTargetRules TargetRules { get => this._targetFactory().TargetRules; }
}
