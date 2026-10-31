#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterPeiYuanShaoOlEntries
{
    internal bool AdvanceFulueSubtree() => _resolve().AdvanceFulueSubtree();
    internal void AnswerFulue(PromptChoice choice) => _resolve().AnswerFulue(choice);
    internal SkillProgramStepOutcome BeginFulue(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginFulue(supplied, op);
    internal bool CanRunIssuedFulueCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().CanRunIssuedFulueCandidate(c, context);
    internal PromptChoice ChooseFulueAi(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseFulueAi(p, f);
    internal void CollectIssuedFulueCandidates(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result) => _resolve().CollectIssuedFulueCandidates(action, window, result);
    internal void ContinueFulue(long id) => _resolve().ContinueFulue(id);
    internal bool ContinueIssuedFulue(ProgramSkillFrame f) => _resolve().ContinueIssuedFulue(f);
    internal bool? DecideIssuedTrigger590(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().DecideIssuedTrigger590(candidate, context);
    internal void ExecuteFulueOption(ProgramSkillFrame f, FulueOption option) => _resolve().ExecuteFulueOption(f, option);
    internal void FinishFulue(ProgramSkillFrame f) => _resolve().FinishFulue(f);
    internal IReadOnlyList<PromptChoice> FulueChoices(ProgramSkillFrame f) => _resolve().FulueChoices(f);
    internal CardUseFrame? FulueContextUse(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().FulueContextUse(c, context);
    internal int FulueDamageBonus(GameEngine.IDamageAttempt attack) => _resolve().FulueDamageBonus(attack);
    internal bool FulueFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().FulueFirstChild(f, child);
    internal bool FulueLionProof(ProgramSkillFrame f, int seat, CardMoveReason? reason) => _resolve().FulueLionProof(f, seat, reason);
    internal ProgramSkillFrame? FulueObserverRoot() => _resolve().FulueObserverRoot();
    internal IReadOnlyList<PromptChoice> FulueObtainChoices(long frame, int user, int target) => _resolve().FulueObtainChoices(frame, user, target);
    internal bool FulueOptionAvailable(CardConversionSource source, int turn, FulueOption option) => _resolve().FulueOptionAvailable(source, turn, option);
    internal bool FulueOriginalCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().FulueOriginalCandidate(c, context);
    internal FulueUseEntry? FulueOriginalEntry(CardUseFrame use, int owner, string skill, string instance) => _resolve().FulueOriginalEntry(use, owner, skill, instance);
    internal bool FulueUseProof(CardUseFrame use) => _resolve().FulueUseProof(use);
    internal bool FulueZeroDamage(CardUseFrame use) => _resolve().FulueZeroDamage(use);
    internal bool HasFulueObtainableCard(int user, int target) => _resolve().HasFulueObtainableCard(user, target);
    internal bool HasIssuedFulueRequest(ProgramSkillFrame f) => _resolve().HasIssuedFulueRequest(f);
    internal bool IsFulueRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsFulueRecoveryParent(f, recovery);
    internal bool IsIssuedFulueCandidate(ProgramTriggerCandidate c) => _resolve().IsIssuedFulueCandidate(c);
    internal void IssueFulueCounterUse(long useId, CardActionContext action) => _resolve().IssueFulueCounterUse(useId, action);
    internal void PublishFulue(ProgramSkillFrame f) => _resolve().PublishFulue(f);
    internal bool QualifyFulue(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => _resolve().QualifyFulue(c, t, context);
    internal bool ResumeFulue(long id) => _resolve().ResumeFulue(id);
    internal bool ReturnFulueMovement(ProgramSkillFrame f) => _resolve().ReturnFulueMovement(f);
    internal bool TryBeginFulueAssistedSlash(ProgramSkillFrame frame, int actorSeat, string bind, scoped out SkillProgramStepOutcome outcome) => _resolve().TryBeginFulueAssistedSlash(frame, actorSeat, bind, out outcome);
    internal bool ValidFulueCounterInvoice(CardUseFrame use, FulueCounterInvoice r) => _resolve().ValidFulueCounterInvoice(use, r);
}
