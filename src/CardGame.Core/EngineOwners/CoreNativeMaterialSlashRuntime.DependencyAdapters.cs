#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTrustedMovementDiagnosticsRuntime))]
internal sealed class CoreNativeMaterialSlashCoreTrustedMovementDiagnosticsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTrustedMovementDiagnosticsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeMaterialSlashCoreTrustedMovementDiagnosticsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTrustedMovementDiagnosticsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord> CardMovements { get => this._targetFactory().CardMovements; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramTargetRulesRuntime))]
internal sealed class CoreNativeMaterialSlashCoreProgramTargetRulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramTargetRulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeMaterialSlashCoreProgramTargetRulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramTargetRulesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramTargetRules TargetRules { get => this._targetFactory().TargetRules; }
}
