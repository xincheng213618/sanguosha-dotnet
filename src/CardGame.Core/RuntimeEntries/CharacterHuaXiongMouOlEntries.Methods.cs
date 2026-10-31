#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuaXiongMouOlEntries
{
    internal IReadOnlyList<MouHxYangweiTicketEvent> ActiveMouHxYangweiTickets(int owner, bool outgoing) => _resolve().ActiveMouHxYangweiTickets(owner, outgoing);
    internal bool AdvanceMouHxBojueChildren() => _resolve().AdvanceMouHxBojueChildren();
    internal void AfterMouHxYangweiChain(GameEngine.IDamageAttempt attack) => _resolve().AfterMouHxYangweiChain(attack);
    internal void AnswerMouHxBojue(PromptChoice choice) => _resolve().AnswerMouHxBojue(choice);
    internal int ApplyMouHxYangwei(GameEngine.IDamageAttempt attack) => _resolve().ApplyMouHxYangwei(attack);
    internal void BeforeMouHxYangweiChain(GameEngine.IDamageAttempt attack) => _resolve().BeforeMouHxYangweiChain(attack);
    internal SkillProgramStepOutcome BeginMouHxBojue(ProgramSkillFrame supplied) => _resolve().BeginMouHxBojue(supplied);
    internal void BindMouHxBojueSlash(IGameEvent fact) => _resolve().BindMouHxBojueSlash(fact);
    internal bool CanMouHxBojueSlash(int actor, int target) => _resolve().CanMouHxBojueSlash(actor, target);
    internal PromptChoice ChooseMouHxBojue(PendingDecision p) => _resolve().ChooseMouHxBojue(p);
    internal void CompleteMouHxBojueItem(ProgramSkillFrame f, bool used) => _resolve().CompleteMouHxBojueItem(f, used);
    internal void CompleteMouHxBojueSlash(long id) => _resolve().CompleteMouHxBojueSlash(id);
    internal void ContinueMouHxBojue(long id) => _resolve().ContinueMouHxBojue(id);
    internal bool ContinuePaidMouHxBojue(ProgramSkillFrame f) => _resolve().ContinuePaidMouHxBojue(f);
    internal bool DrainMouHxBojue(ProgramSkillFrame f) => _resolve().DrainMouHxBojue(f);
    internal void FinishMouHxBojue(ProgramSkillFrame f) => _resolve().FinishMouHxBojue(f);
    internal bool FinishMouHxBojueAttack(NativeOwners.CardAttackHandle attack, scoped out bool paused) => _resolve().FinishMouHxBojueAttack(attack, out paused);
    internal bool IsMouHxBojueDying() => _resolve().IsMouHxBojueDying();
    internal bool IsMouHxBojueRecovery(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsMouHxBojueRecovery(f, recovery);
    internal bool IsMouHxBojueUse(CardUseFrame use) => _resolve().IsMouHxBojueUse(use);
    internal void IssueMouHxBojueSlash(ProgramSkillFrame f) => _resolve().IssueMouHxBojueSlash(f);
    internal int MouHxBojueActor(ProgramSkillFrame f) => _resolve().MouHxBojueActor(f);
    internal IReadOnlyList<PromptChoice> MouHxBojueChoices(ProgramSkillFrame f) => _resolve().MouHxBojueChoices(f);
    internal int MouHxBojueChooser(ProgramSkillFrame f) => _resolve().MouHxBojueChooser(f);
    internal bool MouHxBojueFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouHxBojueFirstChild(f, child);
    internal bool MouHxBojueLionPayment(ProgramSkillFrame f, int seat, CardMoveReason? reason) => _resolve().MouHxBojueLionPayment(f, seat, reason);
    internal bool MouHxBojueNativeEdge(int i) => _resolve().MouHxBojueNativeEdge(i);
    internal bool MouHxBojueObserverEdge(int i) => _resolve().MouHxBojueObserverEdge(i);
    internal ProgramSkillFrame? MouHxBojueObserverRoot() => _resolve().MouHxBojueObserverRoot();
    internal int MouHxBojueOther(ProgramSkillFrame f) => _resolve().MouHxBojueOther(f);
    internal int MouHxYangweiChainCursor(GameEngine.IDamageAttempt a) => _resolve().MouHxYangweiChainCursor(a);
    internal string? MouHxYangweiPublicDescription(int owner) => _resolve().MouHxYangweiPublicDescription(owner);
    internal void ObserveMouHxYangwei(IGameEvent fact) => _resolve().ObserveMouHxYangwei(fact);
    internal void PayMouHxBojueDiscard(ProgramSkillFrame f, PromptChoice choice) => _resolve().PayMouHxBojueDiscard(f, choice);
    internal void PublishMouHxBojue(ProgramSkillFrame supplied) => _resolve().PublishMouHxBojue(supplied);
    internal bool ResumeMouHxBojue(long id) => _resolve().ResumeMouHxBojue(id);
    internal bool ReturnMouHxBojueAttack(GameEngine.AttackCompletionReceipt receipt) => _resolve().ReturnMouHxBojueAttack(receipt);
    internal void ReturnMouHxBojueDraw(ProgramSkillFrame f, NativeDrawOutcome o) => _resolve().ReturnMouHxBojueDraw(f, o);
    internal bool ReturnMouHxBojueMovement(ProgramSkillFrame f) => _resolve().ReturnMouHxBojueMovement(f);
    internal void ReturnMouHxBojueUse(CardUseFrame use) => _resolve().ReturnMouHxBojueUse(use);
    internal void SetMouHxYangweiChainAmount(GameEngine.IDamageAttempt a, int amount) => _resolve().SetMouHxYangweiChainAmount(a, amount);
    internal bool ValidMouHxBojue(ProgramSkillFrame f) => _resolve().ValidMouHxBojue(f);
    internal void ValidateMouHxBojueUse(CardUseFrame use) => _resolve().ValidateMouHxBojueUse(use);
}
