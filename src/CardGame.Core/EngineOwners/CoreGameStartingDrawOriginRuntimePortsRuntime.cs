#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule, Core.ProgramCapability]
internal sealed partial class CoreGameStartingDrawOriginRuntimePortsRuntime : IGameStartingDrawNativeFacts, IGameStartingDrawNativeReceipts, IGameStartingDrawNativeValidation
{
    private readonly RuntimeEntries.CharacterTangTangOlEntries _characterTangTangOlOperations;
    private readonly RuntimeEntries.CoreGameEngineOutsidePhaseDrawDiscardEntries _coreOutsidePhaseDrawDiscardOperations;
    private readonly EngineStates.Contracts.ICoreMatchSetupSetupCompleteAccess _coreMatchSetupSetupCompleteAccess;
    internal CoreGameStartingDrawOriginRuntimePortsRuntime(RuntimeEntries.CharacterTangTangOlEntries characterTangTangOlOperations, RuntimeEntries.CoreGameEngineOutsidePhaseDrawDiscardEntries coreOutsidePhaseDrawDiscardOperations, EngineStates.Contracts.ICoreMatchSetupSetupCompleteAccess coreMatchSetupSetupCompleteAccess)
    {
        this._characterTangTangOlOperations = characterTangTangOlOperations;
        this._coreOutsidePhaseDrawDiscardOperations = coreOutsidePhaseDrawDiscardOperations;
        this._coreMatchSetupSetupCompleteAccess = coreMatchSetupSetupCompleteAccess;
    }
}
