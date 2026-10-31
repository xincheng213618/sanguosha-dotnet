#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineTieredRoundZeroReturnsRuntime
{
    internal bool TieredRoundZeroDyingRescueRide(int dyingIndex, DyingFrame dying)
    {
        if (dyingIndex < 0 || dyingIndex + 1 >= _resolutionStackOperations._resolutionStack.Count || _coreDyingCursorRuntimeOperations.ActiveDying?.FrameId != dying.Id || _resolutionStackOperations._resolutionStack[dyingIndex] is not DyingFrame original || original != dying || _resolutionStackOperations._resolutionStack[dyingIndex + 1] is not CardUseFrame rescue || !_coreTieredRoundZeroUsesOperations.IsIssuedTieredRoundUse(rescue, true) || rescue.TieredRoundConversionUse!.DyingFrameId != dying.Id || rescue.DyingResponse is not { } token || token.ResolutionId != dying.Id || token.ResponderSeat != dying.ResponderSeat || rescue.SourceSeat != dying.ResponderSeat || rescue.CardKind is not (CardKind.Peach or CardKind.Alcohol) || !rescue.TargetSeats.SequenceEqual([dying.VictimSeat]) || rescue.Action is not { Type: CardActionType.Use, PhysicalCards.Count: 0 } action || action.ActorSeat != token.ResponderSeat || !action.EffectiveDesignatedTargetSeats.SequenceEqual([dying.VictimSeat]) || (rescue.CardKind == CardKind.Peach ? !token.UsedPeach || token.PeachCardId != 0 || token.UsedAlcohol || token.AlcoholCardId is not null : !token.UsedAlcohol || token.AlcoholCardId != 0 || token.UsedPeach || token.PeachCardId is not null || rescue.SourceSeat != dying.VictimSeat) || token.UsedPeachPhysicalCardKind is not null)
            return false;
        for (var child = dyingIndex + 2; child < _resolutionStackOperations._resolutionStack.Count; child++)
            if (!_coreParticipantHandDamageRidesOperations.ParticipantHandRescueObserverRide(_resolutionStackOperations._resolutionStack[child], _resolutionStackOperations._resolutionStack[child - 1], rescue))
                return false;
        return true;
    }

    internal bool HasTieredRoundZeroDyingRide(long dyingId)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dyingId && f is DyingFrame);
        return index >= 0 && TieredRoundZeroDyingRescueRide(index, (DyingFrame)_resolutionStackOperations._resolutionStack[index]);
    }

    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 610, group: 10)]
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 700)]
    internal bool IsTieredRoundZeroRescueProgramDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && _resolutionStackOperations._resolutionStack.LastOrDefault() is ProgramSkillFrame && HasTieredRoundZeroDyingRide(dying.FrameId);
    internal Card ReadTieredRoundUseAppearance(long ownerId, CardAppearanceReference appearance) => appearance.Id == 0 && appearance.Kind == CardKind.FireAttack && appearance.Suit == Suit.None && appearance.Rank == 0 && _characterZhouYuMouOperations.IsMouZyZeroFireUse(ownerId) ? new Card(0, CardKind.FireAttack, Suit.None, 0) : appearance.Id == 0 && _characterHuanShuJunOlOperations.IsHuanpeiUse(ownerId) && _coreCardUseLifecycleOperations.LifecycleCardUse(ownerId)!.CardKind == appearance.Kind && appearance.Suit == Suit.None && appearance.Rank == 0 ? _characterHuanShuJunOlOperations.HuanpeiRepresentation(ownerId) : appearance.Id == 0 && _characterLiuZhangOlOperations.IsZonghuUse(ownerId) && _coreCardUseLifecycleOperations.LifecycleCardUse(ownerId)!.CardKind == appearance.Kind && appearance.Suit == Suit.None && appearance.Rank == 0 ? _characterLiuZhangOlOperations.ZonghuRepresentation(ownerId) : appearance.Id == 0 && _characterGuoTuOlOperations.IsWeijieUse(ownerId) && _coreCardUseLifecycleOperations.LifecycleCardUse(ownerId)!.CardKind == appearance.Kind && appearance.Suit == Suit.None && appearance.Rank == 0 ? _characterGuoTuOlOperations.WeijieRepresentation(ownerId) : appearance.Id == 0 && _characterLuKangWumiaoOperations.IsWuLuKangKegouUse(ownerId) && _coreCardUseLifecycleOperations.LifecycleCardUse(ownerId)!.CardKind == appearance.Kind && appearance.Suit == Suit.None && appearance.Rank == 0 ? _characterLuKangWumiaoOperations.WuLuKangKegouRepresentation(ownerId) : appearance.Id == 0 && _characterHuCheErMouOperations.IsMouHuCheErUse(ownerId) && _coreCardUseLifecycleOperations.LifecycleCardUse(ownerId)!.CardKind == appearance.Kind && appearance.Suit == Suit.None && appearance.Rank == 0 ? _characterHuCheErMouOperations.MouHuCheErRepresentation(ownerId) : appearance.Id == 0 && _characterZhangXiuMouOperations.IsMouZhangXiuAnniversaryUse(ownerId) && _coreCardUseLifecycleOperations.LifecycleCardUse(ownerId)!.CardKind == appearance.Kind && appearance.Suit == Suit.None && appearance.Rank == 0 ? _characterZhangXiuMouOperations.MouZhangXiuAnniversaryRepresentation(ownerId) : appearance.Id == 0 && _characterHuLieMouOperations.IsMouHuLieUse(ownerId) && _coreCardUseLifecycleOperations.LifecycleCardUse(ownerId)!.CardKind == appearance.Kind && appearance.Suit == Suit.None && appearance.Rank == 0 ? _characterHuLieMouOperations.MouHuLieRepresentation(ownerId) : appearance.Id == 0 && _characterGuanHaiOlOperations.IsYouqueUse(ownerId) && _coreCardUseLifecycleOperations.LifecycleCardUse(ownerId)!.CardKind == appearance.Kind && appearance.Suit == Suit.None && appearance.Rank == 0 ? _characterGuanHaiOlOperations.YouqueRepresentation(ownerId) : appearance.Id == 0 && _characterTaoQianOlOperations.IsZongluanUse(ownerId) && _coreCardUseLifecycleOperations.LifecycleCardUse(ownerId)!.CardKind == appearance.Kind && appearance.Suit == Suit.None && appearance.Rank == 0 ? _characterTaoQianOlOperations.ZongluanRepresentation(ownerId) : appearance.Id == 0 && appearance.Kind == CardKind.Alcohol && appearance.Suit == Suit.None && appearance.Rank == 0 && (_characterLiuYanOlOperations.IsLiuYanLegacyAlcoholUse(ownerId) || _characterCuiYanOlOperations.IsCuiYanLegacyAlcoholUse(ownerId) || _characterHuangYueyingOlMouOperations.IsMouBingcaiLegacyAlcoholUse(ownerId)) ? new Card(0, CardKind.Alcohol, Suit.None, 0) : appearance.Id == 0 && _characterAHuiNanOlOperations.IsAHuiNanJuemanUse(ownerId) && _coreCardUseLifecycleOperations.LifecycleCardUse(ownerId)!.CardKind == appearance.Kind && appearance.Suit == Suit.None && appearance.Rank == 0 ? new Card(0, appearance.Kind, Suit.None, 0) : appearance.Id == 0 && _characterQinLangOlOperations.IsQinLangXianyingUse(ownerId) && _coreCardUseLifecycleOperations.LifecycleCardUse(ownerId)!.CardKind == appearance.Kind && appearance.Suit == Suit.None && appearance.Rank == 0 ? new Card(0, appearance.Kind, Suit.None, 0) : appearance.Id == 0 && _characterTianChouOlOperations.IsTianChouShandaoUse(ownerId) && _coreCardUseLifecycleOperations.LifecycleCardUse(ownerId)!.CardKind == appearance.Kind && appearance.Suit == Suit.None && appearance.Rank == 0 ? new Card(0, appearance.Kind, Suit.None, 0) : appearance.Id == 0 && _coreRoundOnceCardlessNativeUsesOperations.IsRoundOnceCardlessBasicUse(ownerId) && _coreCardUseLifecycleOperations.LifecycleCardUse(ownerId)!.CardKind == appearance.Kind && appearance.Suit == Suit.None && appearance.Rank == 0 ? _coreRoundOnceCardlessNativeUsesOperations.RoundOnceCardlessNativeRepresentation(ownerId) : appearance.Id == 0 && _characterBaoSanNiangOlOperations.IsChosenAssaultUse(ownerId) && appearance.Kind == CardKind.BarbarianAssault && appearance.Suit == Suit.None && appearance.Rank == 0 ? new Card(0, CardKind.BarbarianAssault, Suit.None, 0) : appearance.Id == 0 && _characterZhugeLiangMouOperations.IsMouZhijueZeroUse(ownerId) && _coreCardUseLifecycleOperations.LifecycleCardUse(ownerId)!.CardKind == appearance.Kind && appearance.Suit == Suit.None && appearance.Rank == 0 ? new Card(0, appearance.Kind, Suit.None, 0) : appearance.Id == 0 && _coreTieredRoundZeroUsesOperations.IsTieredRoundZeroUse(ownerId) && _coreCardUseLifecycleOperations.LifecycleCardUse(ownerId)!.CardKind == appearance.Kind && appearance.Suit == Suit.None && appearance.Rank == 0 ? _coreTieredRoundZeroUsesOperations.TieredRoundZeroRepresentation(ownerId) : _coreCardAttackStateOperations.ReadCardAppearance(appearance);
    internal bool IsPaidZeroMaterialAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, IReadOnlyList<Card> processing)
    {
        if ((!_characterHuanShuJunOlOperations.IsHuanpeiUse(attack.ResolutionId) && !_characterLiuZhangOlOperations.IsZonghuUse(attack.ResolutionId) && !_characterGuoTuOlOperations.IsWeijieUse(attack.ResolutionId) && !(_characterGuanHaiOlOperations.IsYouqueUse(attack.ResolutionId) || _characterLuKangWumiaoOperations.IsWuLuKangKegouUse(attack.ResolutionId) || _characterHuCheErMouOperations.IsMouHuCheErUse(attack.ResolutionId) || _characterZhangXiuMouOperations.IsMouZhangXiuAnniversaryUse(attack.ResolutionId) || _characterHuLieMouOperations.IsMouHuLieUse(attack.ResolutionId)) && !_characterTaoQianOlOperations.IsZongluanUse(attack.ResolutionId) && !_coreTieredRoundZeroUsesOperations.IsTieredRoundZeroUse(attack.ResolutionId) && !_coreRoundOnceCardlessNativeUsesOperations.IsRoundOnceCardlessBasicUse(attack.ResolutionId)) || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { } use || attack.Card is not null || attack.PhysicalCards.Count != 0 || attack.EffectiveCardKind != use.CardKind)
            return false;
        var remaining = processing;
        if (_coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword is { ActiveAttack: { } forced } borrowed && global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(forced, attack))
        {
            if (use.SourceSeat != borrowed.WeaponOwnerSeat || !use.TargetSeats.SequenceEqual([borrowed.SlashTargetSeat]) || use.Action?.ParentActionId != _coreCardUseLifecycleOperations.LifecycleCardUse(borrowed.ResolutionId)?.Action?.ActionId)
                return false;
            var parentIds = _coreNativeTrickUseOperations.GetCardUsePhysicalCards(borrowed.ResolutionId).Select(c => c.Id).ToArray();
            if (parentIds.Any(id => processing.All(c => c.Id != id)))
                return false;
            remaining = processing.Where(c => !parentIds.Contains(c.Id)).ToArray();
        }
        else if (_coreNativeCardContinuationCursorsOperations.ActiveQinglongFollowup is { } blade && blade.OuterResolutionId != use.Id && _coreCardUseLifecycleOperations.LifecycleCardUse(blade.OuterResolutionId)is { } original && use.Action?.ParentActionId == original.Action?.ActionId && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<QinglongCrescentBladeResolvedEvent>().LastOrDefault()is { Used: true, SlashCardIds.Count: 0 } fact && fact.ResolutionId == original.Id && fact.SourceSeat == use.SourceSeat && fact.TargetSeat == attack.TargetSeat && fact.EffectiveSlashKind == use.CardKind)
        {
            var exactParentIds = blade.NextAttack?.PhysicalCards.Where(c => _cardZonesOperations._cardZones.GetLocation(c.Id) == CardLocation.Processing).Select(c => c.Id).ToArray() ?? [];
            if (exactParentIds.Any(id => processing.All(c => c.Id != id)))
                return false;
            remaining = processing.Where(c => !exactParentIds.Contains(c.Id)).ToArray();
        }

        if (use.CardKind == CardKind.FireAttack)
            return remaining.Count == 0 && MatchesTieredRoundZeroFireAttackAttack(attack);
        return remaining.Count == 0 && ((global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || use.CardKind == CardKind.Duel) && attack.SourceSeat == use.SourceSeat || use.CardKind is CardKind.BarbarianAssault or CardKind.ArrowBarrage && _coreNativeCardContinuationCursorsOperations.ActiveGroupCard is { } group && group.ResolutionId == use.Id && group.SourceSeat == use.SourceSeat && group.DamageSourceSeat == attack.SourceSeat && group.CurrentAttack?.ResolutionId == use.Id);
    }

    internal bool IsTieredRoundZeroFireAttackUse(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { CardId: 0, CardKind: CardKind.FireAttack, PhysicalCardIds.Count: 0 } use && _coreTieredRoundZeroUsesOperations.IsIssuedTieredRoundUse(use, true);
    internal bool MatchesTieredRoundZeroFireAttackAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => IsTieredRoundZeroFireAttackUse(attack.ResolutionId) && _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is { } use && attack.Card is null && attack.PhysicalCards.Count == 0 && attack.EffectiveCardKind == CardKind.FireAttack && attack.SourceSeat == use.SourceSeat && attack.CardUserSeat == use.SourceSeat && attack.ProgramSkillFrameId is null && attack.ProgramSkillCardUseFrameId is null && attack.ProgramJudgmentFrameId is null && !attack.IsDelayedJudgmentDamage && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<FireAttackResolvedEvent>().LastOrDefault(fact => fact.ResolutionId == use.Id)is { CausedDamage: true, MatchingDiscardCardId: > 0 } paid && paid.SourceSeat == use.SourceSeat && use.TargetSeats.Contains(paid.TargetSeat);
    internal bool IsTieredRoundZeroProcessingParent(long id, int cardId) => cardId == 0 && (_characterHuanShuJunOlOperations.IsHuanpeiUse(id) || _characterLiuZhangOlOperations.IsZonghuUse(id) || _characterGuoTuOlOperations.IsWeijieUse(id) || (_characterGuanHaiOlOperations.IsYouqueUse(id) || _characterLuKangWumiaoOperations.IsWuLuKangKegouUse(id) || _characterHuCheErMouOperations.IsMouHuCheErUse(id) || _characterZhangXiuMouOperations.IsMouZhangXiuAnniversaryUse(id) || _characterHuLieMouOperations.IsMouHuLieUse(id)) || _characterTaoQianOlOperations.IsZongluanUse(id) || _coreTieredRoundZeroUsesOperations.IsTieredRoundZeroUse(id) || _coreRoundOnceCardlessNativeUsesOperations.IsRoundOnceCardlessBasicUse(id));
    internal bool MatchesTieredRoundZeroUseAction(long id, CardActionContext action, int cardId) => cardId == 0 && _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { } use && _coreTieredRoundZeroUsesOperations.IsIssuedTieredRoundUse(use, true) && TieredRoundActionsStructurallyMatch(use.Action, action) && action.Type == CardActionType.Use && action.EffectiveKind == use.CardKind && action.PhysicalCards.Count == 0;
    internal bool IsTieredRoundZeroBorrowedAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, IReadOnlyList<Card> processing)
    {
        if (_coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword is not { ActiveAttack: { } child } borrowed || !global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(child, attack) || !_coreTieredRoundZeroUsesOperations.IsTieredRoundZeroUse(borrowed.ResolutionId) || _coreCardUseLifecycleOperations.LifecycleCardUse(borrowed.ResolutionId)?.CardKind != CardKind.BorrowedSword || attack.Card is null || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(attack.EffectiveCardKind ?? attack.Card.Kind))
            return false;
        var costs = attack.PhysicalCards.Where(c => !_coreCurrentUsePhysicalClaimsOperations.IsCurrentUsePhysicalCardClaim(attack.ResolutionId, c.Id)).ToArray();
        if (costs.All(c => _cardZonesOperations._cardZones.GetLocation(c.Id) == CardLocation.Processing))
            return processing.Select(c => c.Id).Order().SequenceEqual(costs.Select(c => c.Id).Order());
        return _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is { Step: ResolutionFrameStep.Completed, Action: { } accepted } use && accepted.PhysicalCards.Select(c => c.CardId).SequenceEqual(attack.PhysicalCards.Select(c => c.Id)) && _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().LastOrDefault()is { Continuation: ProgramCardContinuation.CompletedSlash } window && window.ParentFrameId == use.Id && TieredRoundActionsStructurallyMatch(window.Action, accepted) && processing.Count == 0 && costs.All(c => _cardZonesOperations._cardZones.GetLocation(c.Id).Zone is CardZoneKind.DiscardPile or CardZoneKind.Hand or CardZoneKind.DrawPile);
    }

    internal static bool TieredRoundActionsStructurallyMatch(CardActionContext? left, CardActionContext right) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_TieredRoundZeroReturns.TieredRoundActionsStructurallyMatch(left, right);
    internal bool SkipTieredRoundZeroFinishedMovement(long id, Card card)
    {
        if (card.Id != 0 || !_coreTieredRoundZeroUsesOperations.IsTieredRoundZeroUse(id) && !_characterQinLangOlOperations.IsQinLangXianyingUse(id) && !_characterAHuiNanOlOperations.IsAHuiNanJuemanUse(id))
            return false;
        if (card.Kind != _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.CardKind)
            throw new InvalidOperationException("The logical completed card changed its issued card name.");
        return true; // No physical entity was issued; there is no cleanup move.
    }

    internal void AssertCappedConversionBenefits()
    {
        foreach (var frame in _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Where(f => f.CappedConversionBenefit is not null))
            if (!_coreCappedConversionBenefitsOperations.ValidCappedConversionBenefit(frame))
                throw new InvalidOperationException("A conversion benefit lost its exact draw and after-damage issuance.");
    }

    internal bool IsCappedConversionAwaitedMovement(ProgramSkillFrame frame, ProgramMovementContinuation pending) => frame.CappedConversionBenefit is { AwaitingMovement: true } && pending.SubjectSeat == frame.OwnerSeat && pending.BeforeCount == 0 && pending.CoverageResultBind is null && _coreCappedConversionBenefitsOperations.ValidCappedConversionBenefit(frame);
}
