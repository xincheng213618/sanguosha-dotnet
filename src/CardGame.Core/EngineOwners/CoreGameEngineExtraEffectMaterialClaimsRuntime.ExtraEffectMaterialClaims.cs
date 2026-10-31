#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineExtraEffectMaterialClaimsRuntime
{
    internal void CaptureExtraEffectMaterialClaims(ProgramSkillFrame producer, global::CardGame.Core.GameEngine.IDamageAttempt attack, IReadOnlyList<Card> cards, long before, CardMoveReason reason)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { Action: { } action } use || _characterFeiYiOlOperations.NativeExtraEffectState(use)is not { } extra)
            return;
        if (producer.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied, ParentFrameId: var windowId, DamageFrameId: { } damageId } || _coreDamageCursorRuntimeOperations.ActiveDamageTrigger is not { } window || window.Id != windowId || window.ParentFrameId != damageId || _coreNativeDamageHandlesOperations.GetDamageTriggerAttack(window).ResolutionId != use.Id || producer.OwnerSeat != attack.TargetSeat || action.ActionId != extra.ActionId || cards.Any(c => !action.PhysicalCards.Any(p => p.CardId == c.Id)) || extra.MaterialClaims.Any(c => cards.Any(card => card.Id == c.CardId)))
            throw new InvalidOperationException("Extra-effect material claims require the original actual damage use and unclaimed physical costs.");
        var moves = _coreCardMovementJournalCardMovementsAccess._cardMovements.Where(m => m.Sequence > before && m.Reason == reason && m.From == CardLocation.Processing && m.To == CardLocation.Hand(producer.OwnerSeat)).ToArray();
        if (moves.Length != cards.Count || !moves.Select(m => m.CardId).Order().SequenceEqual(cards.Select(c => c.Id).Order()))
            throw new InvalidOperationException("Extra-effect material claims lost their exact one-time Processing-to-Hand movements.");
        var receipts = moves.Select(m => new ExtraEffectMaterialClaim(use.Id, action.ActionId, m.CardId, producer.Id, producer.SkillId, producer.TriggerId!, producer.SkillInstanceId, producer.GameplayHash, producer.InstructionIndex - 1, producer.OwnerSeat, window.Id, damageId, m.Sequence, reason.Value));
        _characterFeiYiOlOperations.UpdateNativeExtraEffectState(use.Id, current => current with { MaterialClaims = extra.MaterialClaims.Concat(receipts).ToArray() });
    }

    internal bool HasExtraEffectMaterialClaim(long useId, int cardId)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(useId)is not { Action: { } action } use || _characterFeiYiOlOperations.NativeExtraEffectState(use)is not { } extra || extra.MaterialClaims.SingleOrDefault(c => c.CardId == cardId)is not { } claim)
            return false;
        var trigger = _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(claim.SkillId)?.Program is { } program && program.GameplayHash == claim.GameplayHash ? ProgramInstructionResolver.Default.FindTrigger(program, claim.BindingId) : null;
        if (claim.CardUseFrameId != use.Id || claim.ActionId != action.ActionId || extra.ActionId != action.ActionId || !action.PhysicalCards.Any(c => c.CardId == claim.CardId) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(claim.RecipientSeat) || trigger is null || claim.EffectIndex < 0 || claim.EffectIndex >= trigger.Effects.Count || trigger.Effects[claim.EffectIndex].Op is not (SkillProgramEffectOp.ClaimDamageCards or SkillProgramEffectOp.ClaimShanheS5JianxiongCards) || !_coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.Sequence == claim.MovementSequence && m.CardId == claim.CardId && m.From == CardLocation.Processing && m.To == CardLocation.Hand(claim.RecipientSeat) && m.Reason.Value == claim.MovementReason) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DamageTriggerWindowOpenedEvent>().Count(e => e.ResolutionId == claim.DamageWindowFrameId && e.DamageFrameId == claim.DamageFrameId && e.TargetSeat == claim.RecipientSeat && e.CardId == use.CardId && e.CardKind == use.CardKind && e.Candidates.Any(c => c.ToProgramCandidate()is { } p && p.OwnerSeat == claim.RecipientSeat && p.SkillId == claim.SkillId && p.BindingId == claim.BindingId && p.SkillInstanceId == claim.SkillInstanceId && p.GameplayHash == claim.GameplayHash)) != 1 || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramBindingStartedEvent>().Count(e => e.FrameId == claim.ProgramFrameId && e.OwnerSeat == claim.RecipientSeat && e.SkillId == claim.SkillId && e.BindingId == claim.BindingId && e.SkillInstanceId == claim.SkillInstanceId && e.Window == SkillProgramTriggerWindow.AfterDamageApplied) != 1 || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramDamageCardsClaimedEvent>().Count(e => e.FrameId == claim.ProgramFrameId && e.SkillId == claim.SkillId && e.BindingId == claim.BindingId && e.OwnerSeat == claim.RecipientSeat && e.CardIds.Contains(claim.CardId)) != 1)
            throw new InvalidOperationException("An extra-effect material claim lost its exact original use, producer and actual claim movement.");
        return _cardZonesOperations._cardZones.GetLocation(cardId).Zone is CardZoneKind.Hand or CardZoneKind.DrawPile or CardZoneKind.DiscardPile;
    }

    internal void RestoreExtraEffectGroupMaterialClaims(global::CardGame.Core.NativeOwners.GroupCardHandle group)
    {
        var ids = (_coreCardUseLifecycleOperations.LifecycleCardUse(group.ResolutionId)is { } use ? _characterFeiYiOlOperations.NativeExtraEffectState(use) : null)?.MaterialClaims.Select(c => c.CardId).Where(id => HasExtraEffectMaterialClaim(group.ResolutionId, id)).ToArray();
        if (ids is { Length: > 0 })
            group.SetClaimedPhysicalCards(group.DamageClaimedPhysicalCardIds.Concat(ids).Distinct().ToArray());
    }
}
