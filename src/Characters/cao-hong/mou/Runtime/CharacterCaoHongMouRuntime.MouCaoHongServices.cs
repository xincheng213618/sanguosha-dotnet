#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoHongMouRuntime
{
    internal MouCaoHongRuntime MouCaoHong => _mouCaoHongOperations.Get();

    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeOwnerFacts, 35900, SkillProgramEffectOp.ResolveMouCaoHongXianju)]
    internal bool MouCaoHongCanActivate(ProgramActivationLegalityContext c) => c.SkillId == MouCaoHongComposition.Xianju && _characterCaoHongMouOperations.CreateMouCaoHongFacts().ActualPlay(c.OwnerSeat) && _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive && _characterCaoHongMouOperations.CreateMouCaoHongFacts().InRange(c.OwnerSeat).Any(s => _characterCaoHongMouOperations.CreateMouCaoHongFacts().Cards(c.OwnerSeat, s, false).Count > 0);
    internal bool ValidMouCaoHongDebt(MouCaoHongXianjuIssuedEvent d) => d.Source.SkillId == MouCaoHongComposition.Xianju && d.Source.BindingId == "obtain-in-range" && d.TurnOwner == d.Source.OwnerSeat && d.Turn > 0 && d.PhaseId > 0 && _contentRegistryOperations._contentRegistry.GetSkill(d.Source.SkillId).Program?.GameplayHash == d.GameplayHash && _coreProgramEventHistoryOperations.ProgramEventHistory<MouCaoHongStartedEvent>().Count(s => s.FrameId == d.FrameId && s.Source == d.Source && s.GameplayHash == d.GameplayHash && s.Operation == SkillProgramEffectOp.ResolveMouCaoHongXianju && s.Turn == d.Turn && s.TurnOwner == d.TurnOwner && s.PhaseId == d.PhaseId) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramSkillStartedEvent>().Count(s => s.FrameId == d.FrameId && s.SkillId == d.Source.SkillId && s.OwnerSeat == d.Source.OwnerSeat && s.ActivationId == d.Source.BindingId) == 1;
    internal IEnumerable<ProgramTriggerCandidate> IssuedMouCaoHongPlayEndingCandidates(CharacterState owner)
    {
        if (!owner.IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None)
            yield break;
        var all = _coreProgramEventHistoryOperations.ProgramEventHistory<MouCaoHongXianjuIssuedEvent>().ToArray();
        for (var i = 0; i < all.Length; i++)
        {
            var d = all[i];
            if (d.Source.OwnerSeat != owner.Seat || d.Turn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber || d.TurnOwner != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat || d.PhaseId != _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId || !ValidMouCaoHongDebt(d) || _coreProgramEventHistoryOperations.ProgramEventHistory<MouCaoHongDebtConsumedEvent>().Any(e => e.DebtId == d.FrameId))
                continue;
            var t = _contentRegistryOperations._contentRegistry.GetSkill(d.Source.SkillId).Program!.Triggers.Single(t => t.Id == "issued-play-end");
            yield return new(owner.Seat, d.Source.SkillId, t.Id, d.Source.SkillInstanceId, d.GameplayHash, t.Priority, i);
        }
    }

    internal MouCaoHongXianjuIssuedEvent? CurrentMouCaoHongDebt(ProgramTriggerCandidate c, ProgramSkillWindowContext w)
    {
        if (c.SkillId != MouCaoHongComposition.Xianju || c.BindingId != "issued-play-end" || w.Window != SkillProgramTriggerWindow.PlayEnding || w.OwnerSeat != c.OwnerSeat || c.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat || w.OccurrenceIndex != c.OccurrenceIndex || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase != TurnPhase.Play || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == w.ParentFrameId)is not ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.PlayEnding } p || p.OwnerSeat != c.OwnerSeat || p.CandidateIndex < 0 || p.CandidateIndex >= p.Candidates.Count || p.Candidates[p.CandidateIndex] != c)
            return null;
        var all = _coreProgramEventHistoryOperations.ProgramEventHistory<MouCaoHongXianjuIssuedEvent>().ToArray();
        if (c.OccurrenceIndex < 0 || c.OccurrenceIndex >= all.Length)
            return null;
        var d = all[c.OccurrenceIndex];
        if (!ValidMouCaoHongDebt(d) || d.Source.OwnerSeat != c.OwnerSeat || d.Source.SkillInstanceId != c.SkillInstanceId || d.GameplayHash != c.GameplayHash || d.Turn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber || d.TurnOwner != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat || d.PhaseId != _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId)
            return null;
        return !_coreProgramEventHistoryOperations.ProgramEventHistory<MouCaoHongDebtConsumedEvent>().Any(e => e.DebtId == d.FrameId) || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(f => f.MouCaoHong is { } r && r.DebtId == d.FrameId && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, c) && f.WindowContext == w) ? d : null;
    }

    [ProgramIssuedTrigger(390)]
    internal bool IsIssuedMouCaoHongCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => _coreMatchOutcomeWinnerAccess._winner == Winner.None && _coreTableSeatQueriesOperations.IsValidPlayerSeat(c.OwnerSeat) && _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive && CurrentMouCaoHongDebt(c, w)is not null;
    [ProgramDistanceRule(ProgramDistanceStage.DistanceProjection, 35900)]
    internal RuleQueryEvaluation MouCaoHongDistance(CharacterState source, CharacterState target, RuleQueryEvaluation original)
    {
        if (source.Seat == target.Seat)
            return original;
        var grants = _coreProgramEventHistoryOperations.ProgramEventHistory<MouCaoHongDistanceIssuedEvent>().Where(d => d.Turn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && d.TurnOwner == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat && (d.Source.OwnerSeat == source.Seat && d.Target == target.Seat || d.Source.OwnerSeat == target.Seat && d.Target == source.Seat) && !_coreProgramEventHistoryOperations.ProgramEventHistory<TurnEndedEvent>().Any(t => t.TurnNumber == d.Turn && t.ActorSeat == d.TurnOwner) && _contentRegistryOperations._contentRegistry.GetSkill(d.Source.SkillId).Program?.GameplayHash == d.GameplayHash && _coreProgramEventHistoryOperations.ProgramEventHistory<MouCaoHongStartedEvent>().Count(s => s.FrameId == d.FrameId && s.Source == d.Source && s.GameplayHash == d.GameplayHash && s.Operation == SkillProgramEffectOp.ResolveMouCaoHongYingjia && s.ActionId == d.ActionId && s.Turn == d.Turn && s.TurnOwner == d.TurnOwner) == 1).ToArray();
        return grants.Length == 0 ? original : RuleQueryService.Evaluate(SkillRuleQuery.OutgoingDistance, new(1, int.MaxValue), [new RuleQueryBaseTerm("distance:before-mou-cao-hong", CoreGameEngineRuleQueriesRuntime.ConvertRuleValue(original))], grants.Select(g => (RuleQueryContribution)new FiniteRuleQueryContribution($"mou-cao-hong:{(g.FrameId)}:{(g.Target)}", SkillRuleOperation.Set, 1)).ToArray());
    }

    int IMouCaoHongFacts.Turn => _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber;

    int IMouCaoHongFacts.TurnOwner => _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat;

    int IMouCaoHongFacts.PhaseId => _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId;

    long IMouCaoHongFacts.Sequence => _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;

    bool IMouCaoHongFacts.Finished => _coreMatchOutcomeWinnerAccess._winner != Winner.None;

    bool IMouCaoHongFacts.Alive(int s) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(s) && _coreMatchParticipantsPlayersAccess._players[s].IsAlive;
    bool IMouCaoHongFacts.Human(int s) => _coreMatchParticipantsPlayersAccess._players[s].IsHuman;
    bool IMouCaoHongFacts.ActualPlay(int owner) => owner == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } p && p.ActorSeat == owner && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(p);
    string IMouCaoHongFacts.Name(int s) => _coreMatchParticipantsPlayersAccess._players[s].Name;
    int IMouCaoHongFacts.Keep(int owner, int card) => CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(card), _coreMatchParticipantsPlayersAccess._players[owner]);
    SkillProgram? IMouCaoHongFacts.Program(string skill) => _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(skill)?.Program;
    IEnumerable<T> IMouCaoHongFacts.History<T>() => _coreProgramEventHistoryOperations.ProgramEventHistory<T>();
    MouCaoHongUse? IMouCaoHongFacts.Use(ProgramTriggerCandidate c, ProgramSkillWindowContext w)
    {
        if (w.Window != SkillProgramTriggerWindow.CardUseCompleted || w.OwnerSeat != c.OwnerSeat || w.CardUse is not { } info || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == w.ParentFrameId)is not ProgramCardTriggerWindowFrame p || p.Action.ActionId != info.CardActionId || p.Action.Type != CardActionType.Use || p.Action.ActorSeat != c.OwnerSeat || p.CandidateIndex < 0 || p.CandidateIndex >= p.Candidates.Count)
            return null;
        var current = p.Candidates[p.CandidateIndex];
        if (current.OwnerSeat != c.OwnerSeat || current.SkillId != c.SkillId || current.TriggerId != c.BindingId || current.SkillInstanceId != c.SkillInstanceId || current.GameplayHash != c.GameplayHash || _coreCardUseLifecycleOperations.LifecycleCardUse(info.ParentCardUseFrameId)is not { } use || use.Action?.ActionId != p.Action.ActionId || use.SourceSeat != c.OwnerSeat)
            return null;
        return new(p.Id, use.Id, p.Action.ActionId, Array.AsReadOnly(use.TargetSeats.Where(s => _coreTableSeatQueriesOperations.IsValidPlayerSeat(s) && s != c.OwnerSeat).Distinct().Order().ToArray()));
    }

    bool IMouCaoHongFacts.Parent(ProgramSkillFrame f)
    {
        if (f.TriggerId is null)
            return f.SkillId == MouCaoHongComposition.Xianju && f.ActivationId == "obtain-in-range" && f.WindowContext is null && f.SelectedCardIds.Count == 0 && f.SelectedTargetSeats.Count == 0 && ((IMouCaoHongFacts)this).History<ProgramSkillStartedEvent>().Count(p => p.FrameId == f.Id && p.SkillId == f.SkillId && p.OwnerSeat == f.OwnerSeat && p.ActivationId == f.ActivationId) == 1;
        if (f.TriggerId != f.ActivationId || f.WindowContext is not { } w || w.OwnerSeat != f.OwnerSeat)
            return false;
        var n = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (n < 1 || _resolutionStackOperations._resolutionStack[n - 1].Id != w.ParentFrameId)
            return false;
        if (f.SkillId == MouCaoHongComposition.Yingjia && f.TriggerId == "used-other-targets")
            return ((IMouCaoHongFacts)this).Use(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, 0), w)is not null;
        return f.SkillId == MouCaoHongComposition.Xianju && f.TriggerId == "issued-play-end" && _characterCaoHongMouOperations.CurrentMouCaoHongDebt(f, w);
    }

    bool IMouCaoHongFacts.AllDistanceOne(int owner) => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != owner).All(p => _coreNativeRuleQueriesOperations.GetCombatDistance(owner, p.Seat) == 1 && _coreNativeRuleQueriesOperations.GetCombatDistance(p.Seat, owner) == 1);
    IReadOnlyList<int> IMouCaoHongFacts.InRange(int owner) => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != owner && _coreProgramDodgeResponsesOperations.IsWithinAttackRange(owner, p.Seat)).Select(p => p.Seat).Order().ToArray());
    int IMouCaoHongFacts.OutsideCount(int owner) => _coreMatchParticipantsPlayersAccess._players.Count(p => p.IsAlive && p.Seat != owner && !_coreProgramDodgeResponsesOperations.IsWithinAttackRange(owner, p.Seat));
    IReadOnlyList<int> IMouCaoHongFacts.ExchangeTargets(int owner) => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != owner && _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count > 0 && !((IMouCaoHongFacts)this).History<MouCaoHongExchangeIssuedEvent>().Any(x => x.Owner == owner && x.Turn == ((IMouCaoHongFacts)this).Turn && x.Target == p.Seat)).Select(p => p.Seat).Order().ToArray());
    IReadOnlyList<MouCaoHongCard> IMouCaoHongFacts.Hand(int owner) => Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]).Select(c => new MouCaoHongCard(c.Id, c.Kind, CardLocation.Hand(owner), c.DisplayName)).ToArray());
    IReadOnlyList<MouCaoHongCard> IMouCaoHongFacts.Cards(int actor, int holder, bool discard) => !((IMouCaoHongFacts)this).Alive(actor) || !((IMouCaoHongFacts)this).Alive(holder) ? [] : Array.AsReadOnly(new[] { CardZoneKind.Hand, CardZoneKind.Equipment }.SelectMany(z => _cardZonesOperations._cardZones.CardsAt(new(z, holder)).Where(c => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(actor, c, new(z, holder), discard ? OwnedCardMoveIntent.Discard : actor == holder ? OwnedCardMoveIntent.Transfer : OwnedCardMoveIntent.Obtain)).Select(c => new MouCaoHongCard(c.Id, c.Kind, new(z, holder), c.DisplayName))).ToArray());
    MouCaoHongXianjuIssuedEvent? IMouCaoHongFacts.Debt(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => CurrentMouCaoHongDebt(c, w);
    void IMouCaoHongNative.Finish(ProgramSkillFrame f) => _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    void IMouCaoHongNative.Publish() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    bool IMouCaoHongNative.Drain(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    void IMouCaoHongNative.Move(ProgramSkillFrame f)
    {
        if (!MouCaoHong.Valid(f) || f.MouCaoHong is not { } r || r.Cards.Count == 0 || r.Stage is not (MouCaoHongStage.ExchangeTakeChildren or MouCaoHongStage.ReturnChildren or MouCaoHongStage.ObtainChildren or MouCaoHongStage.DiscardChildren))
            throw new InvalidOperationException("Mou Cao Hong movement requires its exact current paid segment.");
        var discard = r.Stage == MouCaoHongStage.DiscardChildren;
        var give = r.Stage == MouCaoHongStage.ReturnChildren;
        var holder = give || discard ? f.OwnerSeat : r.Target;
        if (r.Cards.Any(c => !_characterCaoHongMouOperations.CreateMouCaoHongFacts().Cards(f.OwnerSeat, holder, discard).Contains(c)) || r.Stage == MouCaoHongStage.ExchangeTakeChildren && r.Cards.Any(c => c.From != CardLocation.Hand(r.Target)))
            throw new InvalidOperationException("Mou Cao Hong cannot replace its current native HE material with a different entity or region.");
        var destination = discard ? CardLocation.DiscardPile : CardLocation.Hand(give ? r.Target : f.OwnerSeat);
        var ids = r.Cards.Select(c => c.Id).ToArray();
        void Paid(long batch, IReadOnlyList<CardMovementRecord> moves)
        {
            var parent = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var paid = parent.MouCaoHong!;
            if (!MouCaoHong.Valid(parent) || moves.Count != r.Cards.Count || moves.Any(m => !r.Cards.Any(c => c.Id == m.CardId && c.Kind == m.CardKind && c.From == m.From) || m.Sequence <= r.Before || m.To != destination && m.To != CardLocation.OutsideGame))
                throw new InvalidOperationException("Mou Cao Hong lost its original primary physical movement receipt.");
            _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, parent with { MouCaoHong = paid with { After = moves.Max(m => m.Sequence), Actual = moves.Count(m => discard || m.To == destination) } });
            foreach (var m in moves)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouCaoHongMovementPaidEvent(f.Id, r.Cursor, m.CardId, m.From, m.To, m.Sequence));
            if (give)
                _characterHuangQuanMouOperations.RecordNativeGivenCards(f.Id, f.OwnerSeat, r.Target, ids, r.Before);
        }

        if (discard)
            _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, ids, new(MouCaoHongRuntime.DiscardReason), Paid);
        else
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, destination, new(give ? MouCaoHongRuntime.GiveReason : MouCaoHongRuntime.TakeReason), Paid);
    }

    MouCaoHongCard IMouCaoHongNative.RandomHand(ProgramSkillFrame f, IReadOnlyList<MouCaoHongCard> cards)
    {
        if (!MouCaoHong.Valid(f) || f.MouCaoHong is not { Stage: MouCaoHongStage.ObtainCard } r || cards.Count == 0 || cards.Any(c => c.From != CardLocation.Hand(r.Target)))
            throw new InvalidOperationException("Xianju requires the original actor's blind current opposing-Hand choice.");
        var before = _randomOperations._random.State;
        var card = cards[_randomOperations._random.Next(cards.Count)];
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouCaoHongHandChosenEvent(f.Id, r.Target, card.Id, before, _randomOperations._random.State, cards.Count));
        return card;
    }
}
