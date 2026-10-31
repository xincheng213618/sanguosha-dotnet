#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheChanrangEdictEntries
{
    internal void AdvanceShanheChanrangRecipient(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().AdvanceShanheChanrangRecipient(frame);
    internal global::CardGame.Core.PromptChoice AiShanheChanrang(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().AiShanheChanrang(decision, frame);
    internal void AnswerShanheChanrang(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerShanheChanrang(choice);
    internal void AssertShanheChanrang(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().AssertShanheChanrang(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShanheChanrang(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginShanheChanrang(supplied);
    internal void CaptureShanheChanrangFirstGains(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CaptureShanheChanrangFirstGains(batch);
    internal void CollectShanheChanrangCandidates(global::CardGame.Core.CardMovementBatchContext batch, global::System.Collections.Generic.List<global::CardGame.Core.ProgramTriggerCandidate> candidates) => _resolve().CollectShanheChanrangCandidates(batch, candidates);
    internal global::CardGame.Core.CardMovementBatchContext CompleteMovementBatchCaptureShanheChanrangFirstGains(global::CardGame.Core.CardMovementBatchBuilder batch, global::CardGame.Core.CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureShanheChanrangFirstGains(batch, completed);
    internal bool ContinuePaidShanheChanrang(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ContinuePaidShanheChanrang(frame);
    internal void ContinueShanheChanrang(long frameId) => _resolve().ContinueShanheChanrang(frameId);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateShanheChanrangContext(global::CardGame.Core.CardsMovedTriggerWindowFrame window, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().CreateShanheChanrangContext(window, candidate);
    internal bool IsIssuedShanheChanrang(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsIssuedShanheChanrang(candidate, context);
    internal bool IsShanheChanrangCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().IsShanheChanrangCandidate(candidate);
    internal void PublishShanheChanrang(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishShanheChanrang(frame);
    internal bool ResumeShanheChanrang(long frameId) => _resolve().ResumeShanheChanrang(frameId);
    internal global::CardGame.Core.Card[] ShanheChanrangCards(int giver) => _resolve().ShanheChanrangCards(giver);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ShanheChanrangChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ShanheChanrangChoices(frame);
    internal global::CardGame.Core.Card? ShanheChanrangEquipment(int owner, string instance) => _resolve().ShanheChanrangEquipment(owner, instance);
    internal int[] ShanheChanrangRecipients(global::CardGame.Core.CardMovementBatchContext batch, int owner) => _resolve().ShanheChanrangRecipients(batch, owner);
    internal global::CardGame.Core.ProgramSkillWindowContext? TryCreateShanheChanrangContext(global::CardGame.Core.CardsMovedTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().TryCreateShanheChanrangContext(frame, candidate);
    internal bool HasShanheChanrangCapability { get => _resolve().HasShanheChanrangCapability; }
}
