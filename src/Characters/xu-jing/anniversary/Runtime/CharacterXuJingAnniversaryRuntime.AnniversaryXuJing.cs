#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXuJingAnniversaryRuntime
{
    internal static CardConversionSource AnniversaryXuJingSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal bool HasAnniversaryXuJing => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveAnniversaryXuJingShangyu) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveAnniversaryXuJingCaixia);

    internal AnniversaryXuJingMarkedEvent? AnniversaryXuJingMark(int owner, string? instance = null) => _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryXuJingMarkedEvent>().LastOrDefault(e => e.Source.OwnerSeat == owner && (instance is null || e.Source.SkillInstanceId == instance));
    internal AnniversaryXuJingCaixiaIssuedEvent? AnniversaryXuJingDebt(int owner) => _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryXuJingCaixiaIssuedEvent>().LastOrDefault(e => e.Source.OwnerSeat == owner);
    internal int AnniversaryXuJingRemaining(int owner) => AnniversaryXuJingDebt(owner)is not { } debt ? 0 : Math.Max(0, debt.Count - _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryXuJingCaixiaUseEvent>().Count(e => e.DebtFrameId == debt.FrameId));
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 43100)]
    internal void RecordAnniversaryXuJingActualUse(CurrentTurnCardUseKindsRecordedEvent fact)
    {
        if (!HasAnniversaryXuJing || AnniversaryXuJingDebt(fact.ActorSeat)is not { } debt || AnniversaryXuJingRemaining(fact.ActorSeat) == 0 || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryXuJingCaixiaUseEvent>().Any(e => e.DebtFrameId == debt.FrameId && e.OriginFrameId == fact.OriginFrameId && e.ActionId == fact.CardActionId && e.InstructionIndex == fact.ProgramInstructionIndex))
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryXuJingCaixiaUseEvent(debt.FrameId, fact.ActorSeat, fact.OriginFrameId, fact.CardActionId, fact.ProgramInstructionIndex));
    }

    internal int[] AnniversaryXuJingDiscardIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate) => AnniversaryXuJingMark(candidate.OwnerSeat, candidate.SkillInstanceId)is not { } mark ? [] : batch.Movements.Select((movement, index) => (movement, index)).Where(x => x.movement.CardId == mark.CardId && x.movement.To == CardLocation.DiscardPile && x.movement.From != x.movement.To).Select(x => x.index).ToArray();
    [ProgramPipelineHook(ProgramPipelinePhase.SelectDiscardBatchIndexes, 43100)]
    internal int[]? SelectAnniversaryXuJingDiscards(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, ProgramInstructionFeatures features) => features.HasOperation(SkillProgramEffectOp.ResolveAnniversaryXuJingShangyu) ? AnniversaryXuJingDiscardIndexes(batch, candidate) : null;
    internal global::CardGame.Core.GameEngine.IDamageAttempt? AnniversaryXuJingDamage(ProgramSkillWindowContext context) => _resolutionStackOperations._resolutionStack.OfType<DamageTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId && w.ParentFrameId == context.DamageFrameId)is { } window ? _coreNativeDamageHandlesOperations.GetDamageTriggerAttack(window) : null;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 43100, SkillProgramEffectOp.ResolveAnniversaryXuJingShangyu, SkillProgramEffectOp.ResolveAnniversaryXuJingCaixia)]
    internal bool CanOfferAnniversaryXuJing(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (candidate.SkillId == AnniversaryXuJingComposition.Caixia)
            return AnniversaryXuJingRemaining(candidate.OwnerSeat) == 0 && context.Amount > 0 && (context.SourceSeat == candidate.OwnerSeat || context.TargetSeat == candidate.OwnerSeat);
        if (trigger.Id == "opening-slash")
            return AnniversaryXuJingMark(candidate.OwnerSeat, candidate.SkillInstanceId)is null;
        if (AnniversaryXuJingMark(candidate.OwnerSeat, candidate.SkillInstanceId)is not { } mark)
            return false;
        if (trigger.Id == "marked-slash-discard")
            return context.MovementBatch is { } batch && context.MovementIndex is { } index && AnniversaryXuJingDiscardIndexes(batch, candidate).Contains(index) && _cardZonesOperations._cardZones.GetLocation(mark.CardId) == CardLocation.DiscardPile && AnniversaryXuJingRecipients(candidate.OwnerSeat, mark.CardId).Length > 0;
        return AnniversaryXuJingDamage(context)is { IsSourceLess: false, DamageWasApplied: true, EffectiveCardKind: { } kind } damage && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && damage.PhysicalCards.Any(c => c.Id == mark.CardId);
    }

    internal int[] AnniversaryXuJingRecipients(int owner, int card) => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && !_coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryXuJingGiftIssuedEvent>().Any(e => e.Source.OwnerSeat == owner && e.CardId == card && e.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && e.TurnOwner == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && e.TargetSeat == p.Seat)).OrderBy(p => _coreTableOrderOperations.RelativeTableOrder(owner, p.Seat)).Select(p => p.Seat).ToArray();
    SkillProgramStepOutcome global::CardGame.Core.IAnniversaryXuJingHost.Begin(ProgramSkillFrame frame, SkillProgramEffectOp operation) => BeginAnniversaryXuJing(frame, operation);
    internal SkillProgramStepOutcome BeginAnniversaryXuJing(ProgramSkillFrame supplied, SkillProgramEffectOp operation)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.AnniversaryXuJing is not null || f.InstructionIndex != 1 || f.WindowContext is not { } context)
            throw new InvalidOperationException("Xu Jing requires his original unissued trigger instruction.");
        var mode = operation == SkillProgramEffectOp.ResolveAnniversaryXuJingCaixia ? "caixia" : context.Window switch
        {
            SkillProgramTriggerWindow.GameStarting => "opening",
            SkillProgramTriggerWindow.DiscardPileReceived => "discard",
            _ => "damage"
        };
        var card = mode == "opening" ? 0 : AnniversaryXuJingMark(f.OwnerSeat, f.SkillInstanceId)?.CardId ?? 0;
        var user = mode == "damage" ? AnniversaryXuJingDamage(context)?.CardUserSeat ?? throw new InvalidOperationException("Shangyu requires its actual damaging Slash user.") : f.OwnerSeat;
        var r = new AnniversaryXuJingReceipt(AnniversaryXuJingSource(f), f.GameplayHash, context.ParentFrameId, mode, mode == "caixia" ? AnniversaryXuJingStage.CountChoice : mode == "damage" ? AnniversaryXuJingStage.DrawReady : AnniversaryXuJingStage.GiftChoice, card, user, context.DamageFrameId ?? 0);
        if (mode == "opening")
        {
            var candidates = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Where(c => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(c.Kind)).ToArray();
            if (candidates.Length > 0)
                r = r with
                {
                    CardId = candidates[_randomOperations._random.Next(candidates.Length)].Id,
                    Stage = AnniversaryXuJingStage.MovementChildren
                };
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryXuJing = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryXuJingStartedEvent(f.Id, r.Source, r.Hash, r.ParentId, mode, r.CardId, user, r.DamageId));
        if (mode == "opening")
        {
            if (r.CardId == 0)
            {
                _coreSkillProgramsOperations.FinishProgramSkill(f, true);
                return SkillProgramStepOutcome.AwaitChild;
            }

            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryXuJingMarkedEvent(f.Id, r.Source, r.CardId));
            MoveAnniversaryXuJingEntity(f, CardLocation.DrawPile, CardLocation.Hand(f.OwnerSeat));
        }

        ContinueAnniversaryXuJing(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal static CardMoveReason AnniversaryXuJingMoveReason => new("skill.anniversary.shangyu.entity");
    internal static CardMoveReason AnniversaryXuJingDrawReason => new("skill.anniversary.xujing.draw");

    internal void MoveAnniversaryXuJingEntity(ProgramSkillFrame supplied, CardLocation source, CardLocation destination)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var r = f.AnniversaryXuJing!;
        if (_cardZonesOperations._cardZones.GetLocation(r.CardId) != source)
            throw new InvalidOperationException("Shangyu cannot replace its original marked entity or current source.");
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryXuJing = r with { Stage = AnniversaryXuJingStage.MovementChildren, MovementSource = source, MovementDestination = destination, Before = before, After = before } });
        if (source != destination)
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([r.CardId], destination, AnniversaryXuJingMoveReason, (_, _) =>
            {
                var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                _coreRuntimeOperations.ReplaceRuntimeTop(current with { AnniversaryXuJing = current.AnniversaryXuJing!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            });
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        r = f.AnniversaryXuJing!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryXuJingMovedEvent(f.Id, r.CardId, source, destination, before, r.After));
        _ = _coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(f.Id, f.OwnerSeat);
    }

    internal IReadOnlyList<PromptChoice> AnniversaryXuJingChoices(ProgramSkillFrame f)
    {
        var r = f.AnniversaryXuJing!;
        if (r.Stage == AnniversaryXuJingStage.CountChoice)
            return Enumerable.Range(1, Math.Min(5, _coreMatchParticipantsPlayersAccess._players.Count)).Select(n => new PromptChoice(new($"anniversary-xujing.{(f.Id)}.draw.{(n)}"), $"摸{(n)}张牌，累计使用{(n)}张牌前不能再次发动才瑕", [], [], new Dictionary<string, string> { ["program-action"] = "anniversary-xujing", ["count"] = n.ToString() })).ToArray();
        if (r.Stage != AnniversaryXuJingStage.GiftChoice || _cardZonesOperations._cardZones.GetLocation(r.CardId) != (r.Mode == "opening" ? CardLocation.Hand(f.OwnerSeat) : CardLocation.DiscardPile))
            return[];
        return AnniversaryXuJingRecipients(f.OwnerSeat, r.CardId).Select(seat => new PromptChoice(new($"anniversary-xujing.{(f.Id)}.gift.{(seat)}"), $"将赏誉标记的【杀】交给{(_coreMatchParticipantsPlayersAccess._players[seat].Name)}", [], [seat], new Dictionary<string, string> { ["program-action"] = "anniversary-xujing", ["item"] = "gift" })).ToArray();
    }

    internal void PublishAnniversaryXuJing(ProgramSkillFrame f)
    {
        var choices = AnniversaryXuJingChoices(f);
        if (choices.Count == 0 || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            _coreSkillProgramsOperations.FinishProgramSkill(f, true);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, f.AnniversaryXuJing!.Stage == AnniversaryXuJingStage.GiftChoice ? "赏誉：选择本回合未指定过的角色。" : "才瑕：选择此次摸牌数量。", [], choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            Choices = Array.AsReadOnly(choices.ToArray()),
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("anniversary-xujing")]
    internal void AnswerAnniversaryXuJing(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryXuJing: { } r } f || !AnniversaryXuJingChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Xu Jing rejects a stale gift recipient or draw count.");
        RequireAnniversaryXuJing(f);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        if (r.Stage == AnniversaryXuJingStage.CountChoice)
        {
            var count = int.Parse(choice.Parameters["count"]);
            if (AnniversaryXuJingRemaining(f.OwnerSeat) != 0)
                throw new InvalidOperationException("Caixia is still waiting for actual card uses.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryXuJingCaixiaIssuedEvent(f.Id, r.Source, count));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryXuJing = r with { Count = count, Stage = AnniversaryXuJingStage.DrawReady } });
        }
        else
        {
            var target = choice.Targets.Single();
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryXuJingGiftIssuedEvent(f.Id, r.Source, r.CardId, target, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryXuJing = r with { TargetSeat = target } });
            MoveAnniversaryXuJingEntity(f, r.Mode == "opening" ? CardLocation.Hand(f.OwnerSeat) : CardLocation.DiscardPile, CardLocation.Hand(target));
        }

        ContinueAnniversaryXuJing(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 43100)]
    internal bool ContinueAnniversaryXuJing(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryXuJing: not null } root || root.Id != id)
            return false;
        while (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { AnniversaryXuJing: { } r } f && f.Id == id)
        {
            RequireAnniversaryXuJing(f);
            if (r.Stage is AnniversaryXuJingStage.CountChoice or AnniversaryXuJingStage.GiftChoice)
            {
                PublishAnniversaryXuJing(f);
                return true;
            }

            if (r.Stage == AnniversaryXuJingStage.Drawing)
                return true;
            if (r.Stage is AnniversaryXuJingStage.MovementChildren or AnniversaryXuJingStage.DrawChildren)
            {
                if (DrainAnniversaryXuJing(f))
                    return true;
                _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { PendingMovementContinuation = null });
                r = f.AnniversaryXuJing!;
                if (r.Stage == AnniversaryXuJingStage.MovementChildren && r.Mode == "opening" && r.TargetSeat < 0)
                {
                    _coreRuntimeOperations.ReplaceRuntimeTop(f with { AnniversaryXuJing = r with { Stage = AnniversaryXuJingStage.GiftChoice } });
                    continue;
                }

                if (r.Stage == AnniversaryXuJingStage.DrawChildren && r.Mode == "damage" && r.RewardIndex < 2)
                {
                    _coreRuntimeOperations.ReplaceRuntimeTop(f with { AnniversaryXuJing = r with { Stage = AnniversaryXuJingStage.DrawReady } });
                    continue;
                }

                _coreSkillProgramsOperations.FinishProgramSkill(f, true);
                return true;
            }

            var recipient = r.Mode == "caixia" || r.RewardIndex == 0 ? f.OwnerSeat : r.UserSeat;
            var count = r.Mode == "caixia" ? r.Count : 1;
            if (!_coreMatchParticipantsPlayersAccess._players[recipient].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                if (r.Mode == "damage" && r.RewardIndex == 0 && _coreMatchParticipantsPlayersAccess._players[r.UserSeat].IsAlive)
                {
                    _coreRuntimeOperations.ReplaceRuntimeTop(f with { AnniversaryXuJing = r with { RewardIndex = 1 } });
                    continue;
                }

                _coreSkillProgramsOperations.FinishProgramSkill(f, true);
                return true;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryXuJing = r with { TargetSeat = recipient, Count = count, RewardIndex = r.RewardIndex + 1, Stage = AnniversaryXuJingStage.Drawing } });
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, recipient, count, AnniversaryXuJingDrawReason);
            return true;
        }

        return true;
    }

    internal bool DrainAnniversaryXuJing(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, f.AnniversaryXuJing!.Stage == AnniversaryXuJingStage.MovementChildren ? PostEventContinuation.AwaitedProgramMovement : PostEventContinuation.Program) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, f.AnniversaryXuJing!.Stage == AnniversaryXuJingStage.MovementChildren ? PostEventContinuation.AwaitedProgramMovement : PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id);
    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveAnniversaryXuJingShangyu, SkillProgramEffectOp.ResolveAnniversaryXuJingCaixia)]
    internal void ReturnAnniversaryXuJingDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        if (f.AnniversaryXuJing is not { Stage: AnniversaryXuJingStage.Drawing } r || outcome.RecipientSeat != r.TargetSeat || outcome.RequestedCount != r.Count || outcome.SequenceAfter != _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || outcome.AttemptFrameId is { } attempt && r.Draws.Any(d => d.AttemptFrameId == attempt))
            throw new InvalidOperationException("Xu Jing lost his original once-issued native draw return.");
        var ordinal = r.RewardIndex;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { AnniversaryXuJing = r with { Stage = AnniversaryXuJingStage.DrawChildren, Draws = r.Draws.Append(outcome).ToArray(), Before = outcome.SequenceBefore, After = outcome.SequenceAfter }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryXuJingDrawReturnedEvent(f.Id, ordinal, outcome.RecipientSeat, outcome.RequestedCount, outcome.ActualDrawIds.Count, outcome.SequenceBefore, outcome.SequenceAfter, outcome.AttemptFrameId));
        ContinueAnniversaryXuJing(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 43100)]
    internal bool ReturnAnniversaryXuJingMovement(ProgramSkillFrame f) => f.AnniversaryXuJing is not null && ContinueAnniversaryXuJing(f.Id);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveAnniversaryXuJingShangyu)]
    internal PromptChoice ChooseAnniversaryXuJingGift(PendingDecision p, ProgramSkillFrame f) => p.Choices[0];
    [ProgramAiChoice(SkillProgramEffectOp.ResolveAnniversaryXuJingCaixia)]
    internal PromptChoice ChooseAnniversaryXuJingCount(PendingDecision p, ProgramSkillFrame f) => p.Choices[^1];
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 43100)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 43100)]
    internal bool RetainAnniversaryXuJingIssued(ProgramSkillFrame f) => f.AnniversaryXuJing is not null;
}
