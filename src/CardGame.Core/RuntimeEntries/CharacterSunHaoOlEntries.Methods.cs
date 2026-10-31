#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSunHaoOlEntries
{
    internal bool AdvanceSunHaoObserver() => _resolve().AdvanceSunHaoObserver();
    internal PromptChoice AiSunHao(PendingDecision p, ProgramSkillFrame f) => _resolve().AiSunHao(p, f);
    internal void AnswerSunHao(PromptChoice choice) => _resolve().AnswerSunHao(choice);
    internal void ApplySunHaoChainedDamageBonus(GameEngine.IDamageAttempt attack) => _resolve().ApplySunHaoChainedDamageBonus(attack);
    internal void AssertSunHaoDamageReceipts() => _resolve().AssertSunHaoDamageReceipts();
    internal SkillProgramStepOutcome BeginSunHao(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginSunHao(supplied, op);
    internal bool CanOfferSunHao(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext c) => _resolve().CanOfferSunHao(candidate, trigger, c);
    internal void CollectIssuedSunHaoCanshiCandidates(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result) => _resolve().CollectIssuedSunHaoCanshiCandidates(action, window, result);
    internal bool ContinueDeadSunHao(ProgramSkillFrame f) => _resolve().ContinueDeadSunHao(f);
    internal bool ContinueIssuedSunHao(ProgramSkillFrame f) => _resolve().ContinueIssuedSunHao(f);
    internal void ContinueSunHao(long id) => _resolve().ContinueSunHao(id);
    internal IReadOnlyList<SunHaoCanshiGrantedEvent> CurrentSunHaoGrants(int owner) => _resolve().CurrentSunHaoGrants(owner);
    internal bool DistanceCardUseExemption70HasSunHaoCanshiIssuedDistance(ProgramCardDistanceQuery q) => _resolve().DistanceCardUseExemption70HasSunHaoCanshiIssuedDistance(q);
    internal bool DistancePhysicalSlash20HasSunHaoCanshiIssuedDistance(ProgramSlashDistanceQuery q) => _resolve().DistancePhysicalSlash20HasSunHaoCanshiIssuedDistance(q);
    internal bool DrainSunHao(ProgramSkillFrame f) => _resolve().DrainSunHao(f);
    internal void FinishSunHao(ProgramSkillFrame f) => _resolve().FinishSunHao(f);
    internal bool HasSunHaoCanshiIssuedDistance(long? useId, int actorSeat, int targetSeat) => _resolve().HasSunHaoCanshiIssuedDistance(useId, actorSeat, targetSeat);
    internal bool HasSunHaoCanshiResponseObligation(CardActionContext action, ProgramCardContinuation continuation) => _resolve().HasSunHaoCanshiResponseObligation(action, continuation);
    internal bool IsGuimingTreatedWounded(CharacterState player) => _resolve().IsGuimingTreatedWounded(player);
    internal bool IsIssuedSunHaoCanshiCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext c) => _resolve().IsIssuedSunHaoCanshiCandidate(candidate, c);
    internal bool IsSunHaoPaymentMovement(ProgramSkillFrame f, CardMovementRecord m) => _resolve().IsSunHaoPaymentMovement(f, m);
    internal bool IsSunHaoRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsSunHaoRecoveryParent(f, recovery);
    internal void ObserveSunHaoDamage(IGameEvent payload) => _resolve().ObserveSunHaoDamage(payload);
    internal void PrepareSunHaoChainAdvance(GameEngine.IDamageAttempt attack) => _resolve().PrepareSunHaoChainAdvance(attack);
    internal void PublishSunHao(ProgramSkillFrame f, IReadOnlyList<PromptChoice> choices) => _resolve().PublishSunHao(f, choices);
    internal bool ResumeSunHao(long id) => _resolve().ResumeSunHao(id);
    internal bool ReturnSunHaoMovement(ProgramSkillFrame f) => _resolve().ReturnSunHaoMovement(f);
    internal void SetSunHaoChainAmount(GameEngine.IDamageAttempt attack, int amount) => _resolve().SetSunHaoChainAmount(attack, amount);
    internal void SetSunHaoDamageState(long id, SunHaoChouhaiReceipt? r) => _resolve().SetSunHaoDamageState(id, r);
    internal bool SunHaoCanshiUseKind(CardActionContext a, string skillId) => _resolve().SunHaoCanshiUseKind(a, skillId);
    internal IReadOnlyList<PromptChoice> SunHaoChoices(ProgramSkillFrame f) => _resolve().SunHaoChoices(f);
    internal int SunHaoChouhaiDamageBonus(GameEngine.IDamageAttempt attack) => _resolve().SunHaoChouhaiDamageBonus(attack);
    internal int SunHaoDamageCursor(GameEngine.IDamageAttempt a) => _resolve().SunHaoDamageCursor(a);
    internal SunHaoChouhaiReceipt? SunHaoDamageState(long id) => _resolve().SunHaoDamageState(id);
    internal bool SunHaoFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().SunHaoFirstChild(f, child);
    internal bool SunHaoLion(ProgramSkillFrame f) => _resolve().SunHaoLion(f);
    internal ProgramSkillFrame? SunHaoObserverRoot() => _resolve().SunHaoObserverRoot();
    internal bool HasSunHaoCanshiCapability { get => _resolve().HasSunHaoCanshiCapability; }
    internal bool HasSunHaoChouhaiCapability { get => _resolve().HasSunHaoChouhaiCapability; }
    internal bool SunHaoActualTurnOpen { get => _resolve().SunHaoActualTurnOpen; }
}
