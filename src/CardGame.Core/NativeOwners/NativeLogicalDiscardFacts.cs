#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class NativeLogicalDiscardFacts(EngineStates.CoreMatchSetupState matchSetupState, EngineStates.CoreCardMovementJournalState cardMovementJournalState, Func<RuntimeEntries.CoreResolutionIdentityBoundaryEntries> getCoreResolutionIdentityBoundaryRuntime) : INativeLogicalDiscardFacts
{
    public bool SetupComplete => matchSetupState.ReferenceToSetupComplete;
    public int MovementSequence => cardMovementJournalState.ReferenceToMovementSequence;
    public long ResolutionSequence => getCoreResolutionIdentityBoundaryRuntime()._resolutionSequence;
}
