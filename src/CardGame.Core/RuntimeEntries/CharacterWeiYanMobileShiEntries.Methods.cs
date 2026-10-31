#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWeiYanMobileShiEntries
{
    internal void AnswerMobileShiWeiYan(PromptChoice selected) => _resolve().AnswerMobileShiWeiYan(selected);
    internal SkillProgramStepOutcome BeginMobileShiWeiYan(ProgramSkillFrame input, SkillProgramEffectOp operation) => _resolve().BeginMobileShiWeiYan(input, operation);
    internal bool CanOfferMobileShiWeiYan(ProgramTriggerLegalityContext gate) => _resolve().CanOfferMobileShiWeiYan(gate);
    internal void CaptureMobileShiWeiYanUse(IGameEvent payload) => _resolve().CaptureMobileShiWeiYanUse(payload);
    internal void CaptureMobileShiYinzhanApplied(DamageAppliedEvent damage) => _resolve().CaptureMobileShiYinzhanApplied(damage);
    internal void CommitShiWyMission(ProgramSkillFrame source, bool success) => _resolve().CommitShiWyMission(source, success);
    internal void ContinueMobileShiWeiYan(long id) => _resolve().ContinueMobileShiWeiYan(id);
    internal bool ContinuePaidMobileShiWeiYan(ProgramSkillFrame f) => _resolve().ContinuePaidMobileShiWeiYan(f);
    internal bool DrainMobileShiWeiYan(ProgramSkillFrame f) => _resolve().DrainMobileShiWeiYan(f);
    internal bool HasMobileShiZhuangshiDistance(int seat, long? existingUse = default) => _resolve().HasMobileShiZhuangshiDistance(seat, existingUse);
    internal bool HasMobileShiZhuangshiQuota(int seat) => _resolve().HasMobileShiZhuangshiQuota(seat);
    internal bool IsIssuedMobileShiZhuangshiNoCount(long id, int actor) => _resolve().IsIssuedMobileShiZhuangshiNoCount(id, actor);
    internal bool IsMobileShiZhuangshiUnrespondable(long id) => _resolve().IsMobileShiZhuangshiUnrespondable(id);
    internal bool MobileShiKuangguThresholdDrawMatches(ProgramSkillFrame producer, NativeDrawInvocationProof proof) => _resolve().MobileShiKuangguThresholdDrawMatches(producer, proof);
    internal IReadOnlyList<PromptChoice> MobileShiWeiYanChoices(ProgramSkillFrame f) => _resolve().MobileShiWeiYanChoices(f);
    internal int MobileShiWeiYanPhaseSlashBonus(int seat) => _resolve().MobileShiWeiYanPhaseSlashBonus(seat);
    internal IReadOnlyList<ProgramPublicRuleStateSnapshot> MobileShiWeiYanPublicStates(CharacterState owner, string skillId) => _resolve().MobileShiWeiYanPublicStates(owner, skillId);
    internal int MobileShiYinzhanDamageBonus(GameEngine.IDamageAttempt attack) => _resolve().MobileShiYinzhanDamageBonus(attack);
    internal bool MobileShiZhuangshiCardDistance(ProgramCardDistanceQuery q) => _resolve().MobileShiZhuangshiCardDistance(q);
    internal CardConversionSource? MobileShiZhuangshiCounterspellSource(CharacterState owner, IReadOnlyList<int> physicalCardIds) => _resolve().MobileShiZhuangshiCounterspellSource(owner, physicalCardIds);
    internal bool MobileShiZhuangshiSlashDistance(ProgramSlashDistanceQuery q) => _resolve().MobileShiZhuangshiSlashDistance(q);
    internal bool MobileShiZhuangshiSlashQuota(ProgramSlashQuotaQuery q) => _resolve().MobileShiZhuangshiSlashQuota(q);
    internal void PublishMobileShiWeiYan(ProgramSkillFrame f) => _resolve().PublishMobileShiWeiYan(f);
    internal bool ResumeMobileShiWeiYan(long id) => _resolve().ResumeMobileShiWeiYan(id);
    internal bool ReturnMobileShiWeiYan(ProgramSkillFrame f) => _resolve().ReturnMobileShiWeiYan(f);
    internal PromptChoice SelectMobileShiWeiYanAi(PendingDecision decision) => _resolve().SelectMobileShiWeiYanAi(decision);
    internal int[] ShiWyDiscardable(int seat, bool handOnly) => _resolve().ShiWyDiscardable(seat, handOnly);
    internal bool ShiWyMissionOver(int seat) => _resolve().ShiWyMissionOver(seat);
    internal MobileShiWeiYanPhaseGrantEvent? ShiWyPhaseGrant(int seat) => _resolve().ShiWyPhaseGrant(seat);
    internal int ShiWyPhaseUses(int seat, int turn, int phase) => _resolve().ShiWyPhaseUses(seat, turn, phase);
}
