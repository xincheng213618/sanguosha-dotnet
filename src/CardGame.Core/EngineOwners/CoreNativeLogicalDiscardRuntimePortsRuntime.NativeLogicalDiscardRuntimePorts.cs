#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeLogicalDiscardRuntimePortsRuntime
{
    bool INativeLogicalDiscardFacts.SetupComplete => _coreMatchSetupSetupCompleteAccess._setupComplete;

    int INativeLogicalDiscardFacts.MovementSequence => _coreCardMovementJournalMovementSequenceAccess._movementSequence;

    long INativeLogicalDiscardFacts.ResolutionSequence => _coreResolutionIdentityBoundaryOperations._resolutionSequence;

    bool INativeLogicalDiscardOrigins.IsOwnedDiscardSource(CardLocation location) => CoreGameEngineProgramDiscardOriginsRuntime.IsOwnedDiscardSource(location);
    bool INativeLogicalDiscardOrigins.IsProgramDiscardOriginReason(CardMoveReason reason) => CoreGameEngineProgramDiscardOriginsRuntime.IsProgramDiscardOriginReason(reason);
    CardLocation? INativeLogicalDiscardOrigins.GetProgramDiscardSource(CardMovementRecord movement) => _coreProgramDiscardOriginsOperations.GetProgramDiscardSource(movement);
    bool INativeLogicalDiscardOrigins.IsEphemeralCardDestroyedMaterial(CardMovementRecord movement) => _coreEphemeralCardMaterialsOperations.IsEphemeralCardDestroyedMaterial(movement);
    NativeLogicalDiscardInvocationDraft? INativeLogicalDiscardAdmissions.Admit(NativeLogicalDiscardInvocationDraft? draft) => _characterZhangFeiWumiaoOperations.AdmitWuZishengNativeDiscard(draft);
    void INativeLogicalDiscardAdmissions.AdmitMaterials(NativeLogicalDiscardInvocationDraft draft) => _characterZhangFeiWumiaoOperations.AdmitWuZishengDiscardMaterials(draft);
    void INativeLogicalDiscardAdmissions.Close(NativeLogicalDiscardInvocationDraft? draft) => _characterZhangFeiWumiaoOperations.CloseWuZishengNativeDiscard(draft);
}
