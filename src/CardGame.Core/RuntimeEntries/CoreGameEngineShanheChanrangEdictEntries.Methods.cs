#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheChanrangEdictEntries
{
    internal void AdvanceShanheChanrangRecipient(ProgramSkillFrame frame) => _resolve().AdvanceShanheChanrangRecipient(frame);
    internal PromptChoice AiShanheChanrang(PendingDecision decision, ProgramSkillFrame frame) => _resolve().AiShanheChanrang(decision, frame);
    internal void AnswerShanheChanrang(PromptChoice choice) => _resolve().AnswerShanheChanrang(choice);
    internal SkillProgramStepOutcome BeginShanheChanrang(ProgramSkillFrame supplied) => _resolve().BeginShanheChanrang(supplied);
    internal void CaptureShanheChanrangFirstGains(CardMovementBatchContext batch) => _resolve().CaptureShanheChanrangFirstGains(batch);
    internal void CollectShanheChanrangCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> candidates) => _resolve().CollectShanheChanrangCandidates(batch, candidates);
    internal CardMovementBatchContext CompleteMovementBatchCaptureShanheChanrangFirstGains(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureShanheChanrangFirstGains(batch, completed);
    internal bool ContinuePaidShanheChanrang(ProgramSkillFrame frame) => _resolve().ContinuePaidShanheChanrang(frame);
    internal void ContinueShanheChanrang(long frameId) => _resolve().ContinueShanheChanrang(frameId);
    internal ProgramSkillWindowContext CreateShanheChanrangContext(CardsMovedTriggerWindowFrame window, ProgramTriggerCandidate candidate) => _resolve().CreateShanheChanrangContext(window, candidate);
    internal bool IsIssuedShanheChanrang(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().IsIssuedShanheChanrang(candidate, context);
    internal bool IsShanheChanrangCandidate(ProgramTriggerCandidate candidate) => _resolve().IsShanheChanrangCandidate(candidate);
    internal void PublishShanheChanrang(ProgramSkillFrame frame) => _resolve().PublishShanheChanrang(frame);
    internal bool ResumeShanheChanrang(long frameId) => _resolve().ResumeShanheChanrang(frameId);
    internal Card[] ShanheChanrangCards(int giver) => _resolve().ShanheChanrangCards(giver);
    internal IReadOnlyList<PromptChoice> ShanheChanrangChoices(ProgramSkillFrame frame) => _resolve().ShanheChanrangChoices(frame);
    internal Card? ShanheChanrangEquipment(int owner, string instance) => _resolve().ShanheChanrangEquipment(owner, instance);
    internal int[] ShanheChanrangRecipients(CardMovementBatchContext batch, int owner) => _resolve().ShanheChanrangRecipients(batch, owner);
    internal ProgramSkillWindowContext? TryCreateShanheChanrangContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _resolve().TryCreateShanheChanrangContext(frame, candidate);
    internal bool HasShanheChanrangCapability { get => _resolve().HasShanheChanrangCapability; }
}
