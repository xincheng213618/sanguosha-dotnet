#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDaQiaoMobileRuntime
{
    internal bool UsesPostCostSlashRedirectRange(ProgramSkillFrame frame) => ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).Features.ForOperation(SkillProgramEffectOp.SelectAndMoveOwnedCard).Any(e => e.SlashRedirectPostCostRange == true);
    internal ProgramSkillFrame? PostCostSlashRedirectFrame(NativeOwners.CardAttackHandle attack, int owner) => _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().LastOrDefault(frame => frame.OwnerSeat == owner && frame.WindowContext is { Window: SkillProgramTriggerWindow.SlashTargetRedirecting, CardUse: { } use } && use.ParentCardUseFrameId == attack.ResolutionId && UsesPostCostSlashRedirectRange(frame));
    internal bool IsPostCostSlashRedirectTarget(NativeOwners.CardAttackHandle attack, int owner, int target, int? excludedEquipment)
    {
        if (owner == attack.SourceSeat || !_coreProgramAttackEffectsOperations.IsProgramSlashRedirectBaseTarget(attack, owner, target))
            return false;
        return _characterTaishiCiMobileGodOperations.HasShenTaishiCiPoweiRange(owner, target) || _characterSunYuanOlOperations.HasSayingRoundRange(owner, target) || _coreProgramPersistentCombatPoliciesOperations.IsGameFactionAttackRangeTarget(owner, target) || CoreGameEngineRuleQueriesRuntime.ConvertRuleValue(_coreActionQueriesOperations.EvaluateDistance(_coreMatchParticipantsPlayersAccess._players[owner], _coreMatchParticipantsPlayersAccess._players[target], excludedEquipment)) <= CoreGameEngineRuleQueriesRuntime.ConvertRuleValue(_coreActionQueriesOperations.EvaluateAttackRange(_coreMatchParticipantsPlayersAccess._players[owner], excludedEquipment));
    }

    internal bool IsPostCostSlashRedirectPaymentPair(ProgramSkillFrame frame, NativeOwners.CardAttackHandle attack, int target, CardLocation source, Card card) => source.OwnerSeat == frame.OwnerSeat && source.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && _cardZonesOperations._cardZones.GetLocation(card.Id) == source && !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(frame.OwnerSeat, card, source, OwnedCardMoveIntent.Discard) && IsPostCostSlashRedirectTarget(attack, frame.OwnerSeat, target, source.Zone == CardZoneKind.Equipment ? card.Id : null);
    internal bool HasPostCostSlashRedirectPayment(ProgramSkillFrame frame, NativeOwners.CardAttackHandle attack, int target)
    {
        // The shared enumerator applies real HE discard/source-equipment restrictions.
        // During target selection there is no selected target, so the payment hook
        // is inert; each surviving entity is tested against this candidate once.
        return _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(frame.Id, frame.OwnerSeat, frame.OwnerSeat, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Discard).Where(choice => choice.Cards.Count == 1).Any(choice =>
        {
            var card = _coreCardAttackStateOperations.GetAttackCard(choice.Cards[0]);
            return IsPostCostSlashRedirectPaymentPair(frame, attack, target, _cardZonesOperations._cardZones.GetLocation(card.Id), card);
        });
    }

    internal bool AllowsPostCostSlashRedirectPayment(ProgramSkillFrame frame, CardLocation source, Card card)
    {
        if (_coreProgramOwnedCardsOperations.GetOwnedSelectionEffect(frame).SlashRedirectPostCostRange != true)
            return true;
        return frame.SelectedTargetSeats is [var target] && _coreCardActionsOperations.ProgramCardAttack is { } attack && CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, attack) && attack.TargetSeat == frame.OwnerSeat && IsPostCostSlashRedirectPaymentPair(frame, attack, target, source, card);
    }
}
