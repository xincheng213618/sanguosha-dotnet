#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuanJieMobileShiRuntime
{
    internal const string ShiHjGongmou = "mobile:shi-gongmou", ShiHjZhengshuo = "mobile:shi-zhengshuo";
    internal const string ShiHjExchange = "skill-program.mobile-shi-huan-jie.exchange", ShiHjDiscard = "skill.mobile-shi-huan-jie.discard", ShiHjDraw = "skill-program.mobile-shi-huan-jie.draw";
    internal static CardConversionSource ShiHjSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    SkillProgramStepOutcome global::CardGame.Core.IMobileShiHuanJieHost.Begin(ProgramSkillFrame frame, SkillProgramEffectOp op) => BeginShiHj(frame, op);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 48300)]
    internal bool CanOfferShiHj(ProgramTriggerLegalityContext q) => q.SkillId != ShiHjGongmou || _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != q.OwnerSeat);
    internal SkillProgramStepOutcome BeginShiHj(ProgramSkillFrame input, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        var exchange = op == SkillProgramEffectOp.ResolveMobileShiHuanJieGongmou;
        var r = new MobileShiHuanJieReceipt(ShiHjSource(f), f.GameplayHash, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, f.WindowContext?.ParentFrameId ?? f.ParentFrameId ?? 0, op, exchange ? MobileShiHuanJieStage.Target : MobileShiHuanJieStage.DiscardChildren)
        {
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiHuanJie = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileShiHuanJieStartedEvent(f.Id, r.Source, r.GameplayHash, r.ActualTurn, r.ActualTurnOwner, r.ParentId, op));
        if (exchange)
        {
            PublishShiHj(f);
            return SkillProgramStepOutcome.AwaitChoice;
        }

        var recipients = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).OrderBy(p => _coreTableOrderOperations.RelativeTableOrder(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, p.Seat)).Select(p => p.Seat).ToArray();
        var materials = recipients.SelectMany(seat => _coreTurnHandCategoryRestrictionsOperations.GetSelfDiscardableOwnedHand(_coreMatchParticipantsPlayersAccess._players[seat]).Select(card => new MobileShiHuanJieMaterial(card.Id, card.Kind, CardLocation.Hand(seat), CardLocation.DiscardPile))).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiHuanJie = r with { Recipients = recipients, Materials = materials }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        // All players lose their original hand in one physical rules batch. Each
        // owner still receives its own native logical-discard invoice.
        var drafts = recipients.Select(seat => _coreNativeLogicalDiscardsOperations.CaptureNativeLogicalDiscardInvocation(f.OwnerSeat, seat, materials.Where(m => m.From.OwnerSeat == seat).Select(m => m.CardId).ToArray(), new(ShiHjDiscard), f.Id)).ToArray();
        if (materials.Length > 0)
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(materials.Select(m => m.CardId).ToArray(), CardLocation.DiscardPile, new(ShiHjDiscard), disclosure: CardMovementDisclosure.FaceUp);
        foreach (var draft in drafts)
            _coreNativeLogicalDiscardsOperations.CompleteNativeLogicalDiscardInvocation(draft);
        SealShiHj(f.Id);
        ContinueShiHj(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal IReadOnlyList<PromptChoice> ShiHjChoices(ProgramSkillFrame f) => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat).Select(p => new PromptChoice(new($"shi-huan-jie.{(f.Id)}.target.{(p.Seat)}"), $"与 {(p.Name)} 交换全部手牌（其有{(_corePhysicalOwnedCardQueriesOperations.GetHand(p).Count)}张）", [], [p.Seat], new Dictionary<string, string> { ["program-action"] = "mobile-shi-huan-jie", ["branch"] = "target" })).ToArray());
    internal void PublishShiHj(ProgramSkillFrame f)
    {
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        var choices = ShiHjChoices(f);
        if (choices.Count == 0)
        {
            FinishShiHj(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "共谋：选择交换全部手牌的其他角色。", [], choices.SelectMany(c => c.Targets).ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("mobile-shi-huan-jie")]
    internal void AnswerShiHj(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MobileShiHuanJie.Stage: MobileShiHuanJieStage.Target } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !ShiHjChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Gongmou requires its exact current living target choice.");
        RequireShiHj(f);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var target = choice.Targets.Single();
        var ownerHand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).ToArray();
        var targetHand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).ToArray();
        var materials = ownerHand.Select(c => new MobileShiHuanJieMaterial(c.Id, c.Kind, CardLocation.Hand(f.OwnerSeat), CardLocation.Hand(target))).Concat(targetHand.Select(c => new MobileShiHuanJieMaterial(c.Id, c.Kind, CardLocation.Hand(target), CardLocation.Hand(f.OwnerSeat)))).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect, MobileShiHuanJie = f.MobileShiHuanJie!with { Stage = MobileShiHuanJieStage.ExchangeChildren, Target = target, Materials = materials, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileShiHuanJieTargetChosenEvent(f.Id, target, ownerHand.Length, targetHand.Length));
        if (materials.Length > 0)
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(materials.Select(m => m.CardId).ToArray(), CardLocation.Hand(target), new(ShiHjExchange), destinationForCard: (_, source) => CardLocation.Hand(source.OwnerSeat == f.OwnerSeat ? target : f.OwnerSeat), disclosure: CardMovementDisclosure.FaceDown);
        SealShiHj(f.Id);
        ContinueShiHj(f.Id);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal void SealShiHj(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.MobileShiHuanJie!;
        var moves = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > r.Before && m.Sequence <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence).ToArray();
        var primary = moves.Where(m => r.Materials.Any(p => p.CardId == m.CardId && p.From == m.From)).ToArray();
        foreach (var move in primary)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileShiHuanJieMaterialPaidEvent(id, r.Stage, r.Materials.Single(m => m.CardId == move.CardId), move.Sequence));
        var batch = _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.SingleOrDefault(b => b.ParentFrameId == id && b.ParentBatchId is null && primary.Any(m => b.Movements.Contains(m)))?.Id;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MobileShiHuanJie = r with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, BatchId = batch } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileShiHuanJieMovementPaidEvent(id, r.Stage, r.Cursor, batch, r.Before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, primary.Length));
    }

    internal void GrantShiHjPaidTurnSkills(ProgramSkillFrame f)
    {
        RequireShiHj(f);
        var r = f.MobileShiHuanJie!;
        if (r.Operation != SkillProgramEffectOp.ResolveMobileShiHuanJieGongmou || r.Stage != MobileShiHuanJieStage.GrantChildren || f.PendingMovementContinuation is not null || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.Target) || _coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiHuanJieTargetChosenEvent>().Count(e => e.FrameId == f.Id && e.TargetSeat == r.Target) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiHuanJieMovementPaidEvent>().Count(e => e.FrameId == f.Id && e.Stage == MobileShiHuanJieStage.ExchangeChildren && e.Before == r.Before && e.After == r.After && e.BatchId == r.BatchId) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramTurnSkillsGrantedEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("Gongmou cannot grant before its exact complete real-hand exchange or repeat its paid grants.");
        var expiry = new SkillGrantTurnExpiry(r.ActualTurn, r.ActualTurnOwner);
        _coreProgramSkillChangesOperations.GrantIssuedProgramTurnSkills(f, f.OwnerSeat, ["classic:qice"], expiry);
        _coreProgramSkillChangesOperations.GrantIssuedProgramTurnSkills(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), r.Target, ["classic:kanpo"], expiry);
    }

    internal bool DrainShiHj(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.AwaitedProgramMovement);
    internal void ContinueShiHj(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MobileShiHuanJie: { } r } f || f.Id != id || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            return;
        RequireShiHj(f);
        if (r.Stage == MobileShiHuanJieStage.Target)
        {
            PublishShiHj(f);
            return;
        }

        if (r.Stage is MobileShiHuanJieStage.Drawing or MobileShiHuanJieStage.Complete)
            return;
        if (DrainShiHj(id))
            return;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { PendingMovementContinuation = null });
        r = f.MobileShiHuanJie!;
        if (r.Stage == MobileShiHuanJieStage.ExchangeChildren)
        {
            // The exchange already issued both grants. Losing Gongmou during
            // its paid child does not revoke that original turn obligation.
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiHuanJie = r with { Stage = MobileShiHuanJieStage.GrantChildren } });
            GrantShiHjPaidTurnSkills(f);
            ContinueShiHj(id);
            return;
        }

        if (r.Stage == MobileShiHuanJieStage.GrantChildren)
        {
            FinishShiHj(f);
            return;
        }

        if (r.Stage == MobileShiHuanJieStage.DiscardChildren)
        {
            var materials = _cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile).Select(c => new MobileShiHuanJieMaterial(c.Id, c.Kind, CardLocation.DiscardPile, CardLocation.DrawPile)).ToArray();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiHuanJie = r with { Stage = MobileShiHuanJieStage.ShuffleChildren, Materials = materials, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, BatchId = null }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _corePhysicalMovementFacadesOperations.MoveAllCards(CardLocation.DiscardPile, CardLocation.DrawPile, CardMoveReasons.Reshuffle);
            var before = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Select(c => c.Id).ToArray();
            var random = _randomOperations._random.State;
            _cardZonesOperations._cardZones.Shuffle(CardLocation.DrawPile, _randomOperations._random);
            var after = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Select(c => c.Id).ToArray();
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiHuanJie = f.MobileShiHuanJie!with { ShuffleBefore = before, ShuffleAfter = after, RandomBefore = random, RandomAfter = _randomOperations._random.State } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileShiHuanJieShuffledEvent(id, random, _randomOperations._random.State, before.Length));
            SealShiHj(id);
            ContinueShiHj(id);
            return;
        }

        if (r.Stage == MobileShiHuanJieStage.DrawChildren)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiHuanJie = r with { Cursor = r.Cursor + 1, Draw = null } });
            r = f.MobileShiHuanJie!;
        }

        if (r.Stage is MobileShiHuanJieStage.ShuffleChildren or MobileShiHuanJieStage.DrawChildren)
        {
            while (r.Cursor < r.Recipients.Count && !_coreMatchParticipantsPlayersAccess._players[r.Recipients[r.Cursor]].IsAlive)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiHuanJie = r with { Cursor = r.Cursor + 1 } });
                r = f.MobileShiHuanJie!;
            }

            if (r.Cursor >= r.Recipients.Count || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                FinishShiHj(f);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiHuanJie = r with { Stage = MobileShiHuanJieStage.Drawing, Draw = null, Materials = [], Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, BatchId = null } });
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, r.Recipients[r.Cursor], 4, new(ShiHjDraw));
        }
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveMobileShiHuanJieZhengshuo)]
    internal void ReturnShiHjDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        if (f.MobileShiHuanJie is not { Stage: MobileShiHuanJieStage.Drawing, Draw: null } r || r.Cursor >= r.Recipients.Count || outcome.RecipientSeat != r.Recipients[r.Cursor] || outcome.RequestedCount != 4)
            throw new InvalidOperationException("Zhengshuo draw lost its once-issued original recipient and four-card request.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiHuanJie = r with { Stage = MobileShiHuanJieStage.DrawChildren, Draw = outcome, Before = outcome.SequenceBefore, After = outcome.SequenceAfter }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileShiHuanJieDrawCompletedEvent(f.Id, r.Cursor, outcome.RecipientSeat, outcome.ActualDrawIds.Count, outcome.Replacement is not null));
        ContinueShiHj(f.Id);
    }

    internal void FinishShiHj(ProgramSkillFrame f)
    {
        var r = f.MobileShiHuanJie!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiHuanJie = r with { Stage = MobileShiHuanJieStage.Complete }, PendingMovementContinuation = null });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileShiHuanJieCompletedEvent(f.Id, r.Operation, r.Target));
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 48300)]
    internal bool ResumeShiHj(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MobileShiHuanJie: not null } f || f.Id != id)
            return false;
        ContinueShiHj(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 48300)]
    internal bool ReturnShiHjMovement(ProgramSkillFrame f)
    {
        if (f.MobileShiHuanJie is null)
            return false;
        ContinueShiHj(f.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 48300)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 48300)]
    internal bool KeepPaidShiHj(ProgramSkillFrame f) => f.MobileShiHuanJie is { Stage: not (MobileShiHuanJieStage.Target or MobileShiHuanJieStage.Complete) };
    internal bool ShiHjMovementProof(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => f.MobileShiHuanJie is { } r && effect?.Op == r.Operation && r.Stage is MobileShiHuanJieStage.ExchangeChildren or MobileShiHuanJieStage.DiscardChildren or MobileShiHuanJieStage.ShuffleChildren or MobileShiHuanJieStage.DrawChildren && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null;
    [ProgramAiChoice(SkillProgramEffectOp.ResolveMobileShiHuanJieGongmou)]
    internal PromptChoice ChooseShiHjAi(PendingDecision p, ProgramSkillFrame f) => p.Choices.OrderByDescending(c => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[c.Targets.Single()]).Count).First();
    internal void RequireShiHj(ProgramSkillFrame f)
    {
        var r = f.MobileShiHuanJie ?? throw new InvalidOperationException("Shi Huan Jie lost its owning receipt.");
        if (r.Source != ShiHjSource(f) || r.GameplayHash != f.GameplayHash || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash || f.InstructionIndex != 1 || r.ActualTurn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || r.ActualTurnOwner != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || f.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || r.ParentId != (f.WindowContext?.ParentFrameId ?? f.ParentFrameId ?? 0) || !Enum.IsDefined(r.Stage) || _coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiHuanJieStartedEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.ActualTurn == r.ActualTurn && e.ActualTurnOwner == r.ActualTurnOwner && e.ParentId == r.ParentId && e.Operation == r.Operation) != 1 || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.Cursor < 0 || r.Cursor > r.Recipients.Count || r.Recipients.Distinct().Count() != r.Recipients.Count || r.Recipients.Any(seat => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(seat)) || r.Materials.Select(m => m.CardId).Distinct().Count() != r.Materials.Count || r.Operation == SkillProgramEffectOp.ResolveMobileShiHuanJieGongmou && r.Stage != MobileShiHuanJieStage.Target && (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.Target) || r.Target == f.OwnerSeat))
            throw new InvalidOperationException("Shi Huan Jie changed its original source, paid instruction, actual turn or native material cursor.");
        if (r.ShuffleBefore.Count > 0 || _coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiHuanJieShuffledEvent>().Any(e => e.FrameId == f.Id))
        {
            var order = r.ShuffleBefore.ToList();
            var random = new DeterministicRandom(unchecked((int)r.RandomBefore));
            random.Shuffle(order);
            if (!order.SequenceEqual(r.ShuffleAfter) || random.State != r.RandomAfter || _coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiHuanJieShuffledEvent>().Count(e => e.FrameId == f.Id && e.RandomBefore == r.RandomBefore && e.RandomAfter == r.RandomAfter && e.DeckCount == order.Count) != 1)
                throw new InvalidOperationException("Zhengshuo lost its once-issued real complete-deck shuffle and deterministic RNG.");
        }
    }
}
