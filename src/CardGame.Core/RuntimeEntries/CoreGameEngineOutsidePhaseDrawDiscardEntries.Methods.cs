#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOutsidePhaseDrawDiscardEntries
{
    internal SkillProgramStepOutcome BeginOutsidePhaseDrawDiscard(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginOutsidePhaseDrawDiscard(supplied, op);
    internal bool CanContinueOutsidePhaseDrawDiscard(ProgramSkillFrame f) => _resolve().CanContinueOutsidePhaseDrawDiscard(f);
    internal bool CanRunOutsidePhaseDrawDiscardTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanRunOutsidePhaseDrawDiscardTrigger(candidate, trigger, context);
    internal NativeDrawInvocationDraft? CaptureNativeDrawInvocation(int owner, int requested, CardMoveReason reason, CardConversionSource? directProducer) => _resolve().CaptureNativeDrawInvocation(owner, requested, reason, directProducer);
    internal NativeDrawProgramOrigin? CaptureNativeDrawProgramOrigin(ProgramSkillFrame frame) => _resolve().CaptureNativeDrawProgramOrigin(frame);
    internal void CompleteNativeDrawInvocation(NativeDrawInvocationDraft? draft, IReadOnlyList<int> actualIds) => _resolve().CompleteNativeDrawInvocation(draft, actualIds);
    internal ProgramSkillWindowContext CreateOutsideDiscardContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _resolve().CreateOutsideDiscardContext(frame, candidate);
    internal bool ExactOutsidePhaseParent(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context, scoped out CardsMovedTriggerWindowFrame parent) => _resolve().ExactOutsidePhaseParent(candidate, context, out parent);
    internal int[] MatchingOutsideDiscardIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate) => _resolve().MatchingOutsideDiscardIndexes(batch, candidate);
    internal int[] MatchingOutsideDrawIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate) => _resolve().MatchingOutsideDrawIndexes(batch, candidate);
    internal OutsidePhaseDiscardMaterial[] OutsidePhaseDiscardMaterials(int owner) => _resolve().OutsidePhaseDiscardMaterials(owner);
    internal IReadOnlyList<PromptChoice> OutsidePhaseDrawDiscardChoices(ProgramSkillFrame f) => _resolve().OutsidePhaseDrawDiscardChoices(f);
    internal void PublishOutsidePhaseDrawDiscardChoice(ProgramSkillFrame f) => _resolve().PublishOutsidePhaseDrawDiscardChoice(f);
    internal void RecordOutsidePhaseIssued(ProgramSkillFrame f) => _resolve().RecordOutsidePhaseIssued(f);
    internal void ResolveOutsidePhaseDrawDiscardChoice(PromptChoice choice) => _resolve().ResolveOutsidePhaseDrawDiscardChoice(choice);
    internal bool ResumeOutsidePhaseDrawDiscard(long frameId) => _resolve().ResumeOutsidePhaseDrawDiscard(frameId);
    internal bool ReturnOutsidePhaseDrawDiscardMovement(ProgramSkillFrame f) => _resolve().ReturnOutsidePhaseDrawDiscardMovement(f);
    internal PromptChoice SelectAiOutsidePhaseDrawDiscard(PendingDecision decision, ProgramSkillFrame f) => _resolve().SelectAiOutsidePhaseDrawDiscard(decision, f);
    internal int[]? SelectWindowIndexes_MatchingOutsideDiscardIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => _resolve().SelectWindowIndexes_MatchingOutsideDiscardIndexes(batch, candidate, trigger);
    internal int[]? SelectWindowIndexes_MatchingOutsideDrawIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, CardLocation location) => _resolve().SelectWindowIndexes_MatchingOutsideDrawIndexes(batch, candidate, trigger, location);
    internal ProgramSkillWindowContext? TryCreateOutsideDiscardContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _resolve().TryCreateOutsideDiscardContext(frame, candidate);
    internal bool ValidNativeDrawInvocation(NativeDrawInvocationProof proof) => _resolve().ValidNativeDrawInvocation(proof);
    internal long OutsidePhaseMovementSequence { get => _resolve().OutsidePhaseMovementSequence; }
    internal bool TracksNativeDrawInvocations { get => _resolve().TracksNativeDrawInvocations; }
}
