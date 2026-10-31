#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhouFangOlRuntime
{
    internal int DuanfaDiscardedThisPhase(int ownerSeat, string skillId) => _coreCommandJournalStorageEventsPendingEventsAccess._events.Select(e => e.Payload).Concat(_coreCommandJournalStorageEventsPendingEventsAccess._pendingEvents).OfType<ProgramDuanfaRecycledEvent>().Where(e => e.OwnerSeat == ownerSeat && e.SkillId == skillId && e.TurnNumber == _coreActualTurnProgressTurnNumberAccess._turnNumber).Sum(e => e.DiscardedCardIds.Count);
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 90, SkillProgramEffectOp.DuanfaDiscardAndDraw)]
    internal bool CanStartDuanfaRecycle(CharacterState owner, string skillId) => DuanfaDiscardedThisPhase(owner.Seat, skillId) < owner.MaxHp && _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Any(card => !CoreNativeCardClassificationsRuntime.IsRedSuit(_coreProgramCardPoliciesOperations.GetProgramEffectiveSuit(owner, card)));
    internal SkillProgramStepOutcome DuanfaProgramDiscardAndDraw(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        var binding = active.CardSetBindings.SingleOrDefault(item => item.Name == effect.SourceBind!);
        if (binding is null || binding.CardIds.Count == 0)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(active, "断发的选牌绑定未生成，技能结算取消。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (binding.CardIds.Any(cardId => _cardZonesOperations._cardZones.GetLocation(cardId) != CardLocation.Hand(owner.Seat)))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(active, "断发所选的牌已离开手牌区，技能结算取消。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        var allowance = owner.MaxHp - DuanfaDiscardedThisPhase(owner.Seat, active.SkillId);
        if (allowance <= 0 || binding.CardIds.Count > allowance)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(active, "断发本阶段至多以此法弃置体力上限张牌，本次结算取消，所选牌未移动。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        var cards = binding.CardIds.Select(cardId => _cardZonesOperations._cardZones.CardsAt(CardLocation.Hand(owner.Seat)).Single(card => card.Id == cardId)).ToArray();
        _coreNativeLogicalDiscardsOperations.MoveLogicalDiscardCards(active.OwnerSeat, active.OwnerSeat, cards, CardLocation.Hand(owner.Seat), new($"skill-program.{(active.SkillId)}.duanfa-discard"));
        _corePhysicalDrawFacadesOperations.DrawCards(owner, cards.Length, true, new($"skill-program.{(active.SkillId)}.duanfa-draw"));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramDuanfaRecycledEvent(active.Id, active.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), owner.Seat, _coreActualTurnProgressTurnNumberAccess._turnNumber, Array.AsReadOnly(cards.Select(card => card.Id).ToArray()), cards.Length));
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }
}
