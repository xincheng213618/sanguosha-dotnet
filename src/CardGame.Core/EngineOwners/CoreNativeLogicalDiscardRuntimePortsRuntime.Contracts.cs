#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNativeLogicalDiscardRuntimePortsCoreResolutionIdentityBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long _resolutionSequence { get; set; }
}

internal interface ICoreNativeLogicalDiscardRuntimePortsCoreEphemeralCardMaterialsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsEphemeralCardDestroyedMaterial(global::CardGame.Core.CardMovementRecord movement);
}

internal interface ICoreNativeLogicalDiscardRuntimePortsCharacterZhangFeiWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeLogicalDiscardInvocationDraft? AdmitWuZishengNativeDiscard(global::CardGame.Core.NativeLogicalDiscardInvocationDraft? draft);
    void AdmitWuZishengDiscardMaterials(global::CardGame.Core.NativeLogicalDiscardInvocationDraft draft);
    void CloseWuZishengNativeDiscard(global::CardGame.Core.NativeLogicalDiscardInvocationDraft? draft);
}

internal interface ICoreNativeLogicalDiscardRuntimePortsCoreProgramDiscardOriginsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardLocation? GetProgramDiscardSource(global::CardGame.Core.CardMovementRecord movement);
}
