#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineTargetedTrickHandHpAiRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTargetedOrdinaryTrickEntries _coreTargetedOrdinaryTrickOperations;
    private readonly ICoreTargetedTrickHandHpAiCorePlayerViewProjectionOperations _corePlayerViewProjectionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess _coreAiBrainsAiBrainsAccess;
    internal CoreGameEngineTargetedTrickHandHpAiRuntime(global::CardGame.Core.RuntimeEntries.CoreGameEngineTargetedOrdinaryTrickEntries coreTargetedOrdinaryTrickOperations, ICoreTargetedTrickHandHpAiCorePlayerViewProjectionOperations corePlayerViewProjectionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess coreAiBrainsAiBrainsAccess)
    {
        this._coreTargetedOrdinaryTrickOperations = coreTargetedOrdinaryTrickOperations;
        this._corePlayerViewProjectionOperations = corePlayerViewProjectionOperations;
        this._coreAiBrainsAiBrainsAccess = coreAiBrainsAiBrainsAccess;
    }
}
