#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterPeiYuanShaoOlEntries
{
    internal bool AdvanceFulueSubtree() => _resolve().AdvanceFulueSubtree();
    internal void AnswerFulue(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerFulue(choice);
    internal void AssertFulue(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertFulue(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginFulue(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginFulue(supplied, op);
    internal bool CanRunIssuedFulueCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanRunIssuedFulueCandidate(c, context);
    internal global::CardGame.Core.PromptChoice ChooseFulueAi(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseFulueAi(p, f);
    internal void CollectIssuedFulueCandidates(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.List<global::CardGame.Core.ProgramCardTriggerCandidate> result) => _resolve().CollectIssuedFulueCandidates(action, window, result);
    internal void ContinueFulue(long id) => _resolve().ContinueFulue(id);
    internal bool ContinueIssuedFulue(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueIssuedFulue(f);
    internal bool? DecideIssuedTrigger590(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().DecideIssuedTrigger590(candidate, context);
    internal void ExecuteFulueOption(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.FulueOption option) => _resolve().ExecuteFulueOption(f, option);
    internal void FinishFulue(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishFulue(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> FulueChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FulueChoices(f);
    internal global::CardGame.Core.CardUseFrame? FulueContextUse(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().FulueContextUse(c, context);
    internal int FulueDamageBonus(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().FulueDamageBonus(attack);
    internal bool FulueFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().FulueFirstChild(f, child);
    internal bool FulueLionProof(global::CardGame.Core.ProgramSkillFrame f, int seat, global::CardGame.Core.CardMoveReason? reason) => _resolve().FulueLionProof(f, seat, reason);
    internal global::CardGame.Core.ProgramSkillFrame? FulueObserverRoot() => _resolve().FulueObserverRoot();
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> FulueObtainChoices(long frame, int user, int target) => _resolve().FulueObtainChoices(frame, user, target);
    internal bool FulueOptionAvailable(global::CardGame.Core.CardConversionSource source, int turn, global::CardGame.Core.FulueOption option) => _resolve().FulueOptionAvailable(source, turn, option);
    internal bool FulueOriginalCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().FulueOriginalCandidate(c, context);
    internal global::CardGame.Core.FulueUseEntry? FulueOriginalEntry(global::CardGame.Core.CardUseFrame use, int owner, string skill, string instance) => _resolve().FulueOriginalEntry(use, owner, skill, instance);
    internal bool FulueUseProof(global::CardGame.Core.CardUseFrame use) => _resolve().FulueUseProof(use);
    internal bool FulueZeroDamage(global::CardGame.Core.CardUseFrame use) => _resolve().FulueZeroDamage(use);
    internal bool HasFulueBeforeDamageObserver(long id) => _resolve().HasFulueBeforeDamageObserver(id);
    internal bool HasFulueDamageObserver(long id) => _resolve().HasFulueDamageObserver(id);
    internal bool HasFulueObtainableCard(int user, int target) => _resolve().HasFulueObtainableCard(user, target);
    internal bool HasIssuedFulueRequest(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HasIssuedFulueRequest(f);
    internal bool IsFulueDying() => _resolve().IsFulueDying();
    internal bool IsFulueMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsFulueMovement(f, effect, pending);
    internal bool IsFulueRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsFulueRecoveryParent(f, recovery);
    internal bool IsIssuedFulueCandidate(global::CardGame.Core.ProgramTriggerCandidate c) => _resolve().IsIssuedFulueCandidate(c);
    internal void IssueFulueCounterUse(long useId, global::CardGame.Core.CardActionContext action) => _resolve().IssueFulueCounterUse(useId, action);
    internal void PublishFulue(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishFulue(f);
    internal bool QualifyFulue(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().QualifyFulue(c, t, context);
    internal bool ResumeFulue(long id) => _resolve().ResumeFulue(id);
    internal bool ReturnFulueMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnFulueMovement(f);
    internal bool TryBeginFulueAssistedSlash(global::CardGame.Core.ProgramSkillFrame frame, int actorSeat, string bind, scoped out global::CardGame.Core.SkillProgramStepOutcome outcome) => _resolve().TryBeginFulueAssistedSlash(frame, actorSeat, bind, out outcome);
    internal bool ValidFulueCounterInvoice(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.FulueCounterInvoice r) => _resolve().ValidFulueCounterInvoice(use, r);
}
