#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineQuotaTopProgramsRuntime
{
    internal void RegisterFirstTurnTargetActions(CardActionContext action, SkillProgramTriggerWindow window)
    {
        if (window != SkillProgramTriggerWindow.CardUseTargetsFinalized || action.Type != CardActionType.Use)
            return;
        foreach (var skillId in _contentRegistryOperations._contentRegistry.ProgramDependencies.GetTriggerOperationSkillIds(SkillProgramEffectOp.NullifyFirstTurnTargetByHand))
        {
            var skill = _contentRegistryOperations._contentRegistry.GetSkill(skillId);
            foreach (var trigger in skill.Program!.Triggers.Where(trigger => trigger.Effects.Any(effect => effect.Op == SkillProgramEffectOp.NullifyFirstTurnTargetByHand)))
            {
                if (!ProgramCardKindMatch.Contains(trigger.CardKinds, action.EffectiveKind))
                    continue;
                foreach (var target in action.EffectiveDesignatedTargetSeats.Distinct().Where(target => target != action.ActorSeat))
                    _coreGameEngineQuotaTopProgramsFirstTurnTargetActionsAccess._firstTurnTargetActions.TryAdd((_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, target, skill.Id), action.ActionId);
            }
        }
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeCondition, 40, SkillProgramEffectOp.NullifyFirstTurnTargetByHand)]
    internal bool IsFirstTurnTarget(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => context.CardUse is { } use && use.ActorSeat != candidate.OwnerSeat && use.EventTargetSeat == candidate.OwnerSeat && _coreGameEngineQuotaTopProgramsFirstTurnTargetActionsAccess._firstTurnTargetActions.GetValueOrDefault((_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, candidate.OwnerSeat, candidate.SkillId), -1) == use.CardActionId;
    internal void NullifyProgramFirstTurnTarget(ProgramSkillFrame frame)
    {
        var context = frame.WindowContext ?? throw new InvalidOperationException("First-target defense requires its actual use.");
        var candidate = new ProgramTriggerCandidate(frame.OwnerSeat, frame.SkillId, frame.TriggerId!, frame.SkillInstanceId, frame.GameplayHash, 0);
        var use = context.CardUse ?? throw new InvalidOperationException("First-target defense requires a card action.");
        var parent = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(item => item.Id == use.ParentCardUseFrameId);
        if (context.Window != SkillProgramTriggerWindow.CardUseTargetsFinalized || parent?.Action?.ActionId != use.CardActionId || !parent.TargetSeats.Contains(frame.OwnerSeat) || !IsFirstTurnTarget(candidate, context))
            throw new InvalidOperationException("First-target defense lost its precise designation parent.");
        if (_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[use.ActorSeat]).Count >= _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]).Count)
        {
            _coreProgramCardTargetEffectsOperations.MarkCardEffectIneffective(parent.Id, frame.OwnerSeat);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardEffectSkippedEvent(parent.Id, use.ActorSeat, frame.OwnerSeat, use.EffectiveKind, CardEffectSkipReason.SkillNullified));
        }
    }

    internal int ComputeTurnCriterionQuota(int ownerSeat)
    {
        var history = _corePlayCardHistoryOperations.EventsSinceLastBoundary(item => item is TurnStartedEvent).ToArray();
        var ownedDiscard = _coreCardMovementJournalCardMovementsAccess._cardMovements.Any(record => record.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && _coreProgramDiscardOriginsOperations.GetProgramDiscardSource(record)?.OwnerSeat == ownerSeat);
        return (history.OfType<DamageAppliedEvent>().Any(d => d.SourceSeat == ownerSeat && !d.SourceLess && d.Amount > 0) ? 1 : 0) + (ownedDiscard ? 0 : 1) + (_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).All(p => _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count >= _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[ownerSeat]).Count) ? 1 : 0);
    }

    internal SkillProgramStepOutcome BeginQuotaTop(ProgramSkillFrame frame, int count)
    {
        if (frame.WindowContext?.Window != SkillProgramTriggerWindow.TurnEnding || frame.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || frame.QuotaTop is not null)
            throw new InvalidOperationException("Quota peek requires a clean real owner ending window.");
        var quota = ComputeTurnCriterionQuota(frame.OwnerSeat);
        _ = _corePhysicalDrawFacadesOperations.EnsureDrawPile();
        var ids = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).TakeLast(Math.Min(count, _cardZonesOperations._cardZones.Count(CardLocation.DrawPile))).Reverse().Select(card => card.Id).ToArray();
        frame = frame with
        {
            QuotaTop = new(quota, ids, [], [], quota == 0 ? "order" : "gain")
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame);
        PublishQuotaTop(frame);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal bool QuotaTopSourceValid(ProgramSkillFrame frame) => _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive && _coreProgramLifecycleOperations.CanInvokeProgramSkill(frame) && _coreSkillProgramsOperations.EnabledSkillPrograms(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]).Any(program => program.Id == frame.SkillId && program.GameplayHash == frame.GameplayHash);
    internal void PublishQuotaTop(ProgramSkillFrame frame)
    {
        var draft = frame.QuotaTop!;
        var remaining = draft.ViewedIds.Except(draft.ObtainedIds).Except(draft.UnavailableIds ?? []).Except(draft.OrderedTopIds).ToArray();
        if (draft.Stage == "gain" && draft.ObtainedIds.Count == Math.Min(draft.Quota, draft.ViewedIds.Count))
        {
            frame = frame with
            {
                QuotaTop = draft with
                {
                    Stage = "gain-movement"
                }
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(frame);
            var cards = draft.ObtainedIds.Select(id => _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Single(card => card.Id == id)).ToArray();
            _corePhysicalMovementFacadesOperations.MoveCards(cards, CardLocation.DrawPile, CardLocation.Hand(frame.OwnerSeat), new("program.quota-top.obtain"));
            if (_coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat) == SkillProgramStepOutcome.Continue)
                _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
            return;
        }

        if (draft.Stage == "order" && remaining.Length == 0)
        {
            var kept = draft.ViewedIds.Except(draft.ObtainedIds).Except(draft.UnavailableIds ?? []).ToArray();
            _cardZonesOperations._cardZones.ReorderDrawPileSubsetToTop(kept, draft.OrderedTopIds);
            if (draft.Quota > 0)
            {
                FinishQuotaTop(frame);
                return;
            }

            frame = frame with
            {
                QuotaTop = draft with
                {
                    Stage = "loss-choice"
                }
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(frame);
            draft = frame.QuotaTop!;
        }

        IReadOnlyList<PromptChoice> choices = draft.Stage == "loss-choice" ? _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != frame.OwnerSeat).Select(p => CoreGameEngineNamedDefenseAndPublicDraftRuntime.NamedDraftChoice(frame, "quota-top-loss", p.Name, seat: p.Seat)).ToArray() : remaining.Select(id =>
        {
            var card = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Single(card => card.Id == id);
            return CoreGameEngineNamedDefenseAndPublicDraftRuntime.NamedDraftChoice(frame, draft.Stage == "gain" ? "quota-top-gain" : "quota-top-order", $"{((draft.Stage == "gain" ? "获得" : "置于牌堆顶"))}【{(card.DisplayName)}】（{(CoreNativeCardClassificationsRuntime.GetSuitDisplayName(card.Suit))} {(card.RankText)}）", id);
        }).ToArray();
        if (choices.Count == 0)
        {
            FinishQuotaTop(frame);
            return;
        }

        _coreNamedDefenseAndPublicDraftOperations.PublishNamedDraftPrompt(frame, frame.OwnerSeat, draft.Stage switch
        {
            "gain" => $"获得 {(Math.Min(draft.Quota, draft.ViewedIds.Count))} 张牌。",
            "order" => "依次选择剩余牌：先选的牌在牌堆最上方。",
            _ => "选择另一名角色，你与其依次失去1点体力。"
        }, choices, draft.Stage != "loss-choice");
    }

    [ProgramAnswerPrefix("quota-top-")]
    internal void ResolveQuotaTopChoice(PromptChoice choice)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Quota choice requires its program.");
        var draft = frame.QuotaTop ?? throw new InvalidOperationException("Quota choice requires its frozen cards.");
        if (!QuotaTopSourceValid(frame) || choice.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString())
            throw new InvalidOperationException("Quota choice lost its exact source.");
        var action = choice.Parameters.GetValueOrDefault("program-action");
        if (draft.Stage == "loss-choice" && action == "quota-top-loss" && choice.Targets is [var target] && target != frame.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[target].IsAlive)
            draft = draft with
            {
                Stage = "issued-loss",
                LossTarget = target
            };
        else if (choice.Cards is [var id] && draft.ViewedIds.Contains(id) && !draft.ObtainedIds.Contains(id) && !draft.OrderedTopIds.Contains(id) && _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.DrawPile)
            draft = (draft.Stage, action) switch
            {
                ("gain", "quota-top-gain") => draft with
                {
                    ObtainedIds = [..draft.ObtainedIds, id]
                },
                ("order", "quota-top-order") => draft with
                {
                    OrderedTopIds = [..draft.OrderedTopIds, id]
                },
                _ => throw new InvalidOperationException("Quota choice has an incorrect stage.")};
        else
            throw new InvalidOperationException("Quota choice contains no current entity or participant.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { QuotaTop = draft });
        if (draft.Stage == "issued-loss")
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramQuotaLossIssuedEvent(frame.Id, frame.OwnerSeat, frame.SkillId, frame.SkillInstanceId, draft.LossTarget!.Value));
            ResumeQuotaLoss(frame);
        }
        else
            PublishQuotaTop(frame);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 960)]
    internal bool TryResumeQuotaTop(long frameId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame frame || frame.Id != frameId || frame.QuotaTop is not { } draft)
            return false;
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None)
        {
            FinishQuotaTop(frame);
            return true;
        }

        if (draft.Stage == "issued-loss")
        {
            if (_kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().TryStartPostInstructionWindow(frame.Id))
                return true;
            ResumeQuotaLoss(frame);
            return true;
        }

        if (!QuotaTopSourceValid(frame))
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { QuotaTop = null });
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame with { QuotaTop = null }, "私有牌顶操作的来源已失效。");
            return true;
        }

        if (draft.Stage == "gain-movement")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { QuotaTop = draft with { Stage = "order", UnavailableIds = draft.ViewedIds.Except(draft.ObtainedIds).Where(id => _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.DrawPile).ToArray() } });
            PublishQuotaTop(frame);
            return true;
        }

        return false;
    }

    internal void ResumeQuotaLoss(ProgramSkillFrame frame)
    {
        var draft = frame.QuotaTop!;
        // Only this already-issued operation survives its owner's death or loss.
        // The immutable issuance proves both precommitted seats and exact grant.
        var program = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!;
        if (program.GameplayHash != frame.GameplayHash || frame.WindowContext?.Window != SkillProgramTriggerWindow.TurnEnding || ProgramInstructionResolver.Default.Resolve(frame, program).GetPausedInstruction(frame.InstructionIndex).Effect.Op != SkillProgramEffectOp.PeekTurnQuotaTop || draft.Stage != "issued-loss" || draft.Quota != 0 || draft.LossTarget is not { } target || !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramQuotaLossIssuedEvent>().Any(e => e == new ProgramQuotaLossIssuedEvent(frame.Id, frame.OwnerSeat, frame.SkillId, frame.SkillInstanceId, target)))
            throw new InvalidOperationException("Paired HP loss lacks its exact issued commitment.");
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || draft.LossCursor >= 2)
        {
            FinishQuotaTop(frame);
            return;
        }

        var seat = draft.LossCursor == 0 ? frame.OwnerSeat : target;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { QuotaTop = draft with { LossCursor = draft.LossCursor + 1 } });
        if (!_coreMatchParticipantsPlayersAccess._players[seat].IsAlive || _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().LoseHp(frame.Id, frame.SkillId, seat, 1) == SkillProgramStepOutcome.Continue)
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    internal void FinishQuotaTop(ProgramSkillFrame frame)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { QuotaTop = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    internal CardSnapshot[] GetQuotaPrivatelyViewedCards(int viewerSeat) => _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Where(frame => frame.OwnerSeat == viewerSeat && frame.QuotaTop is { Stage: "gain" or "order" }).SelectMany(frame => frame.QuotaTop!.ViewedIds.Except(frame.QuotaTop.ObtainedIds)).Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.DrawPile).Select(id => _coreSnapshotValueProjectionOperations.ToSnapshot(_cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Single(card => card.Id == id))).ToArray();
    SkillProgramStepOutcome IQuotaTopProgramHost.PeekTurnQuotaTop(ProgramSkillFrame frame, int count) => BeginQuotaTop(frame, count);
    void IQuotaTopProgramHost.NullifyFirstTurnTargetByHand(ProgramSkillFrame frame) => NullifyProgramFirstTurnTarget(frame);
}
