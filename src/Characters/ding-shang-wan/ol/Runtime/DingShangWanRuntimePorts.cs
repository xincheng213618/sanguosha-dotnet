namespace CardGame.Core;

// Startup draw origins consume only this read-only receipt proof. Keeping its
// owner separate from the draw producer avoids constructing an invoice cycle.
internal interface IDingShangWanFrameProofs : IProgramSelectionQuery
{
    void AssertDingShangWan(ProgramSkillFrame frame);
}

internal interface IDingShangWanRuntimeQueries : IDingShangWanFrameProofs
{
    CardConversionSource DingShangWanSource(ProgramSkillFrame frame);
    bool CanOfferDingShangWan(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context);
    int? DingShangWanCounterpart(int owner, ProgramSkillWindowContext context);
    ProgramSkillFrame? DingShangWanObserverRoot();
}

// Existing native owners expose their current facts and individual adjacency
// proofs. Candidate qualification and the complete observer traversal live in
// DingShangWanFrameProofRuntime, with no copied frame, journal or payment state.
internal interface IDingShangWanNativeFacts : IProgramSelectionQuery
{
    Winner Winner { get; }
    long MovementSequence { get; }
    bool CurrentDamageHasSource { get; }
    DyingFrame? ActiveDying { get; }
    bool ValidSeat(int seat);
    bool HasSkillInstance(int seat, string skill, string instance);
    SkillProgramTrigger Trigger(ProgramSkillFrame frame);
    SkillProgramEffect? PausedEffect(ProgramSkillFrame frame);
    bool IsActualAfterTurnEndedParent(DeferredTurnEndFrame frame);
    ProgramTriggerCandidate AfterTurnEndedCandidate(DeferredTurnEndFrame frame);
    bool ValidResponseCompletionWindow(ProgramCardTriggerWindowFrame frame);
    bool RecoveryReplacementFrameRidesOn(ResolutionFrame child, ResolutionFrame parent);
    bool MovementObserverEdge(int child);
    bool DyingExitEdge(int child);
    bool SuspendedOrPaidRescueSuffix(int child, DyingFrame dying);
}

internal interface IDingShangWanNativeOperations : IProgramRuntimeOperations
{
    void Give(long frameId, int giver, int recipient, IReadOnlyList<int> cards, CardMoveReason reason);
}

public sealed partial class GameEngine
{
}
