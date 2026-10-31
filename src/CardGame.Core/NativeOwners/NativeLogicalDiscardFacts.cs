#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class NativeLogicalDiscardFacts(global::CardGame.Core.EngineStates.CoreMatchSetupState matchSetupState, global::CardGame.Core.EngineStates.CoreCardMovementJournalState cardMovementJournalState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreResolutionIdentityBoundaryEntries> getCoreResolutionIdentityBoundaryRuntime) : INativeLogicalDiscardFacts
{
    public bool SetupComplete => matchSetupState.ReferenceToSetupComplete;
    public int MovementSequence => cardMovementJournalState.ReferenceToMovementSequence;
    public long ResolutionSequence => getCoreResolutionIdentityBoundaryRuntime()._resolutionSequence;
}
