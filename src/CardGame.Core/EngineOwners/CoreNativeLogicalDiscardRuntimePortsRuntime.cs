#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreNativeLogicalDiscardRuntimePortsRuntime : global::CardGame.Core.INativeLogicalDiscardAdmissions, global::CardGame.Core.INativeLogicalDiscardFacts, global::CardGame.Core.INativeLogicalDiscardOrigins
{
    private readonly ICoreNativeLogicalDiscardRuntimePortsCharacterZhangFeiWumiaoOperations _characterZhangFeiWumiaoOperations;
    private readonly ICoreNativeLogicalDiscardRuntimePortsCoreEphemeralCardMaterialsOperations _coreEphemeralCardMaterialsOperations;
    private readonly ICoreNativeLogicalDiscardRuntimePortsCoreProgramDiscardOriginsOperations _coreProgramDiscardOriginsOperations;
    private readonly ICoreNativeLogicalDiscardRuntimePortsCoreResolutionIdentityBoundaryOperations _coreResolutionIdentityBoundaryOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess _coreCardMovementJournalMovementSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupSetupCompleteAccess _coreMatchSetupSetupCompleteAccess;
    internal CoreNativeLogicalDiscardRuntimePortsRuntime(ICoreNativeLogicalDiscardRuntimePortsCharacterZhangFeiWumiaoOperations characterZhangFeiWumiaoOperations, ICoreNativeLogicalDiscardRuntimePortsCoreEphemeralCardMaterialsOperations coreEphemeralCardMaterialsOperations, ICoreNativeLogicalDiscardRuntimePortsCoreProgramDiscardOriginsOperations coreProgramDiscardOriginsOperations, ICoreNativeLogicalDiscardRuntimePortsCoreResolutionIdentityBoundaryOperations coreResolutionIdentityBoundaryOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess coreCardMovementJournalMovementSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupSetupCompleteAccess coreMatchSetupSetupCompleteAccess)
    {
        this._characterZhangFeiWumiaoOperations = characterZhangFeiWumiaoOperations;
        this._coreEphemeralCardMaterialsOperations = coreEphemeralCardMaterialsOperations;
        this._coreProgramDiscardOriginsOperations = coreProgramDiscardOriginsOperations;
        this._coreResolutionIdentityBoundaryOperations = coreResolutionIdentityBoundaryOperations;
        this._coreCardMovementJournalMovementSequenceAccess = coreCardMovementJournalMovementSequenceAccess;
        this._coreMatchSetupSetupCompleteAccess = coreMatchSetupSetupCompleteAccess;
    }
}
