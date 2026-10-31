#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTrustedCardZoneDiagnosticsRuntime))]
internal sealed class CharacterZhugeLiangWumiaoCoreTrustedCardZoneDiagnosticsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTrustedCardZoneDiagnosticsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhugeLiangWumiaoCoreTrustedCardZoneDiagnosticsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTrustedCardZoneDiagnosticsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardZoneDiagnostic> CreateCardZoneDiagnostics() => this._targetFactory().CreateCardZoneDiagnostics();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime))]
internal sealed class CharacterZhugeLiangWumiaoCorePlayerViewProjectionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhugeLiangWumiaoCorePlayerViewProjectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}
