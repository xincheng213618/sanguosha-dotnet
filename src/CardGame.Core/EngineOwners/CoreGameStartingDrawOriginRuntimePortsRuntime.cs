#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameStartingDrawOriginRuntimePortsRuntime : global::CardGame.Core.IGameStartingDrawNativeFacts, global::CardGame.Core.IGameStartingDrawNativeReceipts, global::CardGame.Core.IGameStartingDrawNativeValidation
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterTangTangOlEntries _characterTangTangOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineOutsidePhaseDrawDiscardEntries _coreOutsidePhaseDrawDiscardOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupSetupCompleteAccess _coreMatchSetupSetupCompleteAccess;
    internal CoreGameStartingDrawOriginRuntimePortsRuntime(global::CardGame.Core.RuntimeEntries.CharacterTangTangOlEntries characterTangTangOlOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineOutsidePhaseDrawDiscardEntries coreOutsidePhaseDrawDiscardOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupSetupCompleteAccess coreMatchSetupSetupCompleteAccess)
    {
        this._characterTangTangOlOperations = characterTangTangOlOperations;
        this._coreOutsidePhaseDrawDiscardOperations = coreOutsidePhaseDrawDiscardOperations;
        this._coreMatchSetupSetupCompleteAccess = coreMatchSetupSetupCompleteAccess;
    }
}
