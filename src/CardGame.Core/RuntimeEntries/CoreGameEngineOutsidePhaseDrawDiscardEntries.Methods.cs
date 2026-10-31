#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOutsidePhaseDrawDiscardEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginOutsidePhaseDrawDiscard(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginOutsidePhaseDrawDiscard(supplied, op);
    internal bool CanContinueOutsidePhaseDrawDiscard(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueOutsidePhaseDrawDiscard(f);
    internal bool CanRunOutsidePhaseDrawDiscardTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanRunOutsidePhaseDrawDiscardTrigger(candidate, trigger, context);
    internal global::CardGame.Core.NativeDrawInvocationDraft? CaptureNativeDrawInvocation(int owner, int requested, global::CardGame.Core.CardMoveReason reason, global::CardGame.Core.CardConversionSource? directProducer) => _resolve().CaptureNativeDrawInvocation(owner, requested, reason, directProducer);
    internal global::CardGame.Core.NativeDrawProgramOrigin? CaptureNativeDrawProgramOrigin(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CaptureNativeDrawProgramOrigin(frame);
    internal void CompleteNativeDrawInvocation(global::CardGame.Core.NativeDrawInvocationDraft? draft, global::System.Collections.Generic.IReadOnlyList<int> actualIds) => _resolve().CompleteNativeDrawInvocation(draft, actualIds);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateOutsideDiscardContext(global::CardGame.Core.CardsMovedTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().CreateOutsideDiscardContext(frame, candidate);
    internal bool ExactOutsidePhaseParent(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context, scoped out global::CardGame.Core.CardsMovedTriggerWindowFrame parent) => _resolve().ExactOutsidePhaseParent(candidate, context, out parent);
    internal int[] MatchingOutsideDiscardIndexes(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().MatchingOutsideDiscardIndexes(batch, candidate);
    internal int[] MatchingOutsideDrawIndexes(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().MatchingOutsideDrawIndexes(batch, candidate);
    internal global::CardGame.Core.OutsidePhaseDiscardMaterial[] OutsidePhaseDiscardMaterials(int owner) => _resolve().OutsidePhaseDiscardMaterials(owner);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> OutsidePhaseDrawDiscardChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().OutsidePhaseDrawDiscardChoices(f);
    internal void PublishOutsidePhaseDrawDiscardChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishOutsidePhaseDrawDiscardChoice(f);
    internal void RecordOutsidePhaseIssued(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RecordOutsidePhaseIssued(f);
    internal void ResolveOutsidePhaseDrawDiscardChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveOutsidePhaseDrawDiscardChoice(choice);
    internal bool ResumeOutsidePhaseDrawDiscard(long frameId) => _resolve().ResumeOutsidePhaseDrawDiscard(frameId);
    internal bool ReturnOutsidePhaseDrawDiscardMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnOutsidePhaseDrawDiscardMovement(f);
    internal global::CardGame.Core.PromptChoice SelectAiOutsidePhaseDrawDiscard(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiOutsidePhaseDrawDiscard(decision, f);
    internal int[]? SelectWindowIndexes_MatchingOutsideDiscardIndexes(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().SelectWindowIndexes_MatchingOutsideDiscardIndexes(batch, candidate, trigger);
    internal int[]? SelectWindowIndexes_MatchingOutsideDrawIndexes(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.CardLocation location) => _resolve().SelectWindowIndexes_MatchingOutsideDrawIndexes(batch, candidate, trigger, location);
    internal global::CardGame.Core.ProgramSkillWindowContext? TryCreateOutsideDiscardContext(global::CardGame.Core.CardsMovedTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().TryCreateOutsideDiscardContext(frame, candidate);
    internal bool ValidNativeDrawInvocation(global::CardGame.Core.NativeDrawInvocationProof proof) => _resolve().ValidNativeDrawInvocation(proof);
    internal long OutsidePhaseMovementSequence { get => _resolve().OutsidePhaseMovementSequence; }
    internal bool TracksNativeDrawInvocations { get => _resolve().TracksNativeDrawInvocations; }
}
