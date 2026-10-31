#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramTargetWindowProtocolsRuntime))]
internal sealed class CoreCurrentUseTargetCandidateRefreshCoreProgramTargetWindowProtocolsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramTargetWindowProtocolsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentUseTargetCandidateRefreshCoreProgramTargetWindowProtocolsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramTargetWindowProtocolsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramTargetWindowProtocols TargetWindowProtocols { get => this._targetFactory().TargetWindowProtocols; }
}
