#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuBuMouMobileEntries
{
    internal bool AdvanceMouLbChildren() => _resolve().AdvanceMouLbChildren();
    internal void AnswerMouLb(PromptChoice choice) => _resolve().AnswerMouLb(choice);
    internal SkillProgramStepOutcome BeginMouLbLiyu(ProgramSkillFrame input) => _resolve().BeginMouLbLiyu(input);
    internal bool CanMouLbBonus(int owner, ProgramSkillWindowContext c) => _resolve().CanMouLbBonus(owner, c);
    internal bool CanMouLbDuel(ProgramSkillFrame f, int target) => _resolve().CanMouLbDuel(f, target);
    internal bool CanMouLbLiyu(int owner, ProgramSkillWindowContext c) => _resolve().CanMouLbLiyu(owner, c);
    internal PromptChoice ChooseMouLbAi(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseMouLbAi(p, f);
    internal void CompleteMouLbDuel(ProgramSkillFrame f, long useId, bool damage) => _resolve().CompleteMouLbDuel(f, useId, damage);
    internal void ContinueMouLb(long id) => _resolve().ContinueMouLb(id);
    internal bool DrainMouLb(ProgramSkillFrame f) => _resolve().DrainMouLb(f);
    internal void ExpireMouLbGrant(ActualTurnCursorReturnedEvent returned) => _resolve().ExpireMouLbGrant(returned);
    internal void FinishMouLb(ProgramSkillFrame f) => _resolve().FinishMouLb(f);
    internal void FinishMouLbTaking(ProgramSkillFrame f) => _resolve().FinishMouLbTaking(f);
    internal void GrantMouLbWushuang(ProgramSkillFrame f) => _resolve().GrantMouLbWushuang(f);
    internal void IssueMouLbDuel(ProgramSkillFrame f, int target) => _resolve().IssueMouLbDuel(f, target);
    internal bool KeepPaidMouLb(ProgramSkillFrame f) => _resolve().KeepPaidMouLb(f);
    internal bool MatchesMouLbDuelParent(ProgramSkillFrame f, ProgramDamageTargetDuelOrigin o) => _resolve().MatchesMouLbDuelParent(f, o);
    internal IReadOnlyList<PromptChoice> MouLbChoices(ProgramSkillFrame f) => _resolve().MouLbChoices(f);
    internal bool MouLbDying() => _resolve().MouLbDying();
    internal bool MouLbFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouLbFirstChild(f, child);
    internal bool MouLbLionChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouLbLionChild(f, child);
    internal bool MouLbNativeEdge(int index) => _resolve().MouLbNativeEdge(index);
    internal ProgramSkillFrame? MouLbObserverRoot() => _resolve().MouLbObserverRoot();
    internal bool MouLbObserverSuffix(int index) => _resolve().MouLbObserverSuffix(index);
    internal long MouLbOriginAnchor(ProgramSkillFrame f) => _resolve().MouLbOriginAnchor(f);
    internal bool MouLbRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().MouLbRecoveryParent(f, recovery);
    internal void PublishMouLb(ProgramSkillFrame f) => _resolve().PublishMouLb(f);
    internal SkillProgramStepOutcome ResolveMouLbBonus(ProgramSkillFrame f) => _resolve().ResolveMouLbBonus(f);
    internal bool ResumeMouLb(long id) => _resolve().ResumeMouLb(id);
    internal void ReturnMouLbDraw(ProgramSkillFrame f, NativeDrawOutcome o) => _resolve().ReturnMouLbDraw(f, o);
    internal bool ReturnMouLbDuelAttack(GameEngine.AttackCompletionReceipt c) => _resolve().ReturnMouLbDuelAttack(c);
    internal bool ReturnMouLbMovement(ProgramSkillFrame f) => _resolve().ReturnMouLbMovement(f);
    internal void ReturnMouLbNullifiedDuel(CardUseFrame use) => _resolve().ReturnMouLbNullifiedDuel(use);
    internal void SealMouLb(long id, long before) => _resolve().SealMouLb(id, before);
    internal bool ValidMouLb(ProgramSkillFrame f) => _resolve().ValidMouLb(f);
    internal bool HasMouLuBuMobile { get => _resolve().HasMouLuBuMobile; }
}
