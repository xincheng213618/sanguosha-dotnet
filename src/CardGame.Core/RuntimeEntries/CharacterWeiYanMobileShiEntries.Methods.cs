#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWeiYanMobileShiEntries
{
    internal void AnswerMobileShiWeiYan(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerMobileShiWeiYan(selected);
    internal void AssertMobileShiWeiYanPending(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramExecutionPlan plan) => _resolve().AssertMobileShiWeiYanPending(f, plan);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginMobileShiWeiYan(global::CardGame.Core.ProgramSkillFrame input, global::CardGame.Core.SkillProgramEffectOp operation) => _resolve().BeginMobileShiWeiYan(input, operation);
    internal bool CanOfferMobileShiWeiYan(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().CanOfferMobileShiWeiYan(gate);
    internal void CaptureMobileShiWeiYanUse(global::CardGame.Core.IGameEvent payload) => _resolve().CaptureMobileShiWeiYanUse(payload);
    internal void CaptureMobileShiYinzhanApplied(global::CardGame.Core.DamageAppliedEvent damage) => _resolve().CaptureMobileShiYinzhanApplied(damage);
    internal void CommitShiWyMission(global::CardGame.Core.ProgramSkillFrame source, bool success) => _resolve().CommitShiWyMission(source, success);
    internal void ContinueMobileShiWeiYan(long id) => _resolve().ContinueMobileShiWeiYan(id);
    internal bool ContinuePaidMobileShiWeiYan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidMobileShiWeiYan(f);
    internal bool DrainMobileShiWeiYan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainMobileShiWeiYan(f);
    internal bool HasMobileShiZhuangshiDistance(int seat, long? existingUse = default) => _resolve().HasMobileShiZhuangshiDistance(seat, existingUse);
    internal bool HasMobileShiZhuangshiQuota(int seat) => _resolve().HasMobileShiZhuangshiQuota(seat);
    internal bool IsIssuedMobileShiZhuangshiNoCount(long id, int actor) => _resolve().IsIssuedMobileShiZhuangshiNoCount(id, actor);
    internal bool IsMobileShiZhuangshiUnrespondable(long id) => _resolve().IsMobileShiZhuangshiUnrespondable(id);
    internal bool MobileShiKuangguThresholdDrawMatches(global::CardGame.Core.ProgramSkillFrame producer, global::CardGame.Core.NativeDrawInvocationProof proof) => _resolve().MobileShiKuangguThresholdDrawMatches(producer, proof);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MobileShiWeiYanChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MobileShiWeiYanChoices(f);
    internal bool MobileShiWeiYanMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().MobileShiWeiYanMovement(f, e, p);
    internal int MobileShiWeiYanPhaseSlashBonus(int seat) => _resolve().MobileShiWeiYanPhaseSlashBonus(seat);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> MobileShiWeiYanPublicStates(global::CardGame.Core.CharacterState owner, string skillId) => _resolve().MobileShiWeiYanPublicStates(owner, skillId);
    internal int MobileShiYinzhanDamageBonus(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().MobileShiYinzhanDamageBonus(attack);
    internal bool MobileShiZhuangshiCardDistance(global::CardGame.Core.ProgramCardDistanceQuery q) => _resolve().MobileShiZhuangshiCardDistance(q);
    internal global::CardGame.Core.CardConversionSource? MobileShiZhuangshiCounterspellSource(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int> physicalCardIds) => _resolve().MobileShiZhuangshiCounterspellSource(owner, physicalCardIds);
    internal bool MobileShiZhuangshiSlashDistance(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().MobileShiZhuangshiSlashDistance(q);
    internal bool MobileShiZhuangshiSlashQuota(global::CardGame.Core.ProgramSlashQuotaQuery q) => _resolve().MobileShiZhuangshiSlashQuota(q);
    internal void PublishMobileShiWeiYan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishMobileShiWeiYan(f);
    internal bool ResumeMobileShiWeiYan(long id) => _resolve().ResumeMobileShiWeiYan(id);
    internal bool ReturnMobileShiWeiYan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnMobileShiWeiYan(f);
    internal global::CardGame.Core.PromptChoice SelectMobileShiWeiYanAi(global::CardGame.Core.PendingDecision decision) => _resolve().SelectMobileShiWeiYanAi(decision);
    internal int[] ShiWyDiscardable(int seat, bool handOnly) => _resolve().ShiWyDiscardable(seat, handOnly);
    internal bool ShiWyMissionOver(int seat) => _resolve().ShiWyMissionOver(seat);
    internal global::CardGame.Core.MobileShiWeiYanPhaseGrantEvent? ShiWyPhaseGrant(int seat) => _resolve().ShiWyPhaseGrant(seat);
    internal int ShiWyPhaseUses(int seat, int turn, int phase) => _resolve().ShiWyPhaseUses(seat, turn, phase);
}
