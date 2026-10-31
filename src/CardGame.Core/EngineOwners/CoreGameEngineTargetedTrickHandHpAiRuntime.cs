#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineTargetedTrickHandHpAiRuntime
{
    private readonly RuntimeEntries.CoreGameEngineTargetedOrdinaryTrickEntries _coreTargetedOrdinaryTrickOperations;
    private readonly ICoreTargetedTrickHandHpAiCorePlayerViewProjectionOperations _corePlayerViewProjectionOperations;
    private readonly EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess _coreAiBrainsAiBrainsAccess;
    internal CoreGameEngineTargetedTrickHandHpAiRuntime(RuntimeEntries.CoreGameEngineTargetedOrdinaryTrickEntries coreTargetedOrdinaryTrickOperations, ICoreTargetedTrickHandHpAiCorePlayerViewProjectionOperations corePlayerViewProjectionOperations, EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess coreAiBrainsAiBrainsAccess)
    {
        this._coreTargetedOrdinaryTrickOperations = coreTargetedOrdinaryTrickOperations;
        this._corePlayerViewProjectionOperations = corePlayerViewProjectionOperations;
        this._coreAiBrainsAiBrainsAccess = coreAiBrainsAiBrainsAccess;
    }
}
