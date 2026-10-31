#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYangWanOlRuntime
{
    internal ActualDiscardRecoveryPhaseKey? ActualOwnYouyanDiscard(int owner, ProgramSkillWindowContext? context) => context is { Window: SkillProgramTriggerWindow.DiscardPileReceived, MovementBatch.DiscardRecoveryPhase: { Kind: ActualDiscardRecoveryPhaseKind.Play or ActualDiscardRecoveryPhaseKind.Discard } phase } && phase.ActorSeat == owner && phase.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase) ? phase : null;
    internal static string YouyanPhaseUsage(ActualDiscardRecoveryPhaseKey phase) => "youyan-phase:" + phase.Token.ToString(System.Globalization.CultureInfo.InvariantCulture);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 7268, SkillProgramEffectOp.YouyanGainSuitCards)]
    internal bool CanOfferActualOwnYouyan(ProgramTriggerLegalityContext gate) => ActualOwnYouyanDiscard(gate.OwnerSeat, gate.Context)is { } phase && _skillRuntimeStateOperations._skillRuntimeState.GetUsage(gate.OwnerSeat, gate.SkillId, YouyanPhaseUsage(phase), SkillUsageScope.Turn) == 0;
    internal SkillProgramStepOutcome YouyanProgramGainSuitCards(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        if (ActualOwnYouyanDiscard(active.OwnerSeat, active.WindowContext)is not { } phase || frame.WindowContext is not { Window: SkillProgramTriggerWindow.DiscardPileReceived, MovementBatch: { } batch } || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !owner.IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(active))
            return SkillProgramStepOutcome.Continue;
        var usage = YouyanPhaseUsage(phase);
        if (!_skillRuntimeStateOperations._skillRuntimeState.TryConsumeUsage(active.OwnerSeat, active.SkillId, usage, SkillUsageScope.Turn, 1))
            return SkillProgramStepOutcome.Continue;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SkillUsageConsumedEvent(active.OwnerSeat, active.SkillId, usage, SkillUsageScope.Turn, 1));
        var discardedSuitMask = ProgramYouyanDiscardedSuitMask(owner.Seat, batch);
        if (discardedSuitMask == 0)
            return SkillProgramStepOutcome.Continue;
        var gained = 0;
        foreach (var suit in new[]
        {
            Suit.Spade,
            Suit.Heart,
            Suit.Club,
            Suit.Diamond
        }

        )
        {
            if ((discardedSuitMask & SuitMask(suit)) != 0)
                continue;
            var card = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Reverse().FirstOrDefault(item => item.Suit == suit);
            if (card is null)
                continue;
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.DrawPile, CardLocation.Hand(owner.Seat), new($"skill-program.{(active.SkillId)}.youyan-gain"));
            gained++;
        }

        if (gained == 0)
        {
            _coreMatchLogOperations.AddLog("SkillEffect", $"{(owner.Name)} 诱言：牌堆中没有其余花色的牌可获得。", active.OwnerSeat);
            return SkillProgramStepOutcome.Continue;
        }

        _coreMatchLogOperations.AddLog("SkillEffect", $"{(owner.Name)} 诱言：从牌堆中获得 {(gained)} 张与弃置牌花色不同的牌。", active.OwnerSeat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramYouyanGainEvent(active.Id, active.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), active.OwnerSeat, discardedSuitMask, gained));
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal int ProgramYouyanDiscardedSuitMask(int ownerSeat, CardMovementBatchContext batch)
    {
        var mask = 0;
        foreach (var movement in batch.Movements)
        {
            if (movement.To != CardLocation.DiscardPile || _coreProgramDiscardOriginsOperations.GetProgramDiscardSource(movement)is not { OwnerSeat: var source } || source != ownerSeat)
                continue;
            var card = _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(movement.CardId)).SingleOrDefault(item => item.Id == movement.CardId);
            if (card is not null)
                mask |= SuitMask(card.Suit);
        }

        return mask;
    }
}
