#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameStartingDrawOriginRuntimePortsRuntime : global::CardGame.Core.IGameStartingDrawNativeFacts, global::CardGame.Core.IGameStartingDrawNativeReceipts, global::CardGame.Core.IGameStartingDrawNativeValidation
{
    private readonly ICoreGameStartingDrawOriginRuntimePortsCharacterTangTangOlOperations _characterTangTangOlOperations;
    private readonly ICoreGameStartingDrawOriginRuntimePortsCoreOutsidePhaseDrawDiscardOperations _coreOutsidePhaseDrawDiscardOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupSetupCompleteAccess _coreMatchSetupSetupCompleteAccess;
    internal CoreGameStartingDrawOriginRuntimePortsRuntime(ICoreGameStartingDrawOriginRuntimePortsCharacterTangTangOlOperations characterTangTangOlOperations, ICoreGameStartingDrawOriginRuntimePortsCoreOutsidePhaseDrawDiscardOperations coreOutsidePhaseDrawDiscardOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupSetupCompleteAccess coreMatchSetupSetupCompleteAccess)
    {
        this._characterTangTangOlOperations = characterTangTangOlOperations;
        this._coreOutsidePhaseDrawDiscardOperations = coreOutsidePhaseDrawDiscardOperations;
        this._coreMatchSetupSetupCompleteAccess = coreMatchSetupSetupCompleteAccess;
    }
}
