#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterMaMiDiOlEntries
{
    internal bool AdvanceMaMiDiSubtree() => _resolve().AdvanceMaMiDiSubtree();
    internal global::CardGame.Core.PromptChoice AiMaMiDi(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiMaMiDi(p, f);
    internal void AnswerMaMiDi(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerMaMiDi(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginMaMiDi(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginMaMiDi(supplied, op);
    internal global::CardGame.Core.MaMiDiResponseLinkedEvent? BuildMaMiDiResponseLink(global::CardGame.Core.ProgramCardTriggerWindowFrame w) => _resolve().BuildMaMiDiResponseLink(w);
    internal bool CanOfferMaMiDi(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferMaMiDi(candidate, trigger, context);
    internal void CaptureMaMiDiResponseLink(global::CardGame.Core.ProgramCardTriggerWindowFrame w) => _resolve().CaptureMaMiDiResponseLink(w);
    internal void CollectIssuedBingjieCandidates(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.List<global::CardGame.Core.ProgramCardTriggerCandidate> result) => _resolve().CollectIssuedBingjieCandidates(action, window, result);
    internal void ContinueMaMiDi(long id) => _resolve().ContinueMaMiDi(id);
    internal bool ContinuePaidMaMiDi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidMaMiDi(f);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateMaMiDiIssuedContext(global::CardGame.Core.ProgramCardTriggerWindowFrame w, global::CardGame.Core.ProgramCardTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().CreateMaMiDiIssuedContext(w, candidate, c);
    internal bool DrainMaMiDi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainMaMiDi(f);
    internal bool ExactMaMiDiPlay(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().ExactMaMiDiPlay(candidate, c);
    internal void FinishMaMiDi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishMaMiDi(f);
    internal bool HasMaMiDiBeforeDamageObserver(long id) => _resolve().HasMaMiDiBeforeDamageObserver(id);
    internal bool HasMaMiDiDamageObserver(long id) => _resolve().HasMaMiDiDamageObserver(id);
    internal bool IsIssuedBingjieCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().IsIssuedBingjieCandidate(candidate, c);
    internal bool IsMaMiDiMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsMaMiDiMovement(f, e, p);
    internal bool IsMaMiDiRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsMaMiDiRecoveryParent(f, recovery);
    internal bool IsMaMiDiStepMovement(global::CardGame.Core.MaMiDiDiscardStep step, global::CardGame.Core.CardMovementRecord m) => _resolve().IsMaMiDiStepMovement(step, m);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MaMiDiChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MaMiDiChoices(f);
    internal (global::CardGame.Core.BingjieGrantedEvent Grant, int Target) MaMiDiCurrentTarget(global::CardGame.Core.MaMiDiReceipt r) => _resolve().MaMiDiCurrentTarget(r);
    internal bool MaMiDiFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MaMiDiFirstChild(f, child);
    internal bool MaMiDiLion(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MaMiDiLion(f);
    internal global::CardGame.Core.ProgramSkillFrame? MaMiDiObserverRoot() => _resolve().MaMiDiObserverRoot();
    internal void PublishMaMiDi(global::CardGame.Core.ProgramSkillFrame f, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> choices) => _resolve().PublishMaMiDi(f, choices);
    internal bool ResumeMaMiDi(long id) => _resolve().ResumeMaMiDi(id);
    internal bool ReturnMaMiDiMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnMaMiDiMovement(f);
    internal bool TryGetMaMiDiResponse(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext c, scoped out global::CardGame.Core.ProgramCardTriggerWindowFrame window, scoped out global::CardGame.Core.MaMiDiResponseLinkedEvent link) => _resolve().TryGetMaMiDiResponse(candidate, c, out window, out link);
    internal bool HasMaMiDiCapability { get => _resolve().HasMaMiDiCapability; }
}
