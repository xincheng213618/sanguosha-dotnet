#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterMaMiDiOlEntries
{
    internal bool AdvanceMaMiDiSubtree() => _resolve().AdvanceMaMiDiSubtree();
    internal PromptChoice AiMaMiDi(PendingDecision p, ProgramSkillFrame f) => _resolve().AiMaMiDi(p, f);
    internal void AnswerMaMiDi(PromptChoice choice) => _resolve().AnswerMaMiDi(choice);
    internal SkillProgramStepOutcome BeginMaMiDi(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginMaMiDi(supplied, op);
    internal MaMiDiResponseLinkedEvent? BuildMaMiDiResponseLink(ProgramCardTriggerWindowFrame w) => _resolve().BuildMaMiDiResponseLink(w);
    internal bool CanOfferMaMiDi(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferMaMiDi(candidate, trigger, context);
    internal void CaptureMaMiDiResponseLink(ProgramCardTriggerWindowFrame w) => _resolve().CaptureMaMiDiResponseLink(w);
    internal void CollectIssuedBingjieCandidates(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result) => _resolve().CollectIssuedBingjieCandidates(action, window, result);
    internal void ContinueMaMiDi(long id) => _resolve().ContinueMaMiDi(id);
    internal bool ContinuePaidMaMiDi(ProgramSkillFrame f) => _resolve().ContinuePaidMaMiDi(f);
    internal ProgramSkillWindowContext CreateMaMiDiIssuedContext(ProgramCardTriggerWindowFrame w, ProgramCardTriggerCandidate candidate, ProgramSkillWindowContext c) => _resolve().CreateMaMiDiIssuedContext(w, candidate, c);
    internal bool DrainMaMiDi(ProgramSkillFrame f) => _resolve().DrainMaMiDi(f);
    internal bool ExactMaMiDiPlay(ProgramTriggerCandidate candidate, ProgramSkillWindowContext c) => _resolve().ExactMaMiDiPlay(candidate, c);
    internal void FinishMaMiDi(ProgramSkillFrame f) => _resolve().FinishMaMiDi(f);
    internal bool IsIssuedBingjieCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext c) => _resolve().IsIssuedBingjieCandidate(candidate, c);
    internal bool IsMaMiDiRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsMaMiDiRecoveryParent(f, recovery);
    internal bool IsMaMiDiStepMovement(MaMiDiDiscardStep step, CardMovementRecord m) => _resolve().IsMaMiDiStepMovement(step, m);
    internal IReadOnlyList<PromptChoice> MaMiDiChoices(ProgramSkillFrame f) => _resolve().MaMiDiChoices(f);
    internal (BingjieGrantedEvent Grant, int Target) MaMiDiCurrentTarget(MaMiDiReceipt r) => _resolve().MaMiDiCurrentTarget(r);
    internal bool MaMiDiFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MaMiDiFirstChild(f, child);
    internal bool MaMiDiLion(ProgramSkillFrame f) => _resolve().MaMiDiLion(f);
    internal ProgramSkillFrame? MaMiDiObserverRoot() => _resolve().MaMiDiObserverRoot();
    internal void PublishMaMiDi(ProgramSkillFrame f, IReadOnlyList<PromptChoice> choices) => _resolve().PublishMaMiDi(f, choices);
    internal bool ResumeMaMiDi(long id) => _resolve().ResumeMaMiDi(id);
    internal bool ReturnMaMiDiMovement(ProgramSkillFrame f) => _resolve().ReturnMaMiDiMovement(f);
    internal bool TryGetMaMiDiResponse(ProgramTriggerCandidate candidate, ProgramSkillWindowContext c, scoped out ProgramCardTriggerWindowFrame window, scoped out MaMiDiResponseLinkedEvent link) => _resolve().TryGetMaMiDiResponse(candidate, c, out window, out link);
    internal bool HasMaMiDiCapability { get => _resolve().HasMaMiDiCapability; }
}
