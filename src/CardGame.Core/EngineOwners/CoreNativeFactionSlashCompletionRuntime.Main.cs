#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeFactionSlashCompletionRuntime
{
    internal void CompleteProgramFactionCardRequest(NativeOwners.FactionCardRequestHandle pending)
    {
        if (!CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest, pending) || !pending.IsProgramSkillUse || pending.AwaitingProviders || pending.ProgramSkillFrameId is not { } frameId || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame frame || frame.Id != frameId)
            throw new InvalidOperationException("The completed faction-card request lost its program parent.");
        _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest = null;
    }
}
