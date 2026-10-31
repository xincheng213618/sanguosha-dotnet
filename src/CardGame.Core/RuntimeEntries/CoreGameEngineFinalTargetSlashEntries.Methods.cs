#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFinalTargetSlashEntries
{
    internal bool CanOfferFinalTargetSlash(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext window) => _resolve().CanOfferFinalTargetSlash(candidate, trigger, window);
    internal bool DistanceBorrowedPaidWeaponSlash50HasRankSlashRange(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().DistanceBorrowedPaidWeaponSlash50HasRankSlashRange(q);
    internal bool DistanceBorrowedPhysicalSlash90IsWithinSpecificSlashRange(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().DistanceBorrowedPhysicalSlash90IsWithinSpecificSlashRange(q);
    internal bool DistancePhysicalSlash260IsWithinSpecificSlashRange(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().DistancePhysicalSlash260IsWithinSpecificSlashRange(q);
    internal bool DistanceProvidedSlash110HasPotentialRankSlashRange(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().DistanceProvidedSlash110HasPotentialRankSlashRange(q);
    internal bool DistanceVirtualSlash120IsWithinSpecificSlashRange(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().DistanceVirtualSlash120IsWithinSpecificSlashRange(q);
    internal int FinalTargetSlashDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().FinalTargetSlashDamage(attack);
    internal int FinalTargetSlashEscalationBonus(global::CardGame.Core.GameEngine.IDamageAttempt attack, int damageWithoutEscalation) => _resolve().FinalTargetSlashEscalationBonus(attack, damageWithoutEscalation);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTargetSlashReceipt> FinalTargetSlashReceipts(long frameId, int target) => _resolve().FinalTargetSlashReceipts(frameId, target);
    internal bool HasFinalTargetSlashHpDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().HasFinalTargetSlashHpDamage(attack);
    internal bool HasPotentialRankSlashRange(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind kind) => _resolve().HasPotentialRankSlashRange(actor, target, kind);
    internal bool HasRankSlashRange(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind) => _resolve().HasRankSlashRange(actor, kind);
    internal bool IsSpecificRankProviderPaymentLegal(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, bool isTrueZhangba = false) => _resolve().IsSpecificRankProviderPaymentLegal(pending, kind, cards, isTrueZhangba);
    internal bool IsWithinSpecificSlashRange(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind kind, int? rank, long? existingUseFrameId = default, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default) => _resolve().IsWithinSpecificSlashRange(actor, target, kind, rank, existingUseFrameId, excludedEquipmentId, excludedEquipmentSlot);
    internal void IssueFinalTargetSlashReceipt(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().IssueFinalTargetSlashReceipt(frame, effect);
    internal int? ProvidedSpecificSlashRank(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState provider, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, bool isTrueZhangba, global::CardGame.Core.CardKind kind = (global::CardGame.Core.CardKind)0) => _resolve().ProvidedSpecificSlashRank(actor, provider, cards, isTrueZhangba, kind);
    internal int? SpecificSlashRank(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, long? useId = default) => _resolve().SpecificSlashRank(actor, card, kind, useId);
    internal int? ZhangbaSpecificSlashRank(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CharacterState? provider = default) => _resolve().ZhangbaSpecificSlashRank(actor, cards, provider);
}
