#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuBaOlRuntime
{
    internal void PayTongduGift(ProgramSkillFrame f, LiuBaEntity card)
    {
        var r = f.LiuBa!;
        var entry = _characterLuoXianOlOperations.CurrentHandEntry(r.Recipient, card.CardId) ?? throw new InvalidOperationException("Tongdu requires the selected original Hand entry.");
        PrepareLiuBaMovement(ref f, LiuBaStage.GiftChildren, [card]);
        if (r.Recipient == f.OwnerSeat)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TongduGiftIssuedEvent(f.Id, r.Source, r.GameplayHash, r.ActualTurn, r.TurnOwner, r.Recipient, card.CardId, card.Kind, entry, null));
        else
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([card.CardId], CardLocation.Hand(f.OwnerSeat), new(TongduGiftReason), _characterHuangQuanMouOperations.NativeGiveCompletionRecords(f.Id, r.Recipient, f.OwnerSeat, [card.CardId], _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, (batch, records) =>
            {
                RecordLiuBaMovement(f, batch, records);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TongduGiftIssuedEvent(f.Id, r.Source, r.GameplayHash, r.ActualTurn, r.TurnOwner, r.Recipient, card.CardId, card.Kind, entry, records.Single().Sequence));
            }));
        CompleteLiuBaMovement(f, r.Recipient == f.OwnerSeat ? 0 : 1);
    }

    internal void PayTongduReturn(ProgramSkillFrame f)
    {
        var r = f.LiuBa!;
        var c = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, f.WindowContext!.OccurrenceIndex);
        var debt = CurrentTongduDebt(c, f.WindowContext!) ?? throw new InvalidOperationException("Tongdu lost its original issued entity debt.");
        var material = LiuBaOwnedEntities(f.OwnerSeat).Where(e => e.CardId == debt.CardId).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LiuBa = r with { DebtId = debt.FrameId } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TongduDebtConsumedEvent(debt.FrameId, f.Id, r.ParentId, r.ActualTurn, r.PhaseInstance));
        PrepareLiuBaMovement(ref f, LiuBaStage.ReturnChildren, material);
        if (material.Length != 0)
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([debt.CardId], CardLocation.DrawPile, new(TongduReturnReason), (batch, records) => RecordLiuBaMovement(f, batch, records));
        CompleteLiuBaMovement(f, material.Length);
    }
}
