#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhouCangBoundaryMobileRuntime
{
    internal const string BoundaryZhongyongReason = "skill-program.boundary-zhou-cang.zhongyong";
    internal bool HasBoundaryZhouCang => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveBoundaryZhouCangZhongyong);

    internal static CardConversionSource BoundaryZhongyongSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    SkillProgramStepOutcome IBoundaryZhouCangHost.Resolve(ProgramSkillFrame f) => BeginBoundaryZhongyong(f);
    // Capture the actual declaration phase and paid entity lineage, before a later
    // observer can change the phase or claim and return an entity to the discard pile.
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 53300)]
    internal void ObserveBoundaryZhongyongOrigin(IGameEvent payload)
    {
        if (!HasBoundaryZhouCang)
            return;
        if (payload is CardUseDeclaredEvent declared && _coreCardUseLifecycleOperations.LifecycleCardUse(declared.ResolutionId)is { Action: { Type: CardActionType.Use } a } use && CoreNativeCardClassificationsRuntime.IsSlashCard(a.EffectiveKind) && _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } phase && phase.ActorSeat == a.ActorSeat)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryZhongyongUseOriginEvent(use.Id, a.ActionId, a.ActorSeat, phase));
            RecordBoundaryZhongyongEntities(a);
        }
        else if (payload is CardActionAcceptedEvent { Action: { Type: CardActionType.Response, EffectiveKind: CardKind.Dodge } response })
            RecordBoundaryZhongyongEntities(response);
    }

    internal void RecordBoundaryZhongyongEntities(CardActionContext a)
    {
        foreach (var c in a.PhysicalCards)
        {
            var location = _cardZonesOperations._cardZones.GetLocation(c.CardId);
            var paid = location.Zone is CardZoneKind.Processing or CardZoneKind.DiscardPile ? _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == c.CardId && m.From == c.From && m.To.Zone is CardZoneKind.Processing or CardZoneKind.DiscardPile)?.Sequence : null;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryZhongyongEntityOriginEvent(a.ActionId, c.CardId, c.From, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, paid));
        }
    }

    internal bool BoundaryZhongyongEntityAvailable(long action, int id)
    {
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryZhongyongEntityOriginEvent>().SingleOrDefault(e => e.ActionId == action && e.CardId == id)is not { } origin || _cardZonesOperations._cardZones.GetLocation(id).Zone is not (CardZoneKind.Processing or CardZoneKind.DiscardPile))
            return false;
        // Slash declaration precedes its native hand->Processing payment. A
        // response is recorded after payment. Both retain the same original
        // source and first actual payment, so claims before Accepted are visible.
        var payment = origin.PaidSequence is { } paid ? _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.SingleOrDefault(m => m.Sequence == paid && m.CardId == id && m.From == origin.From && m.To.Zone is CardZoneKind.Processing or CardZoneKind.DiscardPile) : _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.FirstOrDefault(m => m.Sequence > origin.Sequence && m.CardId == id && m.From == origin.From && m.To.Zone is CardZoneKind.Processing or CardZoneKind.DiscardPile);
        return payment is not null && !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == id && m.Sequence > payment.Sequence && m.From.Zone is CardZoneKind.Processing or CardZoneKind.DiscardPile && m.To.Zone is not (CardZoneKind.Processing or CardZoneKind.DiscardPile));
    }

    internal CardUseFrame? BoundaryZhongyongUse(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (c.SkillId != BoundaryZhouCangComposition.Skill || c.BindingId != BoundaryZhouCangComposition.Binding || context.Window != SkillProgramTriggerWindow.CardUseCompleted || context.CardUse is not { } identity || _coreCardUseLifecycleOperations.LifecycleCardUse(identity.ParentCardUseFrameId)is not { Step: ResolutionFrameStep.Completed, Action: { Type: CardActionType.Use } a } use || a.ActionId != identity.CardActionId || a.ActorSeat != c.OwnerSeat || !CoreNativeCardClassificationsRuntime.IsSlashCard(a.EffectiveKind) || _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryZhongyongUseOriginEvent>().SingleOrDefault(e => e.UseId == use.Id && e.ActionId == a.ActionId && e.Actor == c.OwnerSeat)is null || _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id) != 1 || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { Continuation: ProgramCardContinuation.CompletedSlash or ProgramCardContinuation.CompletedCard } window || window.ParentFrameId != use.Id || window.Action.ActionId != a.ActionId || !_coreDesignatedExtraTargetsOperations.DesignatedExtraTargetCandidateMatches(c, context, window))
            return null;
        return use;
    }

    internal CardActionContext[] BoundaryZhongyongResponses(CardUseFrame use)
    {
        var accepted = _coreProgramEventHistoryOperations.ProgramEventHistory<CardActionAcceptedEvent>().Select(e => e.Action).ToArray();
        return _coreProgramEventHistoryOperations.ProgramEventHistory<CardResponseCompletedEvent>().Where(e => e.AttackOwnerFrameId == use.Id && e.ParentFrameId == use.Id && e.EffectiveKind == CardKind.Dodge && e.OriginalContinuation is ProgramCardContinuation.Dodge or ProgramCardContinuation.FactionDefenseDodge && use.TargetSeats.Contains(e.NativeActorSeat)).Select(e => accepted.SingleOrDefault(a => a.ActionId == e.ActionId && a.Type == CardActionType.Response && a.EffectiveKind == CardKind.Dodge && a.ParentActionId == use.Action!.ActionId && a.ActorSeat == e.NativeActorSeat && a.ProviderSeat == e.ProviderSeat)).Where(a => a is not null).Cast<CardActionContext>().DistinctBy(a => a.ActionId).ToArray();
    }

    internal bool HasBoundaryZhouCangResponseObserver(CardActionContext a, ProgramCardContinuation continuation) => HasBoundaryZhouCang && a.Type == CardActionType.Response && a.EffectiveKind == CardKind.Dodge && continuation is ProgramCardContinuation.Dodge or ProgramCardContinuation.FactionDefenseDodge && a.ParentActionId is { } parent && _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Any(u => u.Action?.ActionId == parent && _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryZhongyongUseOriginEvent>().Any(e => e.UseId == u.Id && e.ActionId == parent));
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 53300, SkillProgramEffectOp.ResolveBoundaryZhouCangZhongyong)]
    internal bool CanOfferBoundaryZhongyong(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (!_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive || BoundaryZhongyongUse(c, context)is not { } use)
            return false;
        var responses = BoundaryZhongyongResponses(use);
        return responses.Length > 0 ? responses.Any(a => a.PhysicalCards.Any(p => BoundaryZhongyongEntityAvailable(a.ActionId, p.CardId))) || use.Action!.PhysicalCards.Any(p => BoundaryZhongyongEntityAvailable(use.Action.ActionId, p.CardId)) : use.Action!.PhysicalCards.Any(p => BoundaryZhongyongEntityAvailable(use.Action.ActionId, p.CardId));
    }

    internal CardUseFrame RequireBoundaryZhongyongUse(ProgramSkillFrame f)
    {
        if (f.WindowContext is not { } c || BoundaryZhongyongUse(new(f.OwnerSeat, f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, 0), c)is not { } use || _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id)is var i && (i < 2 || _resolutionStackOperations._resolutionStack[i - 1].Id != c.ParentFrameId || _resolutionStackOperations._resolutionStack[i - 2].Id != use.Id))
            throw new InvalidOperationException("Zhongyong requires the original completed native Slash and its immediate trigger mother.");
        return use;
    }

    internal SkillProgramStepOutcome BeginBoundaryZhongyong(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var use = RequireBoundaryZhongyongUse(f);
        if (f.BoundaryZhongyong is not null)
            throw new InvalidOperationException("Zhongyong cannot reopen its original completed use.");
        var responses = BoundaryZhongyongResponses(use);
        var phase = _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryZhongyongUseOriginEvent>().Single(e => e.UseId == use.Id).Phase;
        var r = new BoundaryZhongyongReceipt(BoundaryZhongyongSource(f), f.GameplayHash, use.Id, use.Action!.ActionId, f.WindowContext!.ParentFrameId, phase, responses.Length > 0, BoundaryZhongyongStage.Choose)
        {
            Slash = use.Action.PhysicalCards.Where(p => BoundaryZhongyongEntityAvailable(use.Action.ActionId, p.CardId)).Select(p => p.CardId).ToArray(),
            Dodge = responses.SelectMany(a => a.PhysicalCards.Where(p => BoundaryZhongyongEntityAvailable(a.ActionId, p.CardId)).Select(p => p.CardId)).Distinct().ToArray(),
            Excluded = use.TargetSeats
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryZhongyong = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryZhongyongStartedEvent(f.Id, r.Source, r.Hash, r.UseId, r.ActionId, r.WindowId, r.Responded, r.Slash.Count, r.Dodge.Count, r.Phase));
        if (!r.Responded)
        {
            if (r.Slash.Count == 0)
            {
                FinishBoundaryZhongyong(f);
                return SkillProgramStepOutcome.AwaitChild;
            }

            MoveBoundaryZhongyong(f, BoundaryZhongyongStage.AcquireSlashChildren, f.OwnerSeat, r.Slash);
        }
        else
            PublishBoundaryZhongyong(f);
        // The operation owns its original instruction until all movement observers return.
        return _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null ? SkillProgramStepOutcome.AwaitChoice : SkillProgramStepOutcome.AwaitChild;
    }

    internal bool BoundaryZhongyongOriginalTurn(BoundaryZhongyongReceipt r) => r.Phase.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && r.Phase.ActualTurnOwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat;
    internal int[] BoundaryZhongyongRecipients(ProgramSkillFrame f) => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat && !f.BoundaryZhongyong!.Excluded.Contains(p.Seat)).Select(p => p.Seat).ToArray();
    internal int[] AvailableBoundaryZhongyong(ProgramSkillFrame f, bool slash)
    {
        var r = f.BoundaryZhongyong!;
        var use = RequireBoundaryZhongyongUse(f);
        return (slash ? r.Slash : r.Dodge).Where(id => slash ? BoundaryZhongyongEntityAvailable(r.ActionId, id) : BoundaryZhongyongResponses(use).Any(a => a.PhysicalCards.Any(p => p.CardId == id) && BoundaryZhongyongEntityAvailable(a.ActionId, id))).ToArray();
    }

    internal IReadOnlyList<PromptChoice> BoundaryZhongyongChoices(ProgramSkillFrame f)
    {
        var r = f.BoundaryZhongyong!;
        var result = new List<PromptChoice>();
        PromptChoice Choice(string branch, string label, IReadOnlyList<int> ids, int? target = null) => new(new($"boundary-zhongyong.{(f.Id)}.{(branch)}.{(target)}"), label, ids, target is { } t ? [t] : [], new Dictionary<string, string> { ["program-action"] = "boundary-zhongyong", ["branch"] = branch, ["frame-id"] = f.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) });
        if (_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && (r.Paid || _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId)))
        {
            if (r.Stage == BoundaryZhongyongStage.Choose)
            {
                result.Add(Choice("acquire-dodge", "获得响应的闪，然后可以将杀交给另一名其他角色", AvailableBoundaryZhongyong(f, false)));
                var dodge = AvailableBoundaryZhongyong(f, false);
                if (dodge.Length > 0)
                    foreach (var target in BoundaryZhongyongRecipients(f))
                        result.Add(Choice("give-dodge", $"将响应的闪交给{(_coreMatchParticipantsPlayersAccess._players[target].Name)}，杀次数+1且下一杀伤害+1", dodge, target));
            }
            else if (r.Stage == BoundaryZhongyongStage.GiveSlash && AvailableBoundaryZhongyong(f, true)is { Length: > 0 } slash)
                foreach (var target in BoundaryZhongyongRecipients(f))
                    result.Add(Choice("give-slash", $"将此杀交给{(_coreMatchParticipantsPlayersAccess._players[target].Name)}", slash, target));
        }

        result.Add(Choice("decline", r.Stage == BoundaryZhongyongStage.GiveSlash ? "不交出杀" : "不发动", []));
        return result;
    }

    internal void PublishBoundaryZhongyong(ProgramSkillFrame f)
    {
        var choices = BoundaryZhongyongChoices(f);
        var s = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "忠勇：选择获得或交出本次杀与响应的闪。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = f.OwnerSeat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, s.Name, s.Name, s.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("boundary-zhongyong")]
    internal void ResolveBoundaryZhongyongChoice(PromptChoice selected)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Zhongyong lost its original frame.");
        if (!BoundaryZhongyongChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [selected])) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat)
            throw new InvalidOperationException("Zhongyong choice lost its current entities or exact recipient.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        switch (selected.Parameters["branch"])
        {
            case "decline":
                FinishBoundaryZhongyong(f);
                break;
            case "acquire-dodge":
                if (selected.Cards.Count == 0)
                {
                    _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryZhongyong = f.BoundaryZhongyong!with { Stage = BoundaryZhongyongStage.GiveSlash, Paid = true } });
                    PublishBoundaryZhongyong(f);
                }
                else
                    MoveBoundaryZhongyong(f, BoundaryZhongyongStage.AcquireDodgeChildren, f.OwnerSeat, selected.Cards);
                break;
            case "give-dodge":
                MoveBoundaryZhongyong(f, BoundaryZhongyongStage.GiveDodgeChildren, selected.Targets.Single(), selected.Cards);
                break;
            case "give-slash":
                MoveBoundaryZhongyong(f, BoundaryZhongyongStage.GiveSlashChildren, selected.Targets.Single(), selected.Cards);
                break;
            default:
                throw new InvalidOperationException("Unknown Zhongyong branch.");
        }
    }

    internal void MoveBoundaryZhongyong(ProgramSkillFrame f, BoundaryZhongyongStage stage, int recipient, IReadOnlyList<int> ids)
    {
        foreach (var p in _coreMatchParticipantsPlayersAccess._players)
            _observedSkillGrantRevisionsOperations._observedSkillGrantRevisions.TryAdd(p.Seat, p.SkillGrants.Revision);
        var r = f.BoundaryZhongyong!with
        {
            Stage = stage,
            Paid = true,
            Recipient = recipient,
            Index = f.BoundaryZhongyong!.Index + 1,
            Requested = ids,
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            Settlement = null
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryZhongyong = r, PendingMovementContinuation = new(recipient, 0, null) });
        void Settled(HappyWeiYanGainRequestSettlement settlement)
        {
            _coreHappyWeiYanGainDestinationsOperations.ValidateHappyWeiYanGainRequestSettlement(settlement, f.Id, ids, CardLocation.Hand(recipient), r.Before);
            var now = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(now with { BoundaryZhongyong = now.BoundaryZhongyong!with { Settlement = settlement, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            foreach (var m in settlement.Movements)
            {
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryZhongyongMovementEvent(f.Id, r.Index, stage, recipient, m.CardId, m.From, m.To, m.Sequence));
                if (recipient == f.OwnerSeat && m.To == CardLocation.Hand(recipient) && BoundaryZhongyongOriginalTurn(r))
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryZhongyongUseBannedEvent(f.Id, recipient, m.CardId, r.Phase.TurnNumber, r.Phase.ActualTurnOwnerSeat, m.Sequence));
            }
        }

        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, CardLocation.Hand(recipient), new(BoundaryZhongyongReason), afterRequestSettled: recipient == f.OwnerSeat ? Settled : _characterHuangQuanMouOperations.NativeGiveRequestCompletion(f.Id, f.OwnerSeat, recipient, ids, r.Before, Settled), producerFrameId: f.Id);
        var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        var paid = current.BoundaryZhongyong!;
        _coreRuntimeOperations.ReplaceRuntimeTop(current with { BoundaryZhongyong = paid with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryZhongyongMovementSettledEvent(f.Id, r.Index, stage, recipient, ids.Count, r.Before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        ContinueBoundaryZhongyong(f.Id);
    }

    internal bool DrainBoundaryZhongyong(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    internal void ContinueBoundaryZhongyong(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { BoundaryZhongyong: { } r } f || f.Id != id)
            return;
        if (f.PendingMovementContinuation is not null)
        {
            if (DrainBoundaryZhongyong(f))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.BoundaryZhongyong!;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        }

        if (r.Stage is BoundaryZhongyongStage.Choose or BoundaryZhongyongStage.GiveSlash)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishBoundaryZhongyong(f);
            return;
        }

        if (r.Stage == BoundaryZhongyongStage.AcquireDodgeChildren && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryZhongyong = r with { Stage = BoundaryZhongyongStage.GiveSlash, Settlement = null, Requested = [] } });
            PublishBoundaryZhongyong(f);
            return;
        }

        if (r.Stage == BoundaryZhongyongStage.GiveDodgeChildren && BoundaryZhongyongOriginalTurn(r) && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && r.Settlement!.Movements.Any(m => m.To == CardLocation.Hand(r.Recipient)))
        {
            if (_coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryZhongyongBonusEvent>().Any(e => e.FrameId == f.Id))
                throw new InvalidOperationException("Zhongyong cannot grant the same paid bonus twice.");
            _coreCardUseModulesOperations.GrantProgramTurnRuleModifier(f, SkillRuleQuery.SlashLimit, SkillRuleOperation.Add, 1, []);
            var index = _coreGameEngineNextSlashProgramsNamedNextSlashReservesAccess._namedNextSlashReserves.FindIndex(n => n.OwnerSeat == f.OwnerSeat && n.SkillId == f.SkillId && n.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && n.TurnSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat);
            if (index < 0)
                _coreGameEngineNextSlashProgramsNamedNextSlashReservesAccess._namedNextSlashReserves.Add(new(f.OwnerSeat, f.SkillId, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, 1));
            else
                _coreGameEngineNextSlashProgramsNamedNextSlashReservesAccess._namedNextSlashReserves[index] = _coreGameEngineNextSlashProgramsNamedNextSlashReservesAccess._namedNextSlashReserves[index] with
                {
                    Amount = checked(_coreGameEngineNextSlashProgramsNamedNextSlashReservesAccess._namedNextSlashReserves[index].Amount + 1)
                };
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NextSlashDamageReservedEvent(f.OwnerSeat, f.SkillId, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, r.UseId, r.Excluded.FirstOrDefault(), 1));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryZhongyongBonusEvent(f.Id, r.UseId, f.OwnerSeat, r.Recipient, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat));
        }

        FinishBoundaryZhongyong(f);
    }

    internal void FinishBoundaryZhongyong(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { BoundaryZhongyong = null, PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 53300)]
    internal bool ResumeBoundaryZhongyong(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { BoundaryZhongyong: not null } f || f.Id != id)
            return false;
        ContinueBoundaryZhongyong(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 53300)]
    internal bool ReturnBoundaryZhongyongMovement(ProgramSkillFrame f)
    {
        if (f.BoundaryZhongyong is null)
            return false;
        ContinueBoundaryZhongyong(f.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 53300)]
    internal bool AdvanceBoundaryZhongyong()
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { BoundaryZhongyong: not null, PendingMovementContinuation: not null } f)
        {
            ContinueBoundaryZhongyong(f.Id);
            return true;
        }

        return BoundaryZhongyongObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 53300)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 53300)]
    internal bool KeepBoundaryZhongyongPaidFrame(ProgramSkillFrame f) => f.BoundaryZhongyong is { Paid: true };
}
