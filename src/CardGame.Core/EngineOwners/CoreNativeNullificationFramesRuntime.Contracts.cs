#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNativeNullificationFramesCoreDamageColorDuelProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetTrickRepresentation(long frameId, int cardId, bool requireProcessing = false);
}

internal interface ICoreNativeNullificationFramesCoreNativeSlashTargetDuelOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryGetNativeSlashTargetDuel(global::CardGame.Core.NullificationWindowFrame pending, out global::CardGame.Core.CardUseFrame use, out global::CardGame.Core.NativeSlashTargetDuelPlan plan, out global::CardGame.Core.NativeSlashTargetDuelSlice slice);
    global::CardGame.Core.Card NativeSlashTargetDuelRepresentation(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativeNullificationFramesCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next);
}

internal interface ICoreNativeNullificationFramesResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreNativeNullificationFramesCoreOriginalUseLightningOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidOriginalUseLightningDelayed(global::CardGame.Core.DelayedEffectContinuation d);
}
