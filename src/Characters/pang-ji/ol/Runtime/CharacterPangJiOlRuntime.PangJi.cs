#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterPangJiOlRuntime
{
    internal const string BiguoCostReason = "skill-program.pang-ji.biguo.discard";
    internal const string BiguoObtainReason = "skill-program.pang-ji.biguo.obtain";
    internal const string BiguoResultBind = "biguo-contest", BiguoGainBind = "biguo-category-gain";
    internal bool HasPangJiCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveBiguoDrawAndArmTargetPindian) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveBiguoTargetedPindianAndCategoryBan) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveDouyuPreRevealSwapAndWinDraw);

    internal static CardConversionSource PangJiSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_pang_ji_ol_PangJi.PangJiSource(f);
    internal bool BiguoTurnOpen(int turn, int owner) => turn > 0 && turn == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber && _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.TurnNumber == turn && _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat == owner && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<TurnEndedEvent>().Any(e => e.TurnNumber == turn && e.ActorSeat == owner) && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<ActualTurnCursorReturnedEvent>().Any(e => e.ActualTurn == turn && e.OwnerSeat == owner);
    SkillProgramStepOutcome IPangJiHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginPangJi(f, op);
    internal IReadOnlyList<PromptChoice> BiguoCosts(ProgramSkillFrame f) => _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, f.OwnerSeat, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Discard);
    internal bool CanBiguoPindian(int owner) => _coreMatchParticipantsPlayersAccess._players[owner].IsAlive && _characterLuKangWumiaoOperations.HasWuLuKangPindianInput(_coreMatchParticipantsPlayersAccess._players[owner]) && !_resolutionStackOperations._resolutionStack.Any(f => f is PindianFrame) && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != owner && _coreEquipmentSlotGroupProgramsOperations.CanBePindianTarget(owner, p.Seat));
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 28130)]
    internal bool CanOfferPangJi(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext c)
    {
        if (trigger.Effects is [{ Op: SkillProgramEffectOp.ResolveBiguoDrawAndArmTargetPindian }])
            return c.Window == SkillProgramTriggerWindow.DrawPhaseEnded && c.OwnerSeat == candidate.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat])).Any();
        return trigger.Effects is not [{ Op: SkillProgramEffectOp.ResolveBiguoTargetedPindianAndCategoryBan }] || IsIssuedBiguoCandidate(candidate, c);
    }

    internal SkillProgramStepOutcome BeginPangJi(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.PangJi is not null || f.InstructionIndex != 1 || f.TriggerId is null || f.WindowContext is not { } c || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0)
            throw new InvalidOperationException("Pang Ji requires his exact unexecuted native binding.");
        var source = PangJiSource(f);
        var target = op == SkillProgramEffectOp.ResolveBiguoTargetedPindianAndCategoryBan;
        var candidate = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, c.OccurrenceIndex);
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            return SkillProgramStepOutcome.Continue;
        if (target ? !IsIssuedBiguoCandidate(candidate, c) : c.Window != SkillProgramTriggerWindow.DrawPhaseEnded || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
            throw new InvalidOperationException("Biguo lost his native draw-end or already-paid target entitlement.");
        var r = new PangJiReceipt(source, f.GameplayHash, op, c.ParentFrameId, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, target ? PangJiStage.Pindian : PangJiStage.CostChoice)
        {
            Target = c.BiguoTarget,
            Paid = target,
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PangJi = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PangJiStartedEvent(f.Id, source, f.GameplayHash, op, r.WindowId, r.ActualTurn, r.TurnOwner));
        if (target)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BiguoTargetAcceptedEvent(f.Id, r.Target!));
            var childId = _coreResolutionIdentityBoundaryOperations._resolutionSequence + 1;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PangJi = r = r with { PindianId = childId } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BiguoPindianIssuedEvent(f.Id, childId, r.Target!.ArmFrameId, r.Target.UseId, r.Target.ActionId, f.OwnerSeat, r.Before));
            var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
            _corePindianOperations.BeginSharedPindian(f.Id, new(f.SkillId, skill.Name, skill.Name + " · 拼点", skill.Description), f.OwnerSeat, programResultBind: BiguoResultBind);
        }
        else
            PublishPangJi(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> PangJiChoices(ProgramSkillFrame f)
    {
        if (f.PangJi is not { Paid: false, Stage: PangJiStage.CostChoice })
            return[];
        PromptChoice Choice(string branch, string label, IReadOnlyList<int> ids) => new(new($"pang-ji.{(f.Id)}.{(branch)}.{(string.Join('-', ids))}"), label, ids, [], new Dictionary<string, string> { ["program-action"] = "pang-ji", ["pang-ji"] = branch, ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture) });
        return Array.AsReadOnly(BiguoCosts(f).Select(c => Choice("cost-card", "弃置" + c.Label, c.Cards)).Append(Choice("cancel", "不发动愎果", [])).Select(PromptChoiceProjection.Freeze).ToArray());
    }

    internal void PublishPangJi(ProgramSkillFrame f)
    {
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
        {
            FinishPangJi(f);
            return;
        }

        var choices = PangJiChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "愎果：弃置一张真实手牌或装备牌。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("pang-ji")]
    internal void AnswerPangJi(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { PangJi: { Paid: false, Stage: PangJiStage.CostChoice } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !PangJiChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Biguo requires the exact original private HE cost choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        if (choice.Parameters["pang-ji"] == "cancel")
        {
            FinishPangJi(f);
            return;
        }

        if (choice.Parameters["pang-ji"] != "cost-card")
            throw new InvalidOperationException("Unknown Pang Ji input.");
        var id = choice.Cards.Single();
        var cost = new CardActionCost(id, _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind, _cardZonesOperations._cardZones.GetLocation(id));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PangJi = r with { Cost = cost, Paid = true, Stage = PangJiStage.CostChildren, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BiguoCostCommittedEvent(f.Id, id, cost.CardKind, cost.From, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, [id], new(BiguoCostReason), (batch, moves) =>
        {
            if (moves is not [var m] || m.CardId != id || m.From != cost.From || m.CardKind != cost.CardKind)
                throw new InvalidOperationException("Biguo cannot replace his original one-card logical discard invoice.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BiguoCostPaidEvent(f.Id, batch, id, m.CardKind, m.From, m.To, m.Reason, m.Sequence));
        });
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { PangJi = f.PangJi!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        ContinuePangJi(f.Id);
    }

    internal void ObtainBiguoCategories(ProgramSkillFrame f)
    {
        var r = f.PangJi!;
        var entities = new List<PangJiEntity>();
        foreach (var category in new[]
        {
            SkillProgramCardCategory.Basic,
            SkillProgramCardCategory.Trick,
            SkillProgramCardCategory.Equipment
        }

        )
        {
            var cards = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Where(c => CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(c.Kind) == category).OrderBy(c => c.Id).ToArray();
            if (cards.Length > 0)
            {
                var c = cards[_randomOperations._random.Next(cards.Length)];
                entities.Add(new(c.Id, c.Kind, CardLocation.DrawPile));
            }
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PangJi = r with { Stage = PangJiStage.ObtainChildren, Obtained = entities, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        for (var i = 0; i < entities.Count; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BiguoObtainEntityIssuedEvent(f.Id, i, entities[i].CardId, entities[i].PrintedKind));
        _coreProgramLifecycleOperations.SetProgramCardSet(f.Id, BiguoGainBind, entities.Select(e => e.CardId).ToArray(), SkillProgramCardSetVisibility.Public, entities.Select(e => e.From).ToArray());
        _coreProgramCategoryChallengesOperations.RevealProgramBoundCards(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), BiguoGainBind);
        if (entities.Count > 0)
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(entities.Select(e => e.CardId).ToArray(), CardLocation.Hand(f.OwnerSeat), new CardMoveReason(BiguoObtainReason), (batch, moves) =>
            {
                for (var i = 0; i < entities.Count; i++)
                {
                    var entity = entities[i];
                    var m = moves.Single(m => m.CardId == entity.CardId && m.From == entity.From && m.To == CardLocation.Hand(f.OwnerSeat));
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BiguoObtainEntityPaidEvent(f.Id, i, batch, m.CardId, m.CardKind, m.From, m.To, m.Sequence));
                }
            });
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { PangJi = f.PangJi!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
    }

    internal bool DrainPangJi(long id) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id);
    internal void ContinuePangJi(long id)
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { PangJi: { } r } f && f.Id == id && _corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
        {
            if (r.Stage == PangJiStage.Pindian)
            {
                var facts = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<PindianResultDeterminedEvent>().Where(e => e.FrameId == r.PindianId && e.SkillId == f.SkillId).ToArray();
                var binding = f.PindianResultBindings.SingleOrDefault(b => b.Name == BiguoResultBind);
                if (facts is not [var determined] || binding is null || determined.Result.SourceSeat != f.OwnerSeat || binding.SourceSeat != determined.Result.SourceSeat || binding.OpponentSeat != determined.Result.OpponentSeat || binding.SourceRank != determined.Result.SourceRank || binding.OpponentRank != determined.Result.OpponentRank || binding.SourceWon != determined.Result.SourceWon || _resolutionStackOperations._resolutionStack.Any(x => x.Id == r.PindianId))
                    throw new InvalidOperationException("Biguo lost his completed native pair, claims or final result binding.");
                var result = determined.Result;
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PangJi = r = r with { Stage = PangJiStage.PindianChildren, Result = result, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BiguoPindianReturnedEvent(f.Id, r.PindianId, result.SourceSeat, result.OpponentSeat, result.SourceCardId, result.OpponentCardId, result.SourceRank, result.OpponentRank));
            }
            if (r.Stage is PangJiStage.CostChildren or PangJiStage.ObtainChildren or PangJiStage.PindianChildren)
            {
                if (DrainPangJi(id))
                    return;
                f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                r = f.PangJi!;
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
            }

            if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                FinishPangJi(f);
                return;
            }

            if (r.Stage == PangJiStage.CostChildren)
            {
                if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
                {
                    FinishPangJi(f);
                    return;
                }

                ObtainBiguoCategories(f);
                continue;
            }

            if (r.Stage == PangJiStage.ObtainChildren)
            {
                if (_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && BiguoTurnOpen(r.ActualTurn, r.TurnOwner))
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BiguoArmedEvent(f.Id, r.Source, r.GameplayHash, r.ActualTurn, r.TurnOwner));
                FinishPangJi(f);
                return;
            }

            if (r.Stage == PangJiStage.PindianChildren)
            {
                FinishPangJi(f);
                return;
            }

            PublishPangJi(f);
            return;
        }
    }

    internal void FinishPangJi(ProgramSkillFrame f)
    {
        var r = f.PangJi!;
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PangJiCompletedEvent(f.Id, r.Op, r.Paid));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PangJi = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 28130)]
    internal bool ResumePangJi(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { PangJi: not null } f || f.Id != id || f.InstructionIndex == 0)
            return false;
        ContinuePangJi(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 28130)]
    internal bool ReturnPangJiMovement(ProgramSkillFrame f)
    {
        if (f.PangJi is null)
            return false;
        ContinuePangJi(f.Id);
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveBiguoDrawAndArmTargetPindian)]
    internal PromptChoice ChoosePangJiAi(PendingDecision p, ProgramSkillFrame f) => p.Choices.Where(c => c.Cards.Count == 1).OrderBy(c => CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards[0]), _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])).ThenBy(c => c.Id.Value, StringComparer.Ordinal).FirstOrDefault() ?? p.Choices.Single();
}
