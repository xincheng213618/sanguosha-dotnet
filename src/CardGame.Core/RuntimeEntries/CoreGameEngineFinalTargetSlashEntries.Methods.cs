#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFinalTargetSlashEntries
{
    internal bool CanOfferFinalTargetSlash(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext window) => _resolve().CanOfferFinalTargetSlash(candidate, trigger, window);
    internal bool DistanceBorrowedPaidWeaponSlash50HasRankSlashRange(ProgramSlashDistanceQuery q) => _resolve().DistanceBorrowedPaidWeaponSlash50HasRankSlashRange(q);
    internal bool DistanceBorrowedPhysicalSlash90IsWithinSpecificSlashRange(ProgramSlashDistanceQuery q) => _resolve().DistanceBorrowedPhysicalSlash90IsWithinSpecificSlashRange(q);
    internal bool DistancePhysicalSlash260IsWithinSpecificSlashRange(ProgramSlashDistanceQuery q) => _resolve().DistancePhysicalSlash260IsWithinSpecificSlashRange(q);
    internal bool DistanceProvidedSlash110HasPotentialRankSlashRange(ProgramSlashDistanceQuery q) => _resolve().DistanceProvidedSlash110HasPotentialRankSlashRange(q);
    internal bool DistanceVirtualSlash120IsWithinSpecificSlashRange(ProgramSlashDistanceQuery q) => _resolve().DistanceVirtualSlash120IsWithinSpecificSlashRange(q);
    internal int FinalTargetSlashDamage(GameEngine.IDamageAttempt attack) => _resolve().FinalTargetSlashDamage(attack);
    internal int FinalTargetSlashEscalationBonus(GameEngine.IDamageAttempt attack, int damageWithoutEscalation) => _resolve().FinalTargetSlashEscalationBonus(attack, damageWithoutEscalation);
    internal IEnumerable<ProgramTargetSlashReceipt> FinalTargetSlashReceipts(long frameId, int target) => _resolve().FinalTargetSlashReceipts(frameId, target);
    internal bool HasFinalTargetSlashHpDamage(GameEngine.IDamageAttempt attack) => _resolve().HasFinalTargetSlashHpDamage(attack);
    internal bool HasPotentialRankSlashRange(CharacterState actor, CharacterState target, CardKind kind) => _resolve().HasPotentialRankSlashRange(actor, target, kind);
    internal bool HasRankSlashRange(CharacterState actor, CardKind kind) => _resolve().HasRankSlashRange(actor, kind);
    internal bool IsSpecificRankProviderPaymentLegal(NativeOwners.FactionCardRequestHandle pending, CardKind kind, IReadOnlyList<Card> cards, bool isTrueZhangba = false) => _resolve().IsSpecificRankProviderPaymentLegal(pending, kind, cards, isTrueZhangba);
    internal bool IsWithinSpecificSlashRange(CharacterState actor, CharacterState target, CardKind kind, int? rank, long? existingUseFrameId = default, int? excludedEquipmentId = default, EquipmentSlot? excludedEquipmentSlot = default) => _resolve().IsWithinSpecificSlashRange(actor, target, kind, rank, existingUseFrameId, excludedEquipmentId, excludedEquipmentSlot);
    internal void IssueFinalTargetSlashReceipt(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().IssueFinalTargetSlashReceipt(frame, effect);
    internal int? ProvidedSpecificSlashRank(CharacterState actor, CharacterState provider, IReadOnlyList<Card> cards, bool isTrueZhangba, CardKind kind = (CardKind)0) => _resolve().ProvidedSpecificSlashRank(actor, provider, cards, isTrueZhangba, kind);
    internal int? SpecificSlashRank(CharacterState actor, Card card, CardKind kind, long? useId = default) => _resolve().SpecificSlashRank(actor, card, kind, useId);
    internal int? ZhangbaSpecificSlashRank(CharacterState actor, IReadOnlyList<Card> cards, CharacterState? provider = default) => _resolve().ZhangbaSpecificSlashRank(actor, cards, provider);
}
