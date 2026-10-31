#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeLogicalDiscardRuntimePortsRuntime
{
    bool global::CardGame.Core.INativeLogicalDiscardFacts.SetupComplete => _coreMatchSetupSetupCompleteAccess._setupComplete;

    int global::CardGame.Core.INativeLogicalDiscardFacts.MovementSequence => _coreCardMovementJournalMovementSequenceAccess._movementSequence;

    long global::CardGame.Core.INativeLogicalDiscardFacts.ResolutionSequence => _coreResolutionIdentityBoundaryOperations._resolutionSequence;

    bool global::CardGame.Core.INativeLogicalDiscardOrigins.IsOwnedDiscardSource(CardLocation location) => global::CardGame.Core.EngineOwners.CoreGameEngineProgramDiscardOriginsRuntime.IsOwnedDiscardSource(location);
    bool global::CardGame.Core.INativeLogicalDiscardOrigins.IsProgramDiscardOriginReason(CardMoveReason reason) => global::CardGame.Core.EngineOwners.CoreGameEngineProgramDiscardOriginsRuntime.IsProgramDiscardOriginReason(reason);
    CardLocation? global::CardGame.Core.INativeLogicalDiscardOrigins.GetProgramDiscardSource(CardMovementRecord movement) => _coreProgramDiscardOriginsOperations.GetProgramDiscardSource(movement);
    bool global::CardGame.Core.INativeLogicalDiscardOrigins.IsEphemeralCardDestroyedMaterial(CardMovementRecord movement) => _coreEphemeralCardMaterialsOperations.IsEphemeralCardDestroyedMaterial(movement);
    NativeLogicalDiscardInvocationDraft? global::CardGame.Core.INativeLogicalDiscardAdmissions.Admit(NativeLogicalDiscardInvocationDraft? draft) => _characterZhangFeiWumiaoOperations.AdmitWuZishengNativeDiscard(draft);
    void global::CardGame.Core.INativeLogicalDiscardAdmissions.AdmitMaterials(NativeLogicalDiscardInvocationDraft draft) => _characterZhangFeiWumiaoOperations.AdmitWuZishengDiscardMaterials(draft);
    void global::CardGame.Core.INativeLogicalDiscardAdmissions.Close(NativeLogicalDiscardInvocationDraft? draft) => _characterZhangFeiWumiaoOperations.CloseWuZishengNativeDiscard(draft);
}
