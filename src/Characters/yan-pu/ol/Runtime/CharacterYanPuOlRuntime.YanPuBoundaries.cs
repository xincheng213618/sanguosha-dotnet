#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYanPuOlRuntime
{
    internal static CardConversionSource YanPuSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal bool IsYanPuEndingCandidate(ProgramTriggerCandidate c) => _coreProgramLifecycleOperations.GetProgramTrigger2(c).Effects is [{ Op: SkillProgramEffectOp.ResolveHuantuEnding }];
    internal IReadOnlyList<ProgramTriggerCandidate> IncludeYanPuDrawObservers(CharacterState subject, IReadOnlyList<ProgramTriggerCandidate> existing)
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.GiveCardSkipDrawForEnding))
            return existing;
        var extra = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != subject.Seat).SelectMany(p => _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(p, SkillProgramTriggerWindow.DrawPhaseStarting)).Where(c => _coreProgramLifecycleOperations.GetProgramTrigger2(c).Effects is [{ Op: SkillProgramEffectOp.GiveCardSkipDrawForEnding }]);
        return Array.AsReadOnly(existing.Concat(extra).Distinct().OrderByDescending(c => c.Priority).ThenBy(c => _coreTableOrderOperations.RelativeTableOrder(subject.Seat, c.OwnerSeat)).ThenBy(c => c.SkillId, StringComparer.Ordinal).ToArray());
    }

    internal bool YanPuRoundUsed(int seat, string skill) => _coreProgramEventHistoryOperations.ProgramEventHistory<YanPuHuantuIssuedEvent>().Any(e => e.Promise.Source.OwnerSeat == seat && e.Promise.Source.SkillId == skill && e.Promise.ActualRound == _coreActualRoundProgressRoundNumberAccess._roundNumber);
    [ProgramTriggerProof(ProgramTriggerProofPhase.ForeignDraw, 10)]
    internal bool IsYanPuDrawContext(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _coreProgramLifecycleOperations.GetProgramTrigger2(c).Effects is [{ Op: SkillProgramEffectOp.GiveCardSkipDrawForEnding }] && context.Window == SkillProgramTriggerWindow.DrawPhaseStarting && context.SourceSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && context.TargetSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && context.OwnerSeat == c.OwnerSeat && c.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Draw && _coreActualRoundProgressRoundNumberAccess._roundNumber > 0 && _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat].IsAlive && !YanPuRoundUsed(c.OwnerSeat, c.SkillId) && _coreProgramDodgeResponsesOperations.IsWithinAttackRange(c.OwnerSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat) && _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is { Window: SkillProgramTriggerWindow.DrawPhaseStarting, ActualPhaseSubstitution: null } parent && parent.OwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && parent.CandidateIndex >= 0 && parent.CandidateIndex < parent.Candidates.Count && parent.Candidates[parent.CandidateIndex] == c && (_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat]).Count > 0 || _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat]).Any(card => !_coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(c.OwnerSeat, c.SkillId, c.SkillInstanceId, card) && !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(c.OwnerSeat, card, CardLocation.Equipment(c.OwnerSeat), OwnedCardMoveIntent.Transfer)));
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.Collection, 26410)]
    internal bool CollectYanPuCandidate(SkillProgramTrigger t) => t.Effects is not [{ Op: SkillProgramEffectOp.ResolveHuantuEnding }];
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 26410)]
    internal bool QualifyYanPuCandidate(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => t.Effects is [{ Op: SkillProgramEffectOp.GiveCardSkipDrawForEnding }] ? IsYanPuDrawContext(c, context) : t.Effects is [{ Op: SkillProgramEffectOp.DrawDyingExitRoundDistance }] ? IsYanPuDyingExitContext(c, context) : true;
    internal bool ValidYanPuPromise(YanPuHuantuPromise p) => p.Id > 0 && p.Source.OwnerSeat != p.RecipientSeat && p.ActualRound > 0 && p.ActualRound <= _coreActualRoundProgressRoundNumberAccess._roundNumber && p.ActualTurn > 0 && p.ActualTurnOwner == p.RecipientSeat && p.From.OwnerSeat == p.Source.OwnerSeat && p.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && _contentRegistryOperations._contentRegistry.GetSkill(p.Source.SkillId).Program?.GameplayHash == p.GameplayHash && _coreProgramEventHistoryOperations.ProgramEventHistory<YanPuHuantuIssuedEvent>().Count(e => e.Promise.Id == p.Id) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<YanPuHuantuIssuedEvent>().Any(e => e.Promise == p) && _coreProgramEventHistoryOperations.ProgramEventHistory<YanPuStartedEvent>().Count(e => e.ProgramFrameId == p.Id && e.Source == p.Source && e.GameplayHash == p.GameplayHash && e.WindowId == p.DrawWindowId && e.Round == p.ActualRound && e.Turn == p.ActualTurn && e.TurnOwner == p.ActualTurnOwner && e.Target == p.RecipientSeat && e.Stage == YanPuStage.GiftChoice) == 1 && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence == p.MovementSequence && m.CardId == p.CardId && m.From == p.From && m.To == CardLocation.Hand(p.RecipientSeat) && m.Reason.Value == YanPuGiftReason) == 1;
    internal bool YanPuPromiseConsumed(long id) => _coreProgramEventHistoryOperations.ProgramEventHistory<YanPuHuantuConsumedEvent>().Any(e => e.PromiseId == id);
    internal void AppendYanPuEndingItems(List<TurnEndingBoundaryItem> items)
    {
        foreach (var p in _coreProgramEventHistoryOperations.ProgramEventHistory<YanPuHuantuIssuedEvent>().Select(e => e.Promise).Where(p => p.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && p.ActualTurnOwner == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && !YanPuPromiseConsumed(p.Id)).ToArray())
        {
            if (!ValidYanPuPromise(p) || !_coreMatchParticipantsPlayersAccess._players[p.Source.OwnerSeat].IsAlive)
                continue;
            var t = _contentRegistryOperations._contentRegistry.GetSkill(p.Source.SkillId).Program!.Triggers.Single(t => t.Effects is [{ Op: SkillProgramEffectOp.ResolveHuantuEnding }]);
            var c = new ProgramTriggerCandidate(p.Source.OwnerSeat, p.Source.SkillId, t.Id, p.Source.SkillInstanceId, p.GameplayHash, t.Priority, items.Count);
            items.Add(new(TurnEndingBoundaryItemKind.Program, t.Priority, $"yanpu-ending:{(p.Id)}", c, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat])));
        }
    }

    internal YanPuHuantuPromise? CurrentYanPuEnding(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (!IsYanPuEndingCandidate(c) || context.Window != SkillProgramTriggerWindow.TurnEnding || context.SourceSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || context.TargetSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || context.OwnerSeat != c.OwnerSeat || context.OccurrenceIndex != c.OccurrenceIndex || _resolutionStackOperations._resolutionStack.OfType<TurnEndingBoundaryFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { } w || w.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || w.TurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || w.ItemIndex < 0 || w.ItemIndex >= w.Items.Count || w.Items[w.ItemIndex].Candidate != c)
            return null;
        var p = _coreProgramEventHistoryOperations.ProgramEventHistory<YanPuHuantuIssuedEvent>().Select(e => e.Promise).SingleOrDefault(p => w.Items[w.ItemIndex].StableIdentity == $"yanpu-ending:{(p.Id)}");
        return p is not null && ValidYanPuPromise(p) && p.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && p.ActualTurnOwner == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && p.Source.OwnerSeat == c.OwnerSeat && p.Source.SkillId == c.SkillId && p.Source.SkillInstanceId == c.SkillInstanceId && p.GameplayHash == c.GameplayHash ? p : null;
    }

    internal bool CanRunYanPuEnding(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => CurrentYanPuEnding(c, context)is { } p && !YanPuPromiseConsumed(p.Id) && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive;
    internal ProgramTriggerCandidate? YanPuCurrentEndingFrameCandidate(ProgramSkillFrame f) => f.WindowContext is { Window: SkillProgramTriggerWindow.TurnEnding } context && _resolutionStackOperations._resolutionStack.OfType<TurnEndingBoundaryFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is { } w && w.ItemIndex >= 0 && w.ItemIndex < w.Items.Count && w.Items[w.ItemIndex].Candidate is { } c && context.OccurrenceIndex == c.OccurrenceIndex && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, c) ? c : null;
    internal bool TryBeginYanPuDyingExit(GameEngine.DyingCompletionReceipt dying, bool survived)
    {
        if (!survived || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[dying.VictimSeat].IsAlive || _coreMatchParticipantsPlayersAccess._players[dying.VictimSeat].Hp <= 0 || !(_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.DrawDyingExitRoundDistance) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveQingliuFactionChoice) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMouJiangWeiFuzhan) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerWindow(SkillProgramTriggerWindow.DyingExited) || _coreWushuangModeProjectionOperations.IsWushuangMode))
            return false;
        var facts = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).ToDictionary(p => p.Seat, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts);
        var candidates = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).SelectMany(p => _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(p, SkillProgramTriggerWindow.DyingExited)).Where(c => _coreProgramLifecycleOperations.GetProgramTrigger2(c).Effects is [{ Op: SkillProgramEffectOp.DrawDyingExitRoundDistance or SkillProgramEffectOp.ResolveQingliuFactionChoice or SkillProgramEffectOp.ResolveMouJiangWeiFuzhan }] || _coreWushuangDyingExitOperations.IsWushuangDyingExitCandidate(c, dying.VictimSeat) || _characterGuanYuHappyGodOperations.IsHappyGuanYuDyingExitCandidate(c, dying.VictimSeat) || _coreShanheYongzhanOperations.IsShanheYongzhanDyingExitCandidate(c, dying.VictimSeat)).OrderBy(c => _coreTableOrderOperations.RelativeTableOrder(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, c.OwnerSeat)).ThenByDescending(c => c.Priority).ToArray();
        if (candidates.Length == 0)
            return false;
        var ret = new YanPuDyingExitReturn(dying.FrameId, dying.ParentFrameId, dying.VictimSeat, dying.Continuation, _coreActualRoundProgressRoundNumberAccess._roundNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat);
        _coreRuntimeOperations.PushRuntimeFrame(new ProgramLifecycleTriggerWindowFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, dying.VictimSeat, SkillProgramTriggerWindow.DyingExited, Array.AsReadOnly(candidates), ProgramLifecycleContinuation.ResumeYanPuDyingExit, facts[dying.VictimSeat]) { ParticipantFacts = facts, YanPuDyingExit = ret });
        _coreRuntimeOperations.AdvanceRuntimeTop<ProgramLifecycleTriggerWindowFrame>();
        return true;
    }

    internal void ReturnYanPuDyingExit(ProgramLifecycleTriggerWindowFrame frame)
    {
        var ret = frame.YanPuDyingExit ?? throw new InvalidOperationException("YanPu exit lost its original completed Dying return.");
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != ret.DyingParentFrameId || ret.Round != _coreActualRoundProgressRoundNumberAccess._roundNumber || ret.Turn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || ret.TurnOwner != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat)
            throw new InvalidOperationException("YanPu exit lost its exact paused native producer.");
        var dying = new GameEngine.DyingCompletionReceipt(ret.DyingFrameId, ret.DyingParentFrameId, ret.VictimSeat, ret.Continuation);
        if (_coreDeferredBenefitsProgramsOperations.TryBeginFactionRecoveryDebts(dying, true))
            return;
        _coreNativeDyingOperations.ContinueDyingAfterFactionRecoveryDebts(dying, true);
    }

    [ProgramIssuedTrigger(680, ProgramIssuedTriggerMatch.Decision)]
    internal bool? DecideIssuedTrigger680(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => IsYanPuEndingCandidate(candidate) ? CanRunYanPuEnding(candidate, context) : null;
}
