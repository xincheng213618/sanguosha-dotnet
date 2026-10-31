#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYuJinMobileRuntime
{
    internal PublicPersistentPileSource? MobileYuJinPile(int seat, string instance) => _coreGameEnginePublicPileSourcesPublicPersistentPilesAccess._publicPersistentPiles.GetValueOrDefault((seat, MobileYuJinJieyue.Skill, instance));
    internal bool HasMobileYuJinPile(int seat, string instance) => _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[seat], MobileYuJinJieyue.Skill, instance) && MobileYuJinPile(seat, instance)is { } pile && _corePublicPileSourcesOperations.PublicPileCards(pile).Count > 0;
    [ProgramCardLegality(ProgramCardLegalityStage.ConfiguredConversionBan, 76300)]
    internal bool MobileYuJinConversionUnavailable(ProgramConfiguredViewAsQuery query) => query.Instance.SkillId == MobileYuJinJieyue.Skill && !HasMobileYuJinPile(query.Owner.Seat, query.Instance.SkillInstanceId);
    [ProgramCardLegality(ProgramCardLegalityStage.ViewAsBan, 76300)]
    internal bool MobileYuJinSubmittedConversionUnavailable(ProgramViewAsLegalityQuery query) => query.Source.SkillId == MobileYuJinJieyue.Skill && (!HasMobileYuJinPile(query.Owner.Seat, query.Source.SkillInstanceId) || query.CardIds.Count != 1 || _cardZonesOperations._cardZones.GetLocation(query.CardIds[0]) != CardLocation.Hand(query.Owner.Seat));
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 76300, SkillProgramEffectOp.CollectMobileYuJinJieyue)]
    internal bool MobileYuJinPreparationHasCards(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => context.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow && HasMobileYuJinPile(candidate.OwnerSeat, candidate.SkillInstanceId);
    SkillProgramStepOutcome IMobileYuJinHost.Transfer(ProgramSkillFrame frame, SkillProgramEffect effect) => BeginMobileYuJinTransfer(frame, effect);
    internal static string MobileYuJinTransferReason(SkillProgramEffectOp op) => op == SkillProgramEffectOp.ResolveMobileYuJinJieyue ? "skill.mobile-yu-jin.jieyue.store" : "skill.mobile-yu-jin.jieyue.retrieve";
    internal SkillProgramStepOutcome BeginMobileYuJinTransfer(ProgramSkillFrame supplied, SkillProgramEffect effect)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var store = effect.Op == SkillProgramEffectOp.ResolveMobileYuJinJieyue;
        if (frame.SkillId != MobileYuJinJieyue.Skill || frame.InstructionIndex != (store ? 3 : 1) || frame.TriggerId != (store ? MobileYuJinJieyue.Ending : MobileYuJinJieyue.Preparation) || frame.WindowContext is not { } context || context.Window != (store ? SkillProgramTriggerWindow.TurnEnding : SkillProgramTriggerWindow.TurnStartBeforeNormalFlow) || frame.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || frame.MobileYuJinTransfer is not null || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId))
            throw new InvalidOperationException("Jieyue requires its original own actual ending or preparation instruction.");
        if (store)
            return BeginMobileYuJinChoice(frame);
        var pile = MobileYuJinPile(frame.OwnerSeat, frame.SkillInstanceId);
        if (pile is null || _corePublicPileSourcesOperations.PublicPileCards(pile).Count == 0)
            return SkillProgramStepOutcome.Continue;
        var ids = _corePublicPileSourcesOperations.PublicPileCards(pile).Select(c => c.Id).ToArray();
        return PayMobileYuJinTransfer(frame, effect.Op, pile, ids, ids.Select(_cardZonesOperations._cardZones.GetLocation).ToArray());
    }

    internal SkillProgramStepOutcome PayMobileYuJinTransfer(ProgramSkillFrame frame, SkillProgramEffectOp operation, PublicPersistentPileSource pile, int[] ids, CardLocation[] from)
    {
        var store = operation == SkillProgramEffectOp.ResolveMobileYuJinJieyue;
        var originalId = frame.Id;
        var receipt = new MobileYuJinTransferReceipt
        {
            Operation = operation,
            Source = new(frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId),
            GameplayHash = frame.GameplayHash,
            Pile = pile,
            ActualTurn = _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber,
            ParentId = frame.WindowContext!.ParentFrameId,
            InstructionIndex = frame.InstructionIndex,
            Cards = ids,
            From = from,
            Destination = store ? pile.Location : CardLocation.Hand(frame.OwnerSeat),
            Before = _coreCardMovementJournalCardMovementsAccess._cardMovements.Count,
            After = _coreCardMovementJournalCardMovementsAccess._cardMovements.Count
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { MobileYuJinTransfer = receipt, MobileYuJinChoice = null, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileYuJinTransferStartedEvent(frame.Id, receipt.Operation, receipt.Source, receipt.GameplayHash, receipt.ActualTurn, receipt.ParentId, receipt.InstructionIndex, pile, ids.Length, receipt.Before));
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, receipt.Destination, new(MobileYuJinTransferReason(operation)), (_, _) =>
        {
            var paid = _coreProgramLifecycleOperations.GetActiveProgramFrame(originalId);
            var invoice = paid.MobileYuJinTransfer!with
            {
                After = _coreCardMovementJournalCardMovementsAccess._cardMovements.Count
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(paid with { MobileYuJinTransfer = invoice });
            foreach (var item in invoice.Cards.Select((id, index) => (id, from: invoice.From[index])))
            {
                var move = _coreCardMovementJournalCardMovementsAccess._cardMovements.Single(m => m.Sequence > invoice.Before && m.Sequence <= invoice.After && m.CardId == item.id && m.From == item.from && m.Reason.Value == MobileYuJinTransferReason(invoice.Operation));
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileYuJinCardTransferredEvent(paid.Id, item.id, item.from, move.To, move.Sequence));
            }
        });
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool ValidMobileYuJinTransfer(ProgramSkillFrame frame)
    {
        if (frame.MobileYuJinTransfer is not { } r || frame.SkillId != MobileYuJinJieyue.Skill || r.Source != new CardConversionSource(frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId) || r.GameplayHash != frame.GameplayHash || r.ActualTurn != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || r.ParentId != frame.WindowContext?.ParentFrameId || r.InstructionIndex != frame.InstructionIndex || !ExactMobileYuJinParent(frame, r.Operation == SkillProgramEffectOp.ResolveMobileYuJinJieyue) || r.Pile.OwnerSeat != frame.OwnerSeat || r.Pile.SkillId != frame.SkillId || r.Pile.SkillInstanceId != frame.SkillInstanceId || r.Pile.Capacity != int.MaxValue || r.Cards.Count == 0 || r.Cards.Count != r.From.Count || r.Cards.Distinct().Count() != r.Cards.Count || r.Before < 0 || r.After <= r.Before || r.After > _coreCardMovementJournalCardMovementsAccess._cardMovements.Count)
            return false;
        var store = r.Operation == SkillProgramEffectOp.ResolveMobileYuJinJieyue;
        if (!store && r.Operation != SkillProgramEffectOp.CollectMobileYuJinJieyue || frame.TriggerId != (store ? MobileYuJinJieyue.Ending : MobileYuJinJieyue.Preparation) || frame.InstructionIndex != (store ? 3 : 1) || frame.WindowContext?.Window != (store ? SkillProgramTriggerWindow.TurnEnding : SkillProgramTriggerWindow.TurnStartBeforeNormalFlow) || r.Destination != (store ? r.Pile.Location : CardLocation.Hand(frame.OwnerSeat)) || store && (r.Cards.Count != 1 || frame.SelectedTargetSeats is not [var target] || r.From.Any(l => l.OwnerSeat != target || l.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment))) || !store && r.From.Any(l => l != r.Pile.Location))
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        if (history.OfType<MobileYuJinTransferStartedEvent>().Count(e => e == new MobileYuJinTransferStartedEvent(frame.Id, r.Operation, r.Source, r.GameplayHash, r.ActualTurn, r.ParentId, r.InstructionIndex, r.Pile, r.Cards.Count, r.Before)) != 1 || history.OfType<MobileYuJinCardTransferredEvent>().Count(e => e.FrameId == frame.Id) != r.Cards.Count || history.OfType<MobileYuJinTransferReturnedEvent>().Any(e => e.FrameId == frame.Id))
            return false;
        return r.Cards.Select((card, index) => (card, from: r.From[index])).All(item => _coreCardMovementJournalCardMovementsAccess._cardMovements.Count(m => m.Sequence > r.Before && m.Sequence <= r.After && m.CardId == item.card && m.From == item.from && (m.To == r.Destination || m.To == CardLocation.OutsideGame) && m.Reason.Value == MobileYuJinTransferReason(r.Operation) && history.OfType<MobileYuJinCardTransferredEvent>().Count(e => e.FrameId == frame.Id && e.CardId == item.card && e.From == item.from && e.To == m.To && e.Sequence == m.Sequence) == 1) == 1);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 76300)]
    internal bool ResumeMobileYuJinTransfer(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MobileYuJinTransfer: { } r } frame || frame.Id != id)
            return false;
        if (!ValidMobileYuJinTransfer(frame))
            throw new InvalidOperationException("Jieyue lost its original paid entity transfer.");
        if (_corePublicPilePreparationOperations.PreparationGameEnded())
            return true;
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(frame.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(frame.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(frame.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(frame.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(frame.Id))
            return true;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileYuJinTransferReturnedEvent(frame.Id, r.Cards.Count, r.After));
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { MobileYuJinTransfer = null, PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        return true;
    }
    [ProgramContinuation(ProgramContinuationKind.Movement, 76300)]
    internal bool ReturnMobileYuJinMovement(ProgramSkillFrame frame)
    {
        if (frame.MobileYuJinTransfer is null || frame.PendingMovementContinuation is null)
            return false;
        if (!ValidMobileYuJinTransfer(frame))
            throw new InvalidOperationException("Jieyue movement returned without its original paid receipt.");
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 76300)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 76300)]
    internal bool KeepMobileYuJinPaidTransfer(ProgramSkillFrame frame) => frame.MobileYuJinTransfer is not null && ValidMobileYuJinTransfer(frame);
}
