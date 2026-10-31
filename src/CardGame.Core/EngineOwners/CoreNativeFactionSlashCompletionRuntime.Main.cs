#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeFactionSlashCompletionRuntime
{
    internal void CompleteProgramFactionCardRequest(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending)
    {
        if (!global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest, pending) || !pending.IsProgramSkillUse || pending.AwaitingProviders || pending.ProgramSkillFrameId is not { } frameId || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame frame || frame.Id != frameId)
            throw new InvalidOperationException("The completed faction-card request lost its program parent.");
        _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest = null;
    }
}
