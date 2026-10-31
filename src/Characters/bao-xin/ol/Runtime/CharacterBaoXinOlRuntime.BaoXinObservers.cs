#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterBaoXinOlRuntime
{
    internal bool ValidBaoXin(ProgramSkillFrame f)
    {
        if (f.BaoXin is not { } r)
            return false;
        if (f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || r.Source != BaoXinSource(f) || r.GameplayHash != f.GameplayHash || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != r.GameplayHash || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != r.Op || !BaoXinComposition.Handles(r.Op) || !Enum.IsDefined(r.Stage) || r.ActualTurn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber || r.TurnOwner != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.Holder) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.Recipient) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.Cursor) || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.Original.Select(c => c.CardId).Distinct().Count() != r.Original.Count || r.Moving.Select(c => c.CardId).Distinct().Count() != r.Moving.Count || r.Consumed.Distinct().Count() != r.Consumed.Count || r.Consumed.Any(id => !r.Original.Any(c => c.CardId == id)) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<BaoXinStartedEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.Op == r.Op && e.ActualTurn == r.ActualTurn && e.TurnOwner == r.TurnOwner && e.Holder == r.Holder) != 1)
            return false;
        if (r.Op == SkillProgramEffectOp.ResolveBaoXinMutao)
        {
            if (f.SkillId != "ol:mutao" || f.TriggerId is not null || CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f) != "public-slash-ring" || f.SelectedTargetSeats is not [var holder] || holder != r.Holder || !r.Paid || r.Qualification is not null || r.Pile is not { } pile || pile != new PublicPersistentPileSource(r.Holder, "ol:mutao", "bao-xin:" + f.Id, 0, "bao-xin.mutao." + f.Id) || !IsIssuedBaoXinMutaoPile(pile) || r.Original.Any(c => c.From != CardLocation.Hand(r.Holder) || !CoreNativeCardClassificationsRuntime.IsSlashCard(c.EffectiveKind)))
                return false;
            foreach (var(id, index)in r.Original.Select((c, index) => (c, index)))
                if (_coreProgramEventHistoryOperations.Value.ProgramEventHistory<BaoXinOriginalEntityEvent>().Count(e => e.FrameId == f.Id && e.Index == index && e.CardId == id.CardId && e.Kind == id.Kind && e.EffectiveKind == id.EffectiveKind && e.From == id.From) != 1)
                    return false;
            foreach (var id in r.Consumed)
            {
                var gift = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<BaoXinMutaoGiftEvent>().Where(e => e.FrameId == f.Id && e.CardId == id).ToArray();
                var retired = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<BaoXinMutaoRetiredEvent>().Where(e => e.FrameId == f.Id && e.CardId == id).ToArray();
                if (gift.Length + retired.Length != 1 || gift.Any(e => !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == e.Sequence && m.CardId == id && m.From == pile.Location && m.To == CardLocation.Hand(e.Recipient) && m.Reason.Value == BaoXinGift)) || retired.Any(e => !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == e.DepartureSequence && m.CardId == id && m.From == pile.Location)))
                    return false;
            }

            var gifts = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<BaoXinMutaoGiftEvent>().Where(e => e.FrameId == f.Id).OrderBy(e => e.Ordinal).ToArray();
            if (gifts.Length != r.GiftOrdinal || gifts.Select(e => e.Ordinal).Where((n, i) => n != i + 1).Any() || r.LastRecipient != (gifts.LastOrDefault()?.Recipient) || r.Cursor != (gifts.LastOrDefault()?.Recipient ?? r.Holder))
                return false;
        }
        else
        {
            if (f.SkillId != "ol:yimou" || CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f) != "near-damage" || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { } context || r.Qualification is not { } q || r.Holder != q.VictimSeat || BaoXinQualification(new(f.OwnerSeat, f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex), context) != q || r.Pile is not null || r.Original.Count != 0)
                return false;
            var branch = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<BaoXinYimouBranchEvent>().Where(e => e.FrameId == f.Id).ToArray();
            if (r.Paid ? branch is not [var selected] || selected.Branch != r.Branch || selected.VictimSeat != r.Holder : r.Stage != BaoXinStage.Mode || branch.Length != 0)
                return false;
        }

        if (r.SelectedCard is { } chosen && r.Eligible.Count > 0)
        {
            var ordinal = r.Op == SkillProgramEffectOp.ResolveBaoXinMutao ? r.GiftOrdinal : 0;
            var candidates = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<BaoXinRandomCandidateEvent>().Where(e => e.FrameId == f.Id && e.Ordinal == ordinal).OrderBy(e => e.Index).ToArray();
            var random = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<BaoXinRandomIssuedEvent>().Where(e => e.FrameId == f.Id && e.Ordinal == ordinal).ToArray();
            var replay = new DeterministicRandom(unchecked((int)r.RandomBefore));
            var result = replay.Next(r.Eligible.Count);
            if (candidates.Select(e => e.CardId).SequenceEqual(r.Eligible) == false || candidates.Where((e, i) => e.Index != i).Any() || r.RandomIndex != result || replay.State != r.RandomAfter || r.Eligible[r.RandomIndex] != chosen || random is not [var draw] || draw.CardId != chosen || draw.Recipient != r.Recipient || draw.Index != r.RandomIndex || draw.CandidateCount != r.Eligible.Count || draw.Before != r.RandomBefore || draw.After != r.RandomAfter)
                return false;
        }

        var moving = r.Stage is BaoXinStage.StoreChildren or BaoXinStage.GiftChildren or BaoXinStage.CleanupChildren or BaoXinStage.ObtainChildren or BaoXinStage.YimouGiftChildren or BaoXinStage.DrawChildren;
        if (moving)
        {
            if (!r.Paid || f.PendingMovementContinuation is not { } pending || !IsBaoXinMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), pending) || r.Step <= 0 || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<BaoXinMovementPaidEvent>().Count(e => e.FrameId == f.Id && e.Step == r.Step && e.Stage == r.Stage && e.Before == r.Before && e.After == r.After && e.Count == r.ActualCount && e.Recipient == r.Recipient) != 1)
                return false;
            var records = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<BaoXinEntityMovedEvent>().Where(e => e.FrameId == f.Id && e.Step == r.Step).ToArray();
            if (records.Length != (r.Stage == BaoXinStage.DrawChildren ? 0 : r.ActualCount) || records.Any(e => !r.Moving.Any(c => c.CardId == e.CardId && c.Kind == e.Kind && c.From == e.From) || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == e.Sequence && m.CardId == e.CardId && m.CardKind == e.Kind && m.From == e.From && m.To == e.To && BaoXinMovementMatches(f, m))))
                return false;
        }
        else if (f.PendingMovementContinuation is not null)
            return false;
        if (r.Stage == BaoXinStage.DamageChildren && (!r.DamageIssued || r.LastRecipient is not { } target || r.Damage is < 1 or > 2 || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<BaoXinMutaoDamageIssuedEvent>().Count(e => e.FrameId == f.Id && e.SourceSeat == r.Holder && e.TargetSeat == target && e.Amount == r.Damage && e.Amount == Math.Min(2, e.SlashCount)) != 1 || f.AttackAttempt is { } attack && (attack.SourceSeat != r.Holder || attack.TargetSeat != target || attack.DamageAmount < 0 || attack.SourceLess)))
            return false;
        return true;
    }
    internal bool IsBaoXinMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => f.BaoXin is { Paid: true } r && effect?.Op == r.Op && pending == f.PendingMovementContinuation && pending.SubjectSeat == r.Recipient && pending.BeforeCount == 0 && pending.CoverageResultBind is null;
    internal bool BaoXinMovementMatches(ProgramSkillFrame f, CardMovementRecord m)
    {
        var r = f.BaoXin!;
        if (m.Sequence <= r.Before || m.Sequence > r.After || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m))
            return false;
        return r.Stage switch
        {
            BaoXinStage.StoreChildren => m.From == CardLocation.Hand(r.Holder) && m.To == r.Pile!.Location && m.Reason.Value == BaoXinStore && r.Original.Any(c => c.CardId == m.CardId && c.Kind == m.CardKind),
            BaoXinStage.GiftChildren => m.From == r.Pile!.Location && m.To == CardLocation.Hand(r.Recipient) && m.Reason.Value == BaoXinGift && m.CardId == r.SelectedCard,
            BaoXinStage.CleanupChildren => m.From == r.Pile!.Location && m.To == CardLocation.DiscardPile && m.Reason.Value == BaoXinCleanup && r.Original.Any(c => c.CardId == m.CardId),
            BaoXinStage.ObtainChildren => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(r.Holder) && m.Reason.Value == BaoXinObtain && m.CardId == r.SelectedCard,
            BaoXinStage.YimouGiftChildren => m.From == CardLocation.Hand(r.Holder) && m.To == CardLocation.Hand(r.Recipient) && m.Reason.Value == BaoXinYimouGift && m.CardId == r.SelectedCard,
            BaoXinStage.DrawChildren => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(r.Holder) && m.Reason.Value == BaoXinDraw || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle,
            _ => false
        };
    }

    internal bool BaoXinFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.BaoXin is not { Paid: true } r || !ValidBaoXin(f))
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return f.PendingMovementContinuation is not null && CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => BaoXinMovementMatches(f, m));
        if (child is DamageFrame damage)
            return r.Stage == BaoXinStage.DamageChildren && damage.ParentFrameId == f.Id && f.AttackAttempt is { } attempt && damage.SourceSeat == attempt.SourceSeat && damage.TargetSeat == attempt.TargetSeat && damage.Amount == attempt.DamageAmount;
        if (child is BeforeDamageProgramWindowFrame before)
            return r.Stage == BaoXinStage.DamageChildren && (before.ContinuationAttackResolutionId ?? before.ParentFrameId) == f.Id && f.AttackAttempt is not null;
        return child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal ProgramSkillFrame? BaoXinObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => this.BaoXinFirstChild(f, c), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27450)]
    internal bool AdvanceBaoXinChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && BaoXinObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
