#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineAlternatingSuitAndCompletedTopRuntime
{
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 240)]
    internal void ObserveFirstTurnCategoryUse(IGameEvent payload)
    {
        if (payload is not CardUseDeclaredEvent declared)
            return;
        var use = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(f => f.Id == declared.ResolutionId);
        if (use?.Action is { } action)
        {
            RecordFirstTurnCategoryUse(action);
            return;
        }

        foreach (var skillId in _contentRegistryOperations._contentRegistry.ProgramDependencies.GetTriggerOperationSkillIds(SkillProgramEffectOp.FirstCategoryCompletedTop))
            _coreGameEngineAlternatingSuitAndCompletedTopFirstTurnCategoryUsesAccess._firstTurnCategoryUses.TryAdd((_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, declared.SourceSeat, skillId, CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(declared.CardKind)), -declared.ResolutionId);
    }

    internal void RecordFirstTurnCategoryUse(CardActionContext action)
    {
        foreach (var skillId in _contentRegistryOperations._contentRegistry.ProgramDependencies.GetTriggerOperationSkillIds(SkillProgramEffectOp.FirstCategoryCompletedTop))
            _coreGameEngineAlternatingSuitAndCompletedTopFirstTurnCategoryUsesAccess._firstTurnCategoryUses.TryAdd((_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, action.ActorSeat, skillId, CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(action.EffectiveKind)), action.ActionId);
    }

    internal bool IsFirstCategoryUse(int owner, string skill, CardActionContext action) => action.ActorSeat == owner && _coreGameEngineAlternatingSuitAndCompletedTopFirstTurnCategoryUsesAccess._firstTurnCategoryUses.GetValueOrDefault((_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, owner, skill, CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(action.EffectiveKind)), -1) == action.ActionId;
    internal int[] AvailableCompletedTopCosts(CardActionContext a, long parentFrameId) => a.PhysicalCards.Count != 1 ? [] : a.PhysicalCards.Where(c =>
    {
        var at = _cardZonesOperations._cardZones.GetLocation(c.CardId);
        return at.Zone is CardZoneKind.Processing or CardZoneKind.DiscardPile || at == CardLocation.Equipment(a.ActorSeat) && _coreEquipmentContentOperations.EquipmentRules.IsEquipment(a.EffectiveKind) && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<EquipmentChangedEvent>().Any(e => e.ResolutionId == parentFrameId && e.PlayerSeat == a.ActorSeat && e.CardId == c.CardId && e.CardKind == a.EffectiveKind);
    }).Select(c => c.CardId).Distinct().ToArray();
    internal bool IsLiveSuitAllowance(PlayPhaseSuitAllowanceGrantedEvent e, int seat) => e.OwnerSeat == seat && e.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && (e.Expiry == SkillProgramTurnSkillExpiry.ActualTurnEnd ? e.ActualTurnOwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.TurnNumber == e.TurnNumber && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ActualTurnCursorReturnedEvent>().Any(ended => ended.ActualTurn == e.TurnNumber && ended.OwnerSeat == e.ActualTurnOwnerSeat) : _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && seat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat && e.PhaseInstanceId == _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId);
    internal bool HasPhaseSuitAllowance(int seat, Suit? suit) => suit is { } s && s != Suit.None && _coreProgramEventHistoryOperations.ProgramEventHistory<PlayPhaseSuitAllowanceGrantedEvent>().Any(e => IsLiveSuitAllowance(e, seat) && e.Suits.Contains(s));
    internal IReadOnlyList<IssuedPlayPhaseSuitUseAllowance>? GetIssuedPlayPhaseSuitUseAllowances(int seat)
    {
        var issued = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<PlayPhaseSuitAllowanceGrantedEvent>().Where(e => IsLiveSuitAllowance(e, seat)).Select(e => new IssuedPlayPhaseSuitUseAllowance(e.OwnerSeat, e.SkillId, e.SkillInstanceId, e.TurnNumber, e.PhaseInstanceId, e.Suits) { Expiry = e.Expiry, ActualTurnOwnerSeat = e.ActualTurnOwnerSeat }).ToArray();
        return issued.Length == 0 ? null : issued;
    }

    internal Suit? PhysicalGroupSuit(CharacterState owner, IReadOnlyList<Card> cards)
    {
        var suits = cards.Select(c => _coreNativeCardAppearanceOperations.EffectiveSuit(owner, c)).Distinct().ToArray();
        return suits.Length == 1 ? suits[0] : null;
    }

    internal bool HasPhaseSuitAllowance(CharacterState owner, Card card) => HasPhaseSuitAllowance(owner.Seat, _coreNativeCardAppearanceOperations.EffectiveSuit(owner, card));
    SkillProgramStepOutcome IAlternatingSuitAndCompletedTopHost.BeginAlternatingSuitDrawDiscard(ProgramSkillFrame f) => BeginAlternatingSuitDrawDiscard(f);
    SkillProgramStepOutcome IAlternatingSuitAndCompletedTopHost.BeginFirstCategoryCompletedTop(ProgramSkillFrame f) => BeginFirstCategoryCompletedTop(f);
    internal SkillProgramStepOutcome BeginAlternatingSuitDrawDiscard(ProgramSkillFrame f)
    {
        if (f.ActivationId is null || f.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase != TurnPhase.Play || f.AlternatingSuitTop is not null)
            throw new InvalidOperationException("Alternating suit draw requires clean own Play activation.");
        var yin = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<AlternatingSuitStateCommittedEvent>().LastOrDefault(e => e.OwnerSeat == f.OwnerSeat && e.SkillId == f.SkillId)?.NextYin == true;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AlternatingSuitStateCommittedEvent(f.OwnerSeat, f.SkillId, f.SkillInstanceId, !yin, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId));
        f = f with
        {
            AlternatingSuitTop = new("alternating", "draw", yin ? 1 : 2, [], [])
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f);
        _coreProgramLifecycleOperations.DrawProgramCards(f.Id, f.OwnerSeat, yin ? 2 : 1, null, null, SkillProgramCardSetVisibility.Private, new("skill-program.alternating-suit.draw"));
        return _coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(f.Id, f.OwnerSeat) == SkillProgramStepOutcome.Continue ? ResumeAlternatingSuitTopImmediately(f.Id) : SkillProgramStepOutcome.AwaitChild;
    }

    internal SkillProgramStepOutcome ResumeAlternatingSuitTopImmediately(long id)
    {
        _coreRuntimeOperations.AdvanceRuntimeProgram(id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal SkillProgramStepOutcome BeginFirstCategoryCompletedTop(ProgramSkillFrame f)
    {
        var window = _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().Single(w => w.Id == f.WindowContext!.ParentFrameId);
        var a = window.Action;
        if (!IsFirstCategoryUse(f.OwnerSeat, f.SkillId, a) || f.WindowContext!.Window != SkillProgramTriggerWindow.CardUseCompleted)
            throw new InvalidOperationException("Completed top requires exact first-category use.");
        var ids = AvailableCompletedTopCosts(a, window.ParentFrameId);
        if (ids.Length != 1)
            return SkillProgramStepOutcome.Continue;
        f = f with
        {
            AlternatingSuitTop = new("completed", "order", 1, ids, ids, a.ActionId)
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f);
        CommitAlternatingSuitTop(f);
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 950)]
    internal bool ResumeAlternatingSuitTop(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var d = f.AlternatingSuitTop;
        if (d is null)
            return false;
        if (!_coreQuotaTopProgramsOperations.QuotaTopSourceValid(f))
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { AlternatingSuitTop = null });
            return false;
        }

        if (d.Stage == "draw" && d.Mode == "alternating")
        {
            f = f with
            {
                AlternatingSuitTop = d with
                {
                    Stage = "discard",
                    Required = Math.Min(d.Required, _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count),
                    CardIds = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Select(c => c.Id).ToArray()
                }
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f);
            PublishAlternatingSuitTop(f);
            return true;
        }

        if (d.Stage == "discard-movement")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { AlternatingSuitTop = null });
            return false;
        }

        if (d.Stage == "top-movement")
        {
            f = f with
            {
                AlternatingSuitTop = d with
                {
                    Stage = "reward-draw"
                }
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f);
            _coreProgramLifecycleOperations.DrawProgramCards(f.Id, f.OwnerSeat, 1, null, null, SkillProgramCardSetVisibility.Private, new("skill-program.completed-top.draw"));
            if (_coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(f.Id, f.OwnerSeat) == SkillProgramStepOutcome.Continue)
                _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return true;
        }

        if (d.Stage == "reward-draw")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { AlternatingSuitTop = null });
            return false;
        }

        return false;
    }

    internal void PublishAlternatingSuitTop(ProgramSkillFrame f)
    {
        var d = f.AlternatingSuitTop!;
        if (d.SelectedIds.Count == d.Required)
        {
            CommitAlternatingSuitTop(f);
            return;
        }

        var choices = d.CardIds.Except(d.SelectedIds).Select(id =>
        {
            var c = _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(id)).Single(c => c.Id == id);
            return new PromptChoice(new($"suit-top.{(f.Id)}.{(d.Stage)}.{(id)}"), $"{((d.Mode == "alternating" ? "弃置" : "置顶次序"))}【{(c.DisplayName)}】{(CoreNativeCardClassificationsRuntime.GetSuitDisplayName(c.Suit))}{(c.RankText)}", [id], [], new Dictionary<string, string> { { "program-action", "alternating-suit-top" }, { "frame-id", f.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) } });
        }).ToArray();
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, d.Mode == "alternating" ? "选择要弃置的手牌" : "依次选择牌堆顶顺序（先选为顶牌）", choices.SelectMany(c => c.Cards).ToArray(), [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = d.Mode == "alternating",
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("alternating-suit-top")]
    internal void ResolveAlternatingSuitTopChoice(PromptChoice c)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(long.Parse(c.Parameters["frame-id"], System.Globalization.CultureInfo.InvariantCulture));
        var d = f.AlternatingSuitTop!;
        if (!_corePendingDecisionPolicyPipelineOperations._pendingDecision!.Choices.Any(x => x.Id == c.Id) || c.Cards.Count != 1 || !d.CardIds.Except(d.SelectedIds).Contains(c.Cards[0]))
            throw new InvalidOperationException("Suit/top choice lost exact frame cursor.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        f = f with
        {
            AlternatingSuitTop = d with
            {
                SelectedIds = d.SelectedIds.Append(c.Cards[0]).ToArray()
            }
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f);
        PublishAlternatingSuitTop(f);
    }

    internal void CommitAlternatingSuitTop(ProgramSkillFrame f)
    {
        var d = f.AlternatingSuitTop!;
        var destination = d.Mode == "alternating" ? CardLocation.DiscardPile : CardLocation.DrawPile;
        var ids = d.SelectedIds.ToArray();
        if (d.Mode == "alternating" && ids.Any(id => _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(f.OwnerSeat)))
            throw new InvalidOperationException("Discard lost true owned hand costs.");
        if (d.Mode == "completed")
        {
            var w = _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().Single(w => w.Id == f.WindowContext!.ParentFrameId);
            if (w.Action.ActionId != d.ActionId || !ids.All(id => AvailableCompletedTopCosts(w.Action, w.ParentFrameId).Contains(id)))
                throw new InvalidOperationException("Completed top costs are no longer claimable.");
        }

        var suits = d.Mode == "alternating" ? ids.Select(id => _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _cardZonesOperations._cardZones.CardsAt(CardLocation.Hand(f.OwnerSeat)).Single(c => c.Id == id))).Where(s => s != Suit.None).Distinct().ToArray() : [];
        f = f with
        {
            AlternatingSuitTop = d with
            {
                Stage = d.Mode == "alternating" ? "discard-movement" : "top-movement",
                PaidSuits = suits
            }
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f);
        var moves = ids.Select(id =>
        {
            var at = _cardZonesOperations._cardZones.GetLocation(id);
            return (Card: _cardZonesOperations._cardZones.CardsAt(at).Single(c => c.Id == id), From: at);
        }).ToArray();
        var discardInvocation = d.Mode == "alternating" ? _coreNativeLogicalDiscardsOperations.CaptureNativeLogicalDiscardInvocation(f.OwnerSeat, f.OwnerSeat, ids, new("skill-program.alternating-suit.discard"), f.Id) : null;
        var batch = _coreCardMovementProgramsOperations.BeginCardMovementBatch(moves.Select(m => m.From), [destination]);
        var records = new List<CardMovementRecord>();
        var committed = false;
        try
        {
            foreach (var m in moves)
                _cardZonesOperations._cardZones.Move(m.Card.Id, m.From, destination);
            if (d.Mode == "completed")
                _cardZonesOperations._cardZones.PlaceDrawPileCardsAtTop(ids);
            foreach (var m in moves)
                records.Add(_corePhysicalMovementFacadesOperations.RecordMovement(m.Card, m.From, destination, new(d.Mode == "alternating" ? "skill-program.alternating-suit.discard" : "skill-program.completed-top.place")));
            committed = true;
        }
        finally
        {
            _coreCardMovementProgramsOperations.CompleteCardMovementBatch(batch, records, committed);
        }

        _coreNativeLogicalDiscardsOperations.CompleteNativeLogicalDiscardInvocation(discardInvocation);
        if (d.Mode == "alternating")
        {
            var expiry = ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).GetPausedInstruction(f.InstructionIndex).Effect.SuitAllowanceExpiry;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlayPhaseSuitAllowanceGrantedEvent(f.OwnerSeat, f.SkillId, f.SkillInstanceId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, suits, ids) { Expiry = expiry, ActualTurnOwnerSeat = expiry == SkillProgramTurnSkillExpiry.ActualTurnEnd ? _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat : null });
        }

        if (_coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(f.Id, f.OwnerSeat) == SkillProgramStepOutcome.Continue)
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramDistanceRule(ProgramDistanceStage.CardUseExemption, 120)]
    internal bool DistanceCardUseExemption120HasPhaseSuitAllowance(ProgramCardDistanceQuery q) => (q.CardUseFrameId is { } id && HasPhaseSuitAllowance(q.Actor.Seat, _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(f => f.Id == id)?.Action?.EffectiveSuit));
    [ProgramDistanceRule(ProgramDistanceStage.PhysicalSlash, 190)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedPhysicalSlash, 80)]
    internal bool DistancePhysicalSlash190HasPhaseSuitAllowance(ProgramSlashDistanceQuery q) => HasPhaseSuitAllowance(q.Actor, q.PhysicalCard!);
    [ProgramDistanceRule(ProgramDistanceStage.VirtualSlash, 90)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedSlash, 140)]
    internal bool DistanceVirtualSlash90HasPhaseSuitAllowance(ProgramSlashDistanceQuery q) => HasPhaseSuitAllowance(q.Actor.Seat, q.PhysicalSuit);
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedSlash, 150)]
    internal bool DistanceBorrowedSlash150HasPhaseSuitAllowance(ProgramSlashDistanceQuery q) => (q.AllowAnyPhysicalSuit && _coreActionQueriesOperations.GetSlashUseCards(q.Actor).Any(c => HasPhaseSuitAllowance(q.Actor, c)));
}
