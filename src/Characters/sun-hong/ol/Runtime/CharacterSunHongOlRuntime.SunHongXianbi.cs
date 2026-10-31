#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunHongOlRuntime
{
    internal const string XianbiDiscardReason = "skill-program.sun-hong.xianbi.discard";
    internal const string XianbiDrawReason = "skill-program.sun-hong.xianbi.draw";
    internal const string XianbiObtainReason = "skill-program.sun-hong.xianbi.obtain";
    internal CardConversionSource XianbiSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal bool SunHongTargetBlocked(int owner, int target) => _coreProgramEventHistoryOperations.ProgramEventHistory<ZenrunTargetBlockedEvent>().Any(e => e.OwnerSeat == owner && e.TargetSeat == target && e.Source.OwnerSeat == owner && e.Source.SkillId == "ol:zenrun");
    internal bool XianbiCanDiscard(int owner, Card card) => !card.IsGeneralWeapon && !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(owner, card, CardLocation.Hand(owner), OwnedCardMoveIntent.Discard);
    internal bool XianbiTargetLegal(int owner, int target)
    {
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(owner) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || !_coreMatchParticipantsPlayersAccess._players[owner].IsAlive || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || SunHongTargetBlocked(owner, target))
            return false;
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]);
        var equipment = _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[target]).Count;
        return equipment != hand.Count && (equipment > hand.Count || hand.Count(c => XianbiCanDiscard(owner, c)) >= hand.Count - equipment);
    }

    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 27570, SkillProgramEffectOp.ResolveXianbiHandAdjustmentAndRandomReturns)]
    internal bool CanActivateXianbi(CharacterState owner) => _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && owner.IsAlive && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat == owner.Seat && _coreMatchParticipantsPlayersAccess._players.Any(p => XianbiTargetLegal(owner.Seat, p.Seat));
    SkillProgramStepOutcome global::CardGame.Core.ISunHongXianbiHost.Resolve(ProgramSkillFrame frame) => BeginXianbi(frame);
    internal SkillProgramStepOutcome BeginXianbi(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.Xianbi is not null || f.InstructionIndex != 1 || f.SkillId != "ol:xianbi" || global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f) != "adjust-and-return" || f.TriggerId is not null || f.WindowContext is not null || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || !CanActivateXianbi(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]) || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
            throw new InvalidOperationException("Xianbi needs its own real live Play activation and an unequal legal role.");
        var r = new XianbiReceipt(XianbiSource(f), f.GameplayHash, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, XianbiStage.Target)
        {
            TargetSeat = f.OwnerSeat
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Xianbi = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XianbiStartedEvent(f.Id, r.Source, r.GameplayHash, r.ActualTurn, r.TurnOwner, r.PlayPhase));
        PublishXianbi(f);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal PromptChoice XianbiChoice(ProgramSkillFrame f, string branch, string description, int? card = null, int? target = null) => new(new($"xianbi.{(f.Id)}.{(f.Xianbi!.Stage)}.{(f.Xianbi.Revision)}.{(branch)}.{(card ?? target ?? -1)}"), description, card is { } id ? [id] : [], target is { } seat ? [seat] : [], new Dictionary<string, string> { ["program-action"] = "sun-hong-xianbi", ["branch"] = branch });
    internal IReadOnlyList<PromptChoice> XianbiChoices(ProgramSkillFrame f)
    {
        var r = f.Xianbi!;
        var choices = new List<PromptChoice>();
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            return choices;
        if (r.Stage == XianbiStage.Target)
            choices.AddRange(_coreMatchParticipantsPlayersAccess._players.Where(p => XianbiTargetLegal(f.OwnerSeat, p.Seat)).Select(p => XianbiChoice(f, "target", $"将手牌调整至{(p.Name)}当前装备数（{(_corePhysicalOwnedCardQueriesOperations.GetEquipment(p).Count)}张）", target: p.Seat)));
        if (r.Stage == XianbiStage.DiscardChoice)
        {
            if (r.Selected.Count < r.Required)
                foreach (var card in r.Hand.Where(c => !r.Selected.Contains(c.CardId) && _cardZonesOperations._cardZones.Contains(CardLocation.Hand(f.OwnerSeat), c.CardId) && XianbiCanDiscard(f.OwnerSeat, _coreAdvancedLifecycleOperations.GetAdvancedCard(c.CardId))))
                    choices.Add(XianbiChoice(f, "pick", "选择弃置【" + _coreAdvancedLifecycleOperations.GetAdvancedCard(card.CardId).DisplayName + "】", card.CardId));
            foreach (var id in r.Selected)
                choices.Add(XianbiChoice(f, "unpick", "撤回【" + _coreAdvancedLifecycleOperations.GetAdvancedCard(id).DisplayName + "】", id));
            if (r.Selected.Count == r.Required)
                choices.Add(XianbiChoice(f, "pay", $"整次弃置这{(r.Required)}张手牌"));
        }

        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishXianbi(ProgramSkillFrame f)
    {
        var choices = XianbiChoices(f);
        if (choices.Count == 0)
        {
            FinishXianbi(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.AwaitingResponse });
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "险诐：选择角色或本人的整次弃牌。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("sun-hong-xianbi")]
    internal void AnswerXianbi(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Xianbi: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !XianbiChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Xianbi needs its current exact own private choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var branch = choice.Parameters["branch"];
        if (r.Stage == XianbiStage.Target)
        {
            var target = choice.Targets.Single();
            if (branch != "target" || !XianbiTargetLegal(f.OwnerSeat, target))
                throw new InvalidOperationException("Xianbi cannot change the accepted target.");
            var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Select(c => new XianbiEntity(c.Id, c.Kind, _coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], c))).ToArray();
            var equipment = _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[target]).Count;
            r = r with
            {
                Issued = true,
                TargetSeat = target,
                HandBefore = hand.Length,
                EquipmentCount = equipment,
                Required = Math.Abs(hand.Length - equipment),
                Hand = hand,
                Stage = hand.Length > equipment ? XianbiStage.DiscardChoice : XianbiStage.DrawRequest
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Xianbi = r });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XianbiTargetFrozenEvent(f.Id, target, hand.Length, equipment));
            for (var i = 0; i < hand.Length; i++)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XianbiHandEntityEvent(f.Id, i, hand[i].CardId, hand[i].Kind, hand[i].EffectiveKind));
            if (hand.Length > equipment)
                PublishXianbi(f);
            else
                DrawXianbiIncrease(f);
            return;
        }

        if (r.Stage != XianbiStage.DiscardChoice)
            throw new InvalidOperationException("Xianbi is not selecting a discard.");
        if (branch is "pick" or "unpick")
        {
            var id = choice.Cards.Single();
            var selected = branch == "pick" ? r.Selected.Append(id).ToArray() : r.Selected.Where(c => c != id).ToArray();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Xianbi = r with { Selected = selected, Revision = r.Revision + 1 } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XianbiSelectionChangedEvent(f.Id, r.Revision + 1, id, branch == "pick"));
            PublishXianbi(f);
            return;
        }

        if (branch != "pay" || r.Selected.Count != r.Required || r.Selected.Any(id => !_cardZonesOperations._cardZones.Contains(CardLocation.Hand(f.OwnerSeat), id) || !XianbiCanDiscard(f.OwnerSeat, _coreAdvancedLifecycleOperations.GetAdvancedCard(id))))
            throw new InvalidOperationException("Xianbi must pay the frozen complete Hand difference exactly once.");
        var primary = r.Selected.Select(id => r.Hand.Single(c => c.CardId == id)).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Xianbi = r with { Primary = primary } });
        for (var i = 0; i < primary.Length; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XianbiPrimaryCommittedEvent(f.Id, i, primary[i].CardId, primary[i].Kind, primary[i].EffectiveKind));
        f = PrepareXianbiMovement(f, XianbiStage.DiscardChildren);
        var frameId = f.Id;
        var step = f.Xianbi!.MovementStep;
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, primary.Select(c => c.CardId).ToArray(), new(XianbiDiscardReason), (batch, moves) =>
        {
            foreach (var move in moves)
                RecordXianbiMove(frameId, step, batch, move);
        });
        CompleteXianbiMovement(frameId, primary.Length);
        ContinueXianbi(frameId);
    }

    internal ProgramSkillFrame PrepareXianbiMovement(ProgramSkillFrame f, XianbiStage stage)
    {
        foreach (var player in _coreMatchParticipantsPlayersAccess._players)
            _observedSkillGrantRevisionsOperations._observedSkillGrantRevisions.TryAdd(player.Seat, player.SkillGrants.Revision);
        var r = f.Xianbi!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Xianbi = r with { Stage = stage, MovementStep = r.MovementStep + 1, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, ActualCount = 0 }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        return f;
    }

    internal void CompleteXianbiMovement(long id, int count)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.Xianbi!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Xianbi = r with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, ActualCount = count } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XianbiMovementIssuedEvent(id, r.MovementStep, r.Stage, r.Cursor, r.Before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, count));
    }

    internal void RecordXianbiMove(long frameId, int step, long batch, CardMovementRecord move) => _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XianbiEntityMovedEvent(frameId, step, batch, move.CardId, move.CardKind, move.From, move.To, move.Reason, move.Sequence));
    internal void DrawXianbiIncrease(ProgramSkillFrame f)
    {
        var r = f.Xianbi!;
        if (r.Stage != XianbiStage.DrawRequest || f.NativeDraw is not null || r.Required <= 0)
            throw new InvalidOperationException("Xianbi cannot repeat a frozen native draw request.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Xianbi = r with { Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XianbiDrawRequestedEvent(f.Id, f.OwnerSeat, r.Required, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, r.Required, new(XianbiDrawReason));
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveXianbiHandAdjustmentAndRandomReturns, SkillProgramEffectOp.ResolveZenrunNativeDrawReplacement)]
    internal void ReturnSunHongDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        if (f.Zenrun is not null)
        {
            ReturnZenrunCompensation(f, outcome);
            return;
        }

        if (f.Xianbi is not { Stage: XianbiStage.DrawRequest, Issued: true } r || f.NativeDraw is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != f.Id || outcome.RecipientSeat != f.OwnerSeat || outcome.RequestedCount != r.Required || outcome.SequenceBefore < r.Before || outcome.SequenceAfter < outcome.SequenceBefore || outcome.SequenceAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || outcome.ActualDrawIds.Count > r.Required || outcome.ActualDrawIds.Distinct().Count() != outcome.ActualDrawIds.Count || outcome.Replacement is not null && outcome.ActualDrawIds.Count != 0 || _coreProgramEventHistoryOperations.ProgramEventHistory<XianbiDrawRequestedEvent>().Count(e => e.FrameId == f.Id && e.RecipientSeat == f.OwnerSeat && e.RequestedCount == r.Required && e.SequenceBefore == r.Before) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<XianbiDrawReturnedEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("Xianbi lost its exact pending draw request or ordinary-versus-replaced native return.");
        if (outcome.AttemptFrameId is { } attempt && _coreProgramEventHistoryOperations.ProgramEventHistory<NativeDrawAttemptDecidedEvent>().Count(e => e.AttemptFrameId == attempt && e.ParentFrameId == f.Id && e.RecipientSeat == f.OwnerSeat && e.RequestedCount == r.Required && e.Replaced == (outcome.Replacement is not null)) != 1)
            throw new InvalidOperationException("Xianbi cannot accept a different native draw attempt.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Xianbi = r with { Stage = XianbiStage.DrawChildren, Before = checked((int)outcome.SequenceBefore), After = checked((int)outcome.SequenceAfter), MovementStep = r.MovementStep + 1, ActualCount = outcome.ActualDrawIds.Count }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        for (var i = 0; i < outcome.ActualDrawIds.Count; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XianbiDrawnEntityEvent(f.Id, i, outcome.ActualDrawIds[i]));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XianbiDrawReturnedEvent(f.Id, outcome.AttemptFrameId, outcome.SequenceBefore, outcome.SequenceAfter, outcome.ActualDrawIds.Count, outcome.ReplacementGainedIds.Count));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XianbiMovementIssuedEvent(f.Id, f.Xianbi!.MovementStep, XianbiStage.DrawChildren, r.Cursor, f.Xianbi.Before, f.Xianbi.After, outcome.ActualDrawIds.Count));
        ContinueXianbi(f.Id);
    }

    internal bool DrainXianbi(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    internal void ContinueXianbi(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Xianbi: { } r } f || f.Id != id)
            return;
        if (r.Stage == XianbiStage.DrawRequest)
            return;
        if (r.Stage is XianbiStage.Target or XianbiStage.DiscardChoice)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishXianbi(f);
            return;
        }

        if (XianbiChildren(r.Stage))
        {
            if (DrainXianbi(f))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.Xianbi!;
            if (r.Stage == XianbiStage.DrawChildren)
            {
                FinishXianbi(f);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null, Xianbi = r with { Stage = XianbiStage.NextReturn, Cursor = r.Cursor + (r.Stage == XianbiStage.ObtainChildren ? 1 : 0), ChosenCard = null, Eligible = [] } });
            r = f.Xianbi!;
        }

        while (r.Stage == XianbiStage.NextReturn && r.Cursor < r.Primary.Count && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            var original = r.Primary[r.Cursor];
            var category = CardCatalog.Get(original.EffectiveKind).CategoryName;
            var pool = _cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile).Where(c => c.Id != original.CardId && CardCatalog.Get(c.Kind).CategoryName == category).OrderBy(c => c.Id).ToArray();
            if (pool.Length == 0)
            {
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XianbiReturnSkippedEvent(id, r.Cursor, original.CardId));
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Xianbi = r with { Cursor = r.Cursor + 1 } });
                r = f.Xianbi!;
                continue;
            }

            var before = _randomOperations._random.State;
            var index = _randomOperations._random.Next(pool.Length);
            var card = pool[index];
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Xianbi = r with { Eligible = pool.Select(c => c.Id).ToArray(), ChosenCard = card.Id, RandomBefore = before, RandomAfter = _randomOperations._random.State, RandomIndex = index } });
            for (var i = 0; i < pool.Length; i++)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XianbiRandomCandidateEvent(id, r.Cursor, i, pool[i].Id));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XianbiRandomIssuedEvent(id, r.Cursor, original.CardId, card.Id, index, pool.Length, before, _randomOperations._random.State));
            f = PrepareXianbiMovement(f, XianbiStage.ObtainChildren);
            var step = f.Xianbi!.MovementStep;
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([card.Id], CardLocation.Hand(f.OwnerSeat), new(XianbiObtainReason), (batch, moves) =>
            {
                foreach (var move in moves)
                    RecordXianbiMove(id, step, batch, move);
            });
            CompleteXianbiMovement(id, 1);
            ContinueXianbi(id);
            return;
        }

        FinishXianbi(f);
    }

    internal void FinishXianbi(ProgramSkillFrame f)
    {
        var r = f.Xianbi!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XianbiCompletedEvent(f.Id, r.Issued, r.Primary.Count, r.Cursor));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Xianbi = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, r.Issued);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 27570)]
    internal bool ResumeXianbi(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Xianbi: not null, NativeDraw: null } f || f.Id != id)
            return false;
        ContinueXianbi(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 27570)]
    internal bool ReturnXianbiMovement(ProgramSkillFrame f)
    {
        if (f.Xianbi is null || f.NativeDraw is not null)
            return false;
        ContinueXianbi(f.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27570)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27570)]
    internal bool ContinueIssuedXianbi(ProgramSkillFrame f) => f.Xianbi is { Issued: true } && ValidXianbi(f);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveXianbiHandAdjustmentAndRandomReturns)]
    internal PromptChoice ChooseXianbiAi(PendingDecision prompt, ProgramSkillFrame f)
    {
        if (f.Xianbi!.Stage == XianbiStage.Target)
            return prompt.Choices.OrderByDescending(c => _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[c.Targets.Single()]).Count > _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count ? 100 + _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[c.Targets.Single()]).Count : _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count - _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[c.Targets.Single()]).Count).First();
        return prompt.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "pay") ?? prompt.Choices.Where(c => c.Parameters.GetValueOrDefault("branch") == "pick").OrderBy(c => global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards.Single()), _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])).First();
    }
}
