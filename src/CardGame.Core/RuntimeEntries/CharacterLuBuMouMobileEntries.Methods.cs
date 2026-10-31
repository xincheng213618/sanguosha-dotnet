#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuBuMouMobileEntries
{
    internal bool AdvanceMouLbChildren() => _resolve().AdvanceMouLbChildren();
    internal void AnswerMouLb(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerMouLb(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginMouLbLiyu(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginMouLbLiyu(input);
    internal bool CanMouLbBonus(int owner, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().CanMouLbBonus(owner, c);
    internal bool CanMouLbDuel(global::CardGame.Core.ProgramSkillFrame f, int target) => _resolve().CanMouLbDuel(f, target);
    internal bool CanMouLbLiyu(int owner, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().CanMouLbLiyu(owner, c);
    internal global::CardGame.Core.PromptChoice ChooseMouLbAi(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseMouLbAi(p, f);
    internal void CompleteMouLbDuel(global::CardGame.Core.ProgramSkillFrame f, long useId, bool damage) => _resolve().CompleteMouLbDuel(f, useId, damage);
    internal void ContinueMouLb(long id) => _resolve().ContinueMouLb(id);
    internal bool DrainMouLb(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainMouLb(f);
    internal void ExpireMouLbGrant(global::CardGame.Core.ActualTurnCursorReturnedEvent returned) => _resolve().ExpireMouLbGrant(returned);
    internal void FinishMouLb(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishMouLb(f);
    internal void FinishMouLbTaking(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishMouLbTaking(f);
    internal void GrantMouLbWushuang(global::CardGame.Core.ProgramSkillFrame f) => _resolve().GrantMouLbWushuang(f);
    internal void IssueMouLbDuel(global::CardGame.Core.ProgramSkillFrame f, int target) => _resolve().IssueMouLbDuel(f, target);
    internal bool KeepPaidMouLb(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepPaidMouLb(f);
    internal bool MatchesMouLbDuelParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramDamageTargetDuelOrigin o) => _resolve().MatchesMouLbDuelParent(f, o);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MouLbChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouLbChoices(f);
    internal bool MouLbDying() => _resolve().MouLbDying();
    internal bool MouLbFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MouLbFirstChild(f, child);
    internal bool MouLbLionChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MouLbLionChild(f, child);
    internal bool MouLbNativeEdge(int index) => _resolve().MouLbNativeEdge(index);
    internal global::CardGame.Core.ProgramSkillFrame? MouLbObserverRoot() => _resolve().MouLbObserverRoot();
    internal bool MouLbObserverSuffix(int index) => _resolve().MouLbObserverSuffix(index);
    internal long MouLbOriginAnchor(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouLbOriginAnchor(f);
    internal bool MouLbRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().MouLbRecoveryParent(f, recovery);
    internal void PublishMouLb(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishMouLb(f);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolveMouLbBonus(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ResolveMouLbBonus(f);
    internal bool ResumeMouLb(long id) => _resolve().ResumeMouLb(id);
    internal void ReturnMouLbDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome o) => _resolve().ReturnMouLbDraw(f, o);
    internal bool ReturnMouLbDuelAttack(global::CardGame.Core.GameEngine.AttackCompletionReceipt c) => _resolve().ReturnMouLbDuelAttack(c);
    internal bool ReturnMouLbMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnMouLbMovement(f);
    internal void ReturnMouLbNullifiedDuel(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnMouLbNullifiedDuel(use);
    internal void SealMouLb(long id, long before) => _resolve().SealMouLb(id, before);
    internal bool ValidMouLb(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidMouLb(f);
    internal bool HasMouLuBuMobile { get => _resolve().HasMouLuBuMobile; }
}
