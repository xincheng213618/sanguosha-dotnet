#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiuYanOlEntries
{
    internal bool AdvanceLiuYanChildren() => _resolve().AdvanceLiuYanChildren();
    internal SkillProgramStepOutcome BeginLiuYan(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginLiuYan(supplied, op);
    internal void BeginLiuYanLimu(ProgramSkillFrame f, LiuYanLimuUseOrigin o, Card card) => _resolve().BeginLiuYanLimu(f, o, card);
    internal bool CanActivateLiuYanLimu(CharacterState owner, string skillId) => _resolve().CanActivateLiuYanLimu(owner, skillId);
    internal void ContinueLiuYan(long id) => _resolve().ContinueLiuYan(id);
    internal void ContinueLiuYanLegacyAlcohol(long id) => _resolve().ContinueLiuYanLegacyAlcohol(id);
    internal bool ContinuePaidLiuYan(ProgramSkillFrame f) => _resolve().ContinuePaidLiuYan(f);
    internal bool DistanceCardUseExemption80HasLiuYanLimuTargetAllowance(ProgramCardDistanceQuery q) => _resolve().DistanceCardUseExemption80HasLiuYanLimuTargetAllowance(q);
    internal bool DrainLiuYan(long id) => _resolve().DrainLiuYan(id);
    internal void FinishLiuYan(ProgramSkillFrame f) => _resolve().FinishLiuYan(f);
    internal bool HasLiuYanLimuQuotaAllowance(CharacterState actor, CharacterState target, Card? physical, IReadOnlyList<int>? ids, int? excludedId, EquipmentSlot? excludedSlot) => _resolve().HasLiuYanLimuQuotaAllowance(actor, target, physical, ids, excludedId, excludedSlot);
    internal bool HasLiuYanLimuTargetAllowance(CharacterState actor, CharacterState target, int? excludedEquipmentId = default, EquipmentSlot? excludedEquipmentSlot = default) => _resolve().HasLiuYanLimuTargetAllowance(actor, target, excludedEquipmentId, excludedEquipmentSlot);
    internal bool IsLiuYanLegacyAlcoholUse(long id) => _resolve().IsLiuYanLegacyAlcoholUse(id);
    internal bool IsLiuYanLimuSelfDelayedUse(NullificationWindowFrame p) => _resolve().IsLiuYanLimuSelfDelayedUse(p);
    internal bool IsLiuYanLimuUse(long id) => _resolve().IsLiuYanLimuUse(id);
    internal bool IsLiuYanRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsLiuYanRecoveryParent(f, recovery);
    internal bool LiuYanFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().LiuYanFirstChild(f, child);
    internal bool LiuYanLimuMaterialLegal(CharacterState owner, Card card, string skill, string? instance = default) => _resolve().LiuYanLimuMaterialLegal(owner, card, skill, instance);
    internal bool LiuYanLimuSlashQuotaBeforeLimit(ProgramSlashQuotaQuery query) => _resolve().LiuYanLimuSlashQuotaBeforeLimit(query);
    internal bool LiuYanNoBasicHand(CharacterState p) => _resolve().LiuYanNoBasicHand(p);
    internal bool LiuYanNormalizeImplicitDrawTwoTargetWindow(ProgramTargetCardWindowQuery query) => _resolve().LiuYanNormalizeImplicitDrawTwoTargetWindow(query);
    internal ProgramSkillFrame? LiuYanObserverRoot() => _resolve().LiuYanObserverRoot();
    internal CardUseFrame? LiuYanTusheUse(int owner, ProgramSkillWindowContext context) => _resolve().LiuYanTusheUse(owner, context);
    internal bool MatchesLiuYanLimu(ProgramSkillFrame f, LiuYanLimuUseOrigin o) => _resolve().MatchesLiuYanLimu(f, o);
    internal bool QualifyLiuYanTushe(CharacterState owner, ProgramSkillWindowContext context) => _resolve().QualifyLiuYanTushe(owner, context);
    internal void RecordLiuYanStart(ProgramSkillFrame f) => _resolve().RecordLiuYanStart(f);
    internal bool ResumeLiuYan(long id) => _resolve().ResumeLiuYan(id);
    internal void ReturnLiuYanLimu(CardUseFrame use) => _resolve().ReturnLiuYanLimu(use);
    internal bool TryBeginLiuYanLegacyAlcoholUse(long id, long parentId, long dyingId) => _resolve().TryBeginLiuYanLegacyAlcoholUse(id, parentId, dyingId);
    internal bool TryReturnLiuYanLegacyAlcohol(ProgramCardTriggerWindowFrame w) => _resolve().TryReturnLiuYanLegacyAlcohol(w);
    internal bool TrySkipUnavailableLiuYanLimuEffect(NullificationWindowFrame p) => _resolve().TrySkipUnavailableLiuYanLimuEffect(p);
    internal bool ValidLiuYan(ProgramSkillFrame f) => _resolve().ValidLiuYan(f);
    internal void ValidateLiuYanLegacyAlcohol(CardUseFrame use) => _resolve().ValidateLiuYanLegacyAlcohol(use);
    internal void ValidateLiuYanLimu(CardUseFrame use) => _resolve().ValidateLiuYanLimu(use);
    internal bool HasLiuYanCapability { get => _resolve().HasLiuYanCapability; }
}
