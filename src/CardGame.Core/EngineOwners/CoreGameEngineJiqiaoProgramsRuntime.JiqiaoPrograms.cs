#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineJiqiaoProgramsRuntime
{
    internal const string JiqiaoCostReason = "program.jiqiao.cost", JiqiaoClaimReason = "program.jiqiao.obtain", JiqiaoRemainderReason = "program.jiqiao.remainder";
    internal SkillProgramStepOutcome ExecutePaidJiqiao(ProgramSkillFrame input, SkillProgramEffect effect)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.JiqiaoPaid is not null || f.WindowContext is not { Window: SkillProgramTriggerWindow.PlayPhaseStarting, SourceSeat: { } actor } w || actor != f.OwnerSeat || actor != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat)
            throw new InvalidOperationException("Jiqiao needs its unpaid genuine own Play-start.");
        var cost = _coreProgramLifecycleOperations.GetProgramCardSet(f, effect.SourceBind!);
        var ids = cost.CardIds;
        if (ids.Distinct().Count() != ids.Count || ids.Any(id => _cardZonesOperations._cardZones.GetLocation(id)is not { OwnerSeat: { } s, Zone: CardZoneKind.Hand or CardZoneKind.Equipment } || s != f.OwnerSeat || !_coreEquipmentContentOperations.EquipmentRules.IsEquipment(JiqiaoCard(id).Kind) || JiqiaoCard(id).IsGeneralWeapon || _coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(f.OwnerSeat, JiqiaoCard(id), _cardZonesOperations._cardZones.GetLocation(id), OwnedCardMoveIntent.Discard)))
            throw new InvalidOperationException("Jiqiao selected cards ceased to be actual owner equipment.");
        if (ids.Count == 0)
        {
            _coreProgramLifecycleOperations.SetProgramCardSet(f.Id, effect.ResultBind!, [], SkillProgramCardSetVisibility.Public);
            return SkillProgramStepOutcome.Continue;
        }

        var before = _coreCardMovementJournalMovementSequenceAccess._movementSequence;
        var invoice = new JiqiaoPaidInvoice(new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, w.ParentFrameId, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, f.InstructionIndex, effect.SourceBind!, effect.ResultBind!, ids.Count, before, before);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { JiqiaoPaid = new(invoice, JiqiaoPaidStage.CostChildren, before, before, ids.Count), PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, ids, new(JiqiaoCostReason));
        invoice = invoice with
        {
            CostAfter = _coreCardMovementJournalMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { JiqiaoPaid = f.JiqiaoPaid!with { Invoice = invoice, After = _coreCardMovementJournalMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JiqiaoCostPaidEvent(f.Id, invoice));
        IssueJiqiaoStage(f);
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal Card JiqiaoCard(int id) => _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(id)).Single(c => c.Id == id);
    internal void IssueJiqiaoStage(ProgramSkillFrame f)
    {
        var r = f.JiqiaoPaid!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JiqiaoStageIssuedEvent(f.Id, r.Stage, r.Before, r.After, r.ActualCount));
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 22500)]
    internal bool ResumePaidJiqiao(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { JiqiaoPaid: { } r } f || f.Id != id)
            return false;
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id))
            return true;
        var before = _coreCardMovementJournalMovementSequenceAccess._movementSequence;
        if (r.Stage == JiqiaoPaidStage.CostChildren)
        {
            if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            {
                CompletePaidJiqiao(f);
                return true;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { JiqiaoPaid = r with { Stage = JiqiaoPaidStage.RevealChildren, Before = before, After = before, ActualCount = 0 } });
            _coreProgramLifecycleOperations.RevealProgramTopCards(id, f.OwnerSeat, checked(r.Invoice.PaidCount * 2), null, r.Invoice.RevealBind, SkillProgramCardSetVisibility.Public);
            _coreProgramLifecycleOperations.FilterProgramBoundCards(id, r.Invoice.RevealBind, r.Invoice.RevealBind + ".non-equipment", [], null, [SkillProgramCardCategory.Basic, SkillProgramCardCategory.Trick], [], [], null);
            var count = _coreProgramLifecycleOperations.GetProgramCardSet(_coreProgramLifecycleOperations.GetActiveProgramFrame(id), r.Invoice.RevealBind).CardIds.Count;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { JiqiaoPaid = f.JiqiaoPaid!with { After = _coreCardMovementJournalMovementSequenceAccess._movementSequence, ActualCount = count, RevealedCount = count } });
            IssueJiqiaoStage(f);
            _coreRuntimeOperations.AdvanceRuntimeProgram(id);
            return true;
        }

        var remaining = _coreProgramLifecycleOperations.GetProgramCardSet(f, r.Invoice.RevealBind).CardIds.Where(cid => _cardZonesOperations._cardZones.GetLocation(cid) == CardLocation.Processing).ToArray();
        if (r.Stage == JiqiaoPaidStage.RevealChildren)
        {
            var claim = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive ? remaining.Where(cid => !_coreEquipmentContentOperations.EquipmentRules.IsEquipment(JiqiaoCard(cid).Kind)).ToArray() : [];
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { JiqiaoPaid = r with { Stage = JiqiaoPaidStage.ClaimChildren, Before = before, After = before, ActualCount = claim.Length, ClaimedCount = claim.Length } });
            if (claim.Length > 0)
                _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(claim, CardLocation.Hand(f.OwnerSeat), new(JiqiaoClaimReason));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { JiqiaoPaid = f.JiqiaoPaid!with { After = _coreCardMovementJournalMovementSequenceAccess._movementSequence } });
            IssueJiqiaoStage(f);
            _coreRuntimeOperations.AdvanceRuntimeProgram(id);
            return true;
        }

        if (r.Stage == JiqiaoPaidStage.ClaimChildren)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { JiqiaoPaid = r with { Stage = JiqiaoPaidStage.CleanupChildren, Before = before, After = before, ActualCount = remaining.Length } });
            if (remaining.Length > 0)
                _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(remaining, CardLocation.DiscardPile, new(JiqiaoRemainderReason));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { JiqiaoPaid = f.JiqiaoPaid!with { After = _coreCardMovementJournalMovementSequenceAccess._movementSequence } });
            IssueJiqiaoStage(f);
            _coreRuntimeOperations.AdvanceRuntimeProgram(id);
            return true;
        }

        CompletePaidJiqiao(f);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 22500)]
    internal bool ReturnPaidJiqiaoMovement(ProgramSkillFrame f) => f.JiqiaoPaid is not null && ResumePaidJiqiao(f.Id);
    internal void CompletePaidJiqiao(ProgramSkillFrame f)
    {
        var r = f.JiqiaoPaid!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JiqiaoCompletedEvent(f.Id, r.Invoice.PaidCount, r.RevealedCount, r.ClaimedCount, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { JiqiaoPaid = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }
}
