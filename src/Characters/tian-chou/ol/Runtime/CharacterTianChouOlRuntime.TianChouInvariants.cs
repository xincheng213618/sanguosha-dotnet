#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterTianChouOlRuntime
{
    internal bool ValidTianChou(ProgramSkillFrame f)
    {
        if (f.TianChouShandao is not { } r || f.SkillId != "ol:shandao" || f.ActivationId != "top-and-two-tricks" || f.TriggerId is not null || f.WindowContext is not null || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.PendingMovementContinuation is not null || f.InstructionIndex != 1 || r.InstructionIndex != 1 || r.Source != TianChouSource(f) || r.GameplayHash != f.GameplayHash || r.ActualTurn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || r.StartedAt < 0 || r.StartedAt > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || !Enum.IsDefined(r.Stage) || r.Cursor < 0 || r.Cursor > r.Targets.Count || r.TrickIndex is < 0 or > 2 || r.Candidates.Count == 0 || r.Candidates.Any(s => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(s)) || r.Candidates.Distinct().Count() != r.Candidates.Count || r.Targets.Distinct().Count() != r.Targets.Count || r.Targets.Any(s => !r.Candidates.Contains(s)) || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != SkillProgramEffectOp.ResolveShandaoTopAndGlobalTricks || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<TianChouShandaoStartedEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var started] || started != new TianChouShandaoStartedEvent(f.Id, r.Source, r.GameplayHash, r.ActualTurn, r.PhaseInstanceId, r.StartedAt, r.Candidates.Count) || !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<TianChouShandaoCandidateEvent>().Where(e => e.FrameId == f.Id).OrderBy(e => e.Index).Select(e => e.Seat).SequenceEqual(r.Candidates))
            return false;
        var selected = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<TianChouShandaoSelectedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (selected.Length != r.Targets.Count || selected.Select(e => e.Index).Where((n, i) => n != i).Any() || !selected.Select(e => e.Seat).Order().SequenceEqual(r.Targets.Order()))
            return false;
        var frozen = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<TianChouShandaoTargetsFrozenEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (r.TargetsFrozen ? frozen is not [var freeze] || freeze.Count != r.Targets.Count || r.Targets.Count == 0 || r.Stage == TianChouShandaoStage.Targets || !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<TianChouShandaoTargetOrderedEvent>().Where(e => e.FrameId == f.Id).OrderBy(e => e.Index).Select(e => e.Seat).SequenceEqual(r.Targets) || !Enumerable.Range(0, _coreMatchDefinitionPlayerCountAccess._playerCount).Select(i => _coreTableOrderOperations.TableStepSeat(f.OwnerSeat, i)).Where(r.Targets.Contains).SequenceEqual(r.Targets) : frozen.Length != 0 || r.Stage != TianChouShandaoStage.Targets || r.Cursor != 0 || r.Tops.Count != 0 || r.TrickIndex != 0)
            return false;
        var tops = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<TianChouShandaoTopPaidEvent>().Where(e => e.FrameId == f.Id).ToArray();
        var skipped = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<TianChouShandaoSkippedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (tops.Length != r.Tops.Count || r.Tops.Select(t => t.Index).Distinct().Count() != r.Tops.Count || skipped.Select(t => t.Index).Distinct().Count() != skipped.Length || skipped.Any(s => s.Index < 0 || s.Index >= r.Targets.Count || s.OwnerSeat != r.Targets[s.Index] || r.Tops.Any(t => t.Index == s.Index)))
            return false;
        for (var i = 0; i < r.Tops.Count; i++)
        {
            var t = r.Tops[i];
            if (t.Index < 0 || t.Index >= r.Targets.Count || t.OwnerSeat != r.Targets[t.Index] || t.From.OwnerSeat != t.OwnerSeat || t.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || t.CardId <= 0 || t.Before < r.StartedAt || t.After <= t.Before || t.Sequence <= t.Before || t.Sequence > t.After || t.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || i > 0 && t.Before < r.Tops[i - 1].After || t.To != (t.Kind == CardKind.GeneralWeapon && t.From.Zone == CardZoneKind.Equipment ? CardLocation.OutsideGame : CardLocation.DrawPile) || tops[i] != new TianChouShandaoTopPaidEvent(f.Id, t.Index, t.OwnerSeat, t.CardId, t.Kind, t.From, t.To, t.Before, t.After, t.Sequence) || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence == t.Sequence && m.CardId == t.CardId && m.CardKind == t.Kind && m.From == t.From && m.To == t.To && m.Reason.Value == TianChouTopReason) != 1 || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > t.Before && m.Sequence <= t.After).Any(m => !IsTianChouTopMovement(t, m)))
                return false;
        }

        var processed = r.Tops.Select(t => t.Index).Concat(skipped.Select(t => t.Index)).Order().ToArray();
        var revealing = _characterXiaoQiaoMouOlOperations.IsPublicHandRevealProducerSuspended(f.Id, "shandao-hand-top") && f.PublicHandRevealSelection is { ReturnKey: "shandao-hand-top" } selectedCard && selectedCard.InstructionIndex == f.InstructionIndex && r.Stage == TianChouShandaoStage.TopChildren;
        var processedCount = r.Cursor + (r.Stage == TianChouShandaoStage.TopChildren && !revealing ? 1 : 0);
        if (!processed.SequenceEqual(Enumerable.Range(0, processedCount)) || r.Stage == TianChouShandaoStage.TopChildren && !revealing && (r.Tops.Count == 0 || r.Tops[^1].Index != r.Cursor))
            return false;
        var issued = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<TianChouShandaoNativeIssuedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        var returned = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<TianChouShandaoNativeReturnedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        var noTargets = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<TianChouShandaoTrickSkippedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (issued.Length > 2 || issued.Select(e => e.UseId).Distinct().Count() != issued.Length || issued.Select(e => e.Kind).Distinct().Count() != issued.Length || issued.Any(e => e.ActorSeat != f.OwnerSeat || e.TargetCount < 1 || e.Kind is not (CardKind.FiveGrains or CardKind.ArrowBarrage)) || returned.Select(e => e.UseId).Distinct().Count() != returned.Length || returned.Any(e => !issued.Any(i => i.UseId == e.UseId && i.Kind == e.Kind)) || noTargets.Select(e => e.Kind).Distinct().Count() != noTargets.Length || noTargets.Any(e => e.Kind is not (CardKind.FiveGrains or CardKind.ArrowBarrage) || issued.Any(i => i.Kind == e.Kind)) || r.TrickIndex != returned.Length + noTargets.Length)
            return false;
        foreach (var use in issued)
        {
            var targets = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<TianChouShandaoNativeTargetEvent>().Where(e => e.FrameId == f.Id && e.UseId == use.UseId).OrderBy(e => e.Index).ToArray();
            if (targets.Length != use.TargetCount || targets.Select(t => t.Index).Where((n, i) => n != i).Any() || targets.Select(t => t.TargetSeat).Distinct().Count() != targets.Length || targets.Any(t => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(t.TargetSeat) || (use.Kind == CardKind.FiveGrains ? !r.Targets.Contains(t.TargetSeat) : t.TargetSeat == f.OwnerSeat || r.Targets.Contains(t.TargetSeat))))
                return false;
        }

        if (r.Stage is TianChouShandaoStage.NativeUse or TianChouShandaoStage.NativeChildren)
        {
            if (r.NativeUse is not { } use || use.ProgramFrameId != f.Id || use.InstructionIndex != r.InstructionIndex || use.Source != r.Source || use.GameplayHash != r.GameplayHash || use.ActualTurn != r.ActualTurn || use.PhaseInstanceId != r.PhaseInstanceId || use.ActorSeat != f.OwnerSeat || issued.Count(e => e.UseId == use.CardUseFrameId && e.Kind == use.CardKind && e.TargetCount == use.TargetSeats.Count) != 1 || !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<TianChouShandaoNativeTargetEvent>().Where(e => e.FrameId == f.Id && e.UseId == use.CardUseFrameId).OrderBy(e => e.Index).Select(e => e.TargetSeat).SequenceEqual(use.TargetSeats))
                return false;
            if (r.Stage == TianChouShandaoStage.NativeUse ? issued.Length != returned.Length + 1 || returned.Any(e => e.UseId == use.CardUseFrameId) : issued.Length != returned.Length || !returned.Any(e => e.UseId == use.CardUseFrameId))
                return false;
        }
        else if (r.NativeUse is not null || issued.Length != returned.Length)
            return false;
        return true;
    }

    internal bool IsTianChouTopMovement(TianChouShandaoTop t, CardMovementRecord m) => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > t.Before && m.Sequence <= t.After && (m.Sequence == t.Sequence && m.CardId == t.CardId && m.CardKind == t.Kind && m.From == t.From && m.To == t.To && m.Reason.Value == TianChouTopReason || t.Kind == CardKind.WoodenOx && t.From == CardLocation.Equipment(t.OwnerSeat) && t.WoodenOxGrainIds.Contains(m.CardId) && m.From == CardLocation.WoodenOxGrain(t.OwnerSeat) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.WoodenOxGrainDiscard);
}
