#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShownBoundGiftsRuntime
{

    SkillProgramStepOutcome IShownBoundGiftProgramHost.GiveShownBoundCardsAndGrantTurnHandLimit(ProgramSkillFrame f, string bind) => GiveProgramShownBoundGift(f, bind);
    internal SkillProgramStepOutcome GiveProgramShownBoundGift(ProgramSkillFrame frame, string bind)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        _coreCardUseModulesOperations.ValidateProgramTurnEffectGrant(f);
        if (f.ShownGiftReceipt is not null)
            throw new InvalidOperationException("A shown gift cannot be paid twice.");
        if (f.SelectedTargetSeats.Count != 1)
            throw new InvalidOperationException("A shown gift requires one recipient.");
        var recipient = f.SelectedTargetSeats[0];
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(recipient) || recipient == f.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[recipient].IsAlive)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "受牌角色失效，赠牌已取消。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        var source = _coreProgramLifecycleOperations.GetProgramCardSet(f, bind);
        if (source.Visibility != SkillProgramCardSetVisibility.Public || source.CardIds.Count == 0 || source.CardIds.Count != source.SourceLocations.Count || source.CardIds.Distinct().Count() != source.CardIds.Count || source.SourceLocations.Any(l => l.OwnerSeat != f.OwnerSeat || l.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment)) || source.CardIds.Where((id, i) => _cardZonesOperations._cardZones.GetLocation(id) != source.SourceLocations[i]).Any())
            throw new InvalidOperationException("A shown gift lost its exact public owned entities.");
        if (_coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat || _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.TurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber)
            throw new InvalidOperationException("A shown gift lost its actual turn.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(source.CardIds, CardLocation.Hand(recipient), new($"skill-program.{(f.SkillId)}.{(SkillProgramEffectOp.GiveShownBoundCardsAndGrantTurnHandLimit)}"), _characterHuangQuanMouOperations.NativeGiveCompletionRecords(f.Id, f.OwnerSeat, recipient, source.CardIds, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, (batchId, movements) =>
        {
            var actual = movements.Where(m => m.To == CardLocation.Hand(recipient) && m.From != m.To).Select(m => new ProgramShownGiftCard(m.CardId, CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(m.CardKind), m.Sequence, m.From, m.To)).ToArray();
            var x = actual.Select(c => c.Category).Distinct().Count();
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var receipt = new ProgramShownGiftReceipt(f.InstructionIndex - 1, batchId, recipient, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, Array.AsReadOnly(actual), null);
            _coreRuntimeOperations.ReplaceRuntimeTop(active with { ShownGiftReceipt = receipt });
            if (x > 0)
            {
                var modifier = _turnCardUseEffectsOperations._turnCardUseEffects.GrantRuleModifier(receipt.ActualTurnNumber, receipt.ActualTurnOwnerSeat, f.Id, receipt.InstructionIndex, CoreGameEngineCardUseModulesRuntime.CreateProgramTurnEffectSource(f), SkillRuleQuery.HandLimit, SkillRuleOperation.Add, x, affectedSeat: receipt.ActualTurnOwnerSeat);
                _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { ShownGiftReceipt = receipt with { ModifierGrantSequence = modifier.GrantSequence } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TurnRuleModifierGrantedEvent(modifier));
            }

            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShownBoundGiftCommittedEvent(f.Id, f.SkillId, f.OwnerSeat, recipient, receipt.ActualTurnNumber, receipt.ActualTurnOwnerSeat, actual.Length, x));
        }));
        return _coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(f.Id, f.OwnerSeat);
    }
}
