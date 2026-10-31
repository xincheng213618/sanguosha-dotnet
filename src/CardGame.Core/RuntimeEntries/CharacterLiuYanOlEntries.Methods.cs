#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiuYanOlEntries
{
    internal bool AdvanceLiuYanChildren() => _resolve().AdvanceLiuYanChildren();
    internal global::CardGame.Core.SkillProgramStepOutcome BeginLiuYan(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginLiuYan(supplied, op);
    internal void BeginLiuYanLimu(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.LiuYanLimuUseOrigin o, global::CardGame.Core.Card card) => _resolve().BeginLiuYanLimu(f, o, card);
    internal bool CanActivateLiuYanLimu(global::CardGame.Core.CharacterState owner, string skillId) => _resolve().CanActivateLiuYanLimu(owner, skillId);
    internal void ContinueLiuYan(long id) => _resolve().ContinueLiuYan(id);
    internal void ContinueLiuYanLegacyAlcohol(long id) => _resolve().ContinueLiuYanLegacyAlcohol(id);
    internal bool ContinuePaidLiuYan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidLiuYan(f);
    internal bool DistanceCardUseExemption80HasLiuYanLimuTargetAllowance(global::CardGame.Core.ProgramCardDistanceQuery q) => _resolve().DistanceCardUseExemption80HasLiuYanLimuTargetAllowance(q);
    internal bool DrainLiuYan(long id) => _resolve().DrainLiuYan(id);
    internal void FinishLiuYan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishLiuYan(f);
    internal bool HasLiuYanBeforeDamageObserver(long id) => _resolve().HasLiuYanBeforeDamageObserver(id);
    internal bool HasLiuYanDamageObserver(long id) => _resolve().HasLiuYanDamageObserver(id);
    internal bool HasLiuYanLimuQuotaAllowance(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card? physical, global::System.Collections.Generic.IReadOnlyList<int>? ids, int? excludedId, global::CardGame.Core.EquipmentSlot? excludedSlot) => _resolve().HasLiuYanLimuQuotaAllowance(actor, target, physical, ids, excludedId, excludedSlot);
    internal bool HasLiuYanLimuTargetAllowance(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default) => _resolve().HasLiuYanLimuTargetAllowance(actor, target, excludedEquipmentId, excludedEquipmentSlot);
    internal bool IsLiuYanLegacyAlcoholUse(long id) => _resolve().IsLiuYanLegacyAlcoholUse(id);
    internal bool IsLiuYanLimuSelfDelayedUse(global::CardGame.Core.NullificationWindowFrame p) => _resolve().IsLiuYanLimuSelfDelayedUse(p);
    internal bool IsLiuYanLimuUse(long id) => _resolve().IsLiuYanLimuUse(id);
    internal bool IsLiuYanRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsLiuYanRecoveryParent(f, recovery);
    internal bool LiuYanFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().LiuYanFirstChild(f, child);
    internal bool LiuYanLimuMaterialLegal(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, string skill, string? instance = default) => _resolve().LiuYanLimuMaterialLegal(owner, card, skill, instance);
    internal bool LiuYanLimuSlashQuotaBeforeLimit(global::CardGame.Core.ProgramSlashQuotaQuery query) => _resolve().LiuYanLimuSlashQuotaBeforeLimit(query);
    internal bool LiuYanNoBasicHand(global::CardGame.Core.CharacterState p) => _resolve().LiuYanNoBasicHand(p);
    internal bool LiuYanNormalizeImplicitDrawTwoTargetWindow(global::CardGame.Core.ProgramTargetCardWindowQuery query) => _resolve().LiuYanNormalizeImplicitDrawTwoTargetWindow(query);
    internal global::CardGame.Core.ProgramSkillFrame? LiuYanObserverRoot() => _resolve().LiuYanObserverRoot();
    internal global::CardGame.Core.CardUseFrame? LiuYanTusheUse(int owner, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().LiuYanTusheUse(owner, context);
    internal bool MatchesLiuYanLimu(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.LiuYanLimuUseOrigin o) => _resolve().MatchesLiuYanLimu(f, o);
    internal bool QualifyLiuYanTushe(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().QualifyLiuYanTushe(owner, context);
    internal void RecordLiuYanStart(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RecordLiuYanStart(f);
    internal bool ResumeLiuYan(long id) => _resolve().ResumeLiuYan(id);
    internal void ReturnLiuYanLimu(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnLiuYanLimu(use);
    internal bool TryBeginLiuYanLegacyAlcoholUse(long id, long parentId, long dyingId) => _resolve().TryBeginLiuYanLegacyAlcoholUse(id, parentId, dyingId);
    internal bool TryReturnLiuYanLegacyAlcohol(global::CardGame.Core.ProgramCardTriggerWindowFrame w) => _resolve().TryReturnLiuYanLegacyAlcohol(w);
    internal bool TrySkipUnavailableLiuYanLimuEffect(global::CardGame.Core.NullificationWindowFrame p) => _resolve().TrySkipUnavailableLiuYanLimuEffect(p);
    internal bool ValidLiuYan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidLiuYan(f);
    internal void ValidateLiuYanLegacyAlcohol(global::CardGame.Core.CardUseFrame use) => _resolve().ValidateLiuYanLegacyAlcohol(use);
    internal void ValidateLiuYanLimu(global::CardGame.Core.CardUseFrame use) => _resolve().ValidateLiuYanLimu(use);
    internal bool HasLiuYanCapability { get => _resolve().HasLiuYanCapability; }
}
