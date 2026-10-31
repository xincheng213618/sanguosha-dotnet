#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSunHaoOlEntries
{
    internal bool AdvanceSunHaoObserver() => _resolve().AdvanceSunHaoObserver();
    internal global::CardGame.Core.PromptChoice AiSunHao(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiSunHao(p, f);
    internal void AnswerSunHao(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerSunHao(choice);
    internal void ApplySunHaoChainedDamageBonus(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().ApplySunHaoChainedDamageBonus(attack);
    internal void AssertSunHao(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertSunHao(f);
    internal void AssertSunHaoDamageReceipts() => _resolve().AssertSunHaoDamageReceipts();
    internal global::CardGame.Core.SkillProgramStepOutcome BeginSunHao(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginSunHao(supplied, op);
    internal bool CanOfferSunHao(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().CanOfferSunHao(candidate, trigger, c);
    internal void CollectIssuedSunHaoCanshiCandidates(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.List<global::CardGame.Core.ProgramCardTriggerCandidate> result) => _resolve().CollectIssuedSunHaoCanshiCandidates(action, window, result);
    internal bool ContinueDeadSunHao(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueDeadSunHao(f);
    internal bool ContinueIssuedSunHao(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueIssuedSunHao(f);
    internal void ContinueSunHao(long id) => _resolve().ContinueSunHao(id);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SunHaoCanshiGrantedEvent> CurrentSunHaoGrants(int owner) => _resolve().CurrentSunHaoGrants(owner);
    internal bool DistanceCardUseExemption70HasSunHaoCanshiIssuedDistance(global::CardGame.Core.ProgramCardDistanceQuery q) => _resolve().DistanceCardUseExemption70HasSunHaoCanshiIssuedDistance(q);
    internal bool DistancePhysicalSlash20HasSunHaoCanshiIssuedDistance(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().DistancePhysicalSlash20HasSunHaoCanshiIssuedDistance(q);
    internal bool DrainSunHao(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainSunHao(f);
    internal void FinishSunHao(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishSunHao(f);
    internal bool HasSunHaoBeforeDamageObserver(long id) => _resolve().HasSunHaoBeforeDamageObserver(id);
    internal bool HasSunHaoCanshiIssuedDistance(long? useId, int actorSeat, int targetSeat) => _resolve().HasSunHaoCanshiIssuedDistance(useId, actorSeat, targetSeat);
    internal bool HasSunHaoCanshiResponseObligation(global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation) => _resolve().HasSunHaoCanshiResponseObligation(action, continuation);
    internal bool HasSunHaoDamageObserver(long id) => _resolve().HasSunHaoDamageObserver(id);
    internal bool IsGuimingTreatedWounded(global::CardGame.Core.CharacterState player) => _resolve().IsGuimingTreatedWounded(player);
    internal bool IsIssuedSunHaoCanshiCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().IsIssuedSunHaoCanshiCandidate(candidate, c);
    internal bool IsSunHaoDying() => _resolve().IsSunHaoDying();
    internal bool IsSunHaoMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsSunHaoMovement(f, e, p);
    internal bool IsSunHaoPaymentMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardMovementRecord m) => _resolve().IsSunHaoPaymentMovement(f, m);
    internal bool IsSunHaoRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsSunHaoRecoveryParent(f, recovery);
    internal void ObserveSunHaoDamage(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveSunHaoDamage(payload);
    internal void PrepareSunHaoChainAdvance(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().PrepareSunHaoChainAdvance(attack);
    internal void PublishSunHao(global::CardGame.Core.ProgramSkillFrame f, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> choices) => _resolve().PublishSunHao(f, choices);
    internal bool ResumeSunHao(long id) => _resolve().ResumeSunHao(id);
    internal bool ReturnSunHaoMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnSunHaoMovement(f);
    internal void SetSunHaoChainAmount(global::CardGame.Core.GameEngine.IDamageAttempt attack, int amount) => _resolve().SetSunHaoChainAmount(attack, amount);
    internal void SetSunHaoDamageState(long id, global::CardGame.Core.SunHaoChouhaiReceipt? r) => _resolve().SetSunHaoDamageState(id, r);
    internal bool SunHaoCanshiUseKind(global::CardGame.Core.CardActionContext a, string skillId) => _resolve().SunHaoCanshiUseKind(a, skillId);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> SunHaoChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SunHaoChoices(f);
    internal int SunHaoChouhaiDamageBonus(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().SunHaoChouhaiDamageBonus(attack);
    internal int SunHaoDamageCursor(global::CardGame.Core.GameEngine.IDamageAttempt a) => _resolve().SunHaoDamageCursor(a);
    internal global::CardGame.Core.SunHaoChouhaiReceipt? SunHaoDamageState(long id) => _resolve().SunHaoDamageState(id);
    internal bool SunHaoFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().SunHaoFirstChild(f, child);
    internal bool SunHaoLion(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SunHaoLion(f);
    internal global::CardGame.Core.ProgramSkillFrame? SunHaoObserverRoot() => _resolve().SunHaoObserverRoot();
    internal bool HasSunHaoCanshiCapability { get => _resolve().HasSunHaoCanshiCapability; }
    internal bool HasSunHaoChouhaiCapability { get => _resolve().HasSunHaoChouhaiCapability; }
    internal bool SunHaoActualTurnOpen { get => _resolve().SunHaoActualTurnOpen; }
}
