#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterChenZhiMobileShiRuntime
{
    internal const string ShiCzQuanchong = "mobile:shi-quanchong", ShiCzRenxing = "mobile:shi-renxing";
    internal const string ShiCzDiscard = "skill.mobile-shi-chen-zhi.discard", ShiCzDraw = "skill-program.mobile-shi-chen-zhi.draw";
    internal static CardConversionSource ShiCzSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    SkillProgramStepOutcome global::CardGame.Core.IMobileShiChenZhiHost.Resolve(ProgramSkillFrame frame, SkillProgramEffectOp operation) => BeginShiCz(frame, operation);
    [ProgramCardUsePayment(ProgramCardUsePaymentStage.CaptureMovement, 47600)]
    internal void CaptureShiCzFirstDiscard(CardMovementBatchContext batch)
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMobileShiChenZhiRenxing) || batch.TurnNumber < 1 || batch.DiscardRecoveryPhase is { Kind: ActualDiscardRecoveryPhaseKind.Discard } || _coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiChenZhiFirstDiscardEvent>().Any(e => e.ActualTurn == batch.TurnNumber))
            return;
        var first = batch.Movements.FirstOrDefault(m => _coreProgramDiscardOriginsOperations.GetProgramDiscardSource(m)is { } from && global::CardGame.Core.EngineOwners.CoreGameEngineProgramDiscardOriginsRuntime.IsOwnedDiscardSource(from));
        if (first is not null)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileShiChenZhiFirstDiscardEvent(batch.TurnNumber, batch.Id, first.Sequence));
    }

    [ProgramPipelineHook(ProgramPipelinePhase.SelectDiscardBatchIndexes, 47600)]
    internal int[]? SelectShiCzFirstDiscard(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, ProgramInstructionFeatures features)
    {
        if (!features.HasOperation(SkillProgramEffectOp.ResolveMobileShiChenZhiRenxing))
            return null;
        var first = _coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiChenZhiFirstDiscardEvent>().SingleOrDefault(e => e.ActualTurn == batch.TurnNumber);
        if (first?.BatchId != batch.Id)
            return[];
        return batch.Movements.Select((m, i) => (m, i)).Where(x => x.m.Sequence == first.MovementSequence).Select(x => x.i).ToArray();
    }

    internal MobileShiChenZhiExtraGrantedEvent? ShiCzPendingExtra(int owner)
    {
        if (_coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.Kind != ActualTurnKind.Extra || _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat != owner || !_coreMatchParticipantsPlayersAccess._players[owner].IsAlive || _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramExtraTurnPendedEvent>().LastOrDefault()is not { SkillId: ShiCzQuanchong } last || last.Seat != owner)
            return null;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiChenZhiExtraGrantedEvent>().SingleOrDefault(e => e.FrameId == last.FrameId && e.Source.OwnerSeat == owner && e.Source.SkillId == ShiCzQuanchong && e.IssuedTurn == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber && e.GameplayHash == _contentRegistryOperations._contentRegistry.GetSkill(ShiCzQuanchong).Program!.GameplayHash && !_coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiChenZhiExtraEnteredEvent>().Any(x => x.GrantFrameId == e.FrameId));
    }

    // A paid extra-turn obligation survives loss of its original skill. It is
    // still bound to the last native pend and the original content hash.
    [ProgramPipelineHook(ProgramPipelinePhase.IncludeTriggerCandidate, 47600)]
    internal IReadOnlyList<ProgramTriggerCandidate> IncludeShiCzExtraStart(CharacterState owner, SkillProgramTriggerWindow window, IReadOnlyList<ProgramTriggerCandidate> ordinary)
    {
        if (window != SkillProgramTriggerWindow.TurnBeforeStart || !_contentRegistryOperations._contentRegistry.Skills.ContainsKey(ShiCzQuanchong))
            return ordinary;
        var rest = ordinary.Where(c => c.SkillId != ShiCzQuanchong || c.BindingId != "quanchong-extra-start").ToArray();
        var grant = ShiCzPendingExtra(owner.Seat);
        return grant is null ? Array.AsReadOnly(rest) : Array.AsReadOnly(rest.Append(new ProgramTriggerCandidate(owner.Seat, ShiCzQuanchong, "quanchong-extra-start", grant.Source.SkillInstanceId, grant.GameplayHash, 100)).ToArray());
    }

    [ProgramPipelineHook(ProgramPipelinePhase.IssuedTriggerCandidate, 47600)]
    internal bool IsIssuedShiCzExtraStart(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => candidate.SkillId == ShiCzQuanchong && candidate.BindingId == "quanchong-extra-start" && context.Window == SkillProgramTriggerWindow.TurnBeforeStart && ShiCzPendingExtra(candidate.OwnerSeat)is { } grant && grant.Source.SkillInstanceId == candidate.SkillInstanceId && grant.GameplayHash == candidate.GameplayHash && _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == context.ParentFrameId)is ProgramLifecycleTriggerWindowFrame parent && parent.Window == context.Window && parent.CandidateIndex < parent.Candidates.Count && parent.Candidates[parent.CandidateIndex] == candidate;
    internal bool ShiCzUsedOrPlayedSlash(int seat) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().Reverse().TakeWhile(e => e is not TurnStartedEvent).OfType<CardActionAcceptedEvent>().Any(e => e.Action.ActorSeat == seat && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(e.Action.EffectiveKind));
    internal IReadOnlyList<PromptChoice> ShiCzPaymentChoices(ProgramSkillFrame f, int holder) => _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, holder, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Discard, canSelect: (zone, card) => !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(f.OwnerSeat, card, new(zone, holder), OwnedCardMoveIntent.Discard));
    internal IReadOnlyList<int> ShiCzDiscardTargets(ProgramSkillFrame frame) => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && !ShiCzUsedOrPlayedSlash(p.Seat) && ShiCzPaymentChoices(frame, p.Seat).Count > 0).Select(p => p.Seat).ToArray();
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 47600)]
    internal bool CanOfferShiCz(ProgramTriggerLegalityContext gate)
    {
        if (gate.SkillId == ShiCzQuanchong)
            return gate.Trigger.Id == "quanchong-ending" ? gate.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat : ShiCzPendingExtra(gate.OwnerSeat)is not null;
        if (gate.SkillId != ShiCzRenxing)
            return true;
        return gate.WindowContext?.MovementBatch is { } batch && batch.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber && _coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiChenZhiFirstDiscardEvent>().SingleOrDefault(e => e.ActualTurn == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber)?.BatchId == batch.Id;
    }

    internal SkillProgramStepOutcome BeginShiCz(ProgramSkillFrame input, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.MobileShiChenZhi is not null || f.InstructionIndex != 1 || f.WindowContext is not { } context)
            throw new InvalidOperationException("Shi Chen Zhi needs the original unpaid native trigger instruction.");
        var stage = op == SkillProgramEffectOp.ResolveMobileShiChenZhiRenxing ? MobileShiChenZhiStage.ChooseEffect : f.TriggerId == "quanchong-extra-start" ? MobileShiChenZhiStage.ExtraStartChildren : MobileShiChenZhiStage.DiscardChildren;
        var r = new MobileShiChenZhiReceipt(ShiCzSource(f), f.GameplayHash, context.ParentFrameId, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, stage)
        {
            BeforeMovement = _coreCardMovementJournalMovementSequenceAccess._movementSequence,
            AfterMovement = _coreCardMovementJournalMovementSequenceAccess._movementSequence,
            FirstDiscardBatch = context.MovementBatch?.Id ?? 0
        };
        if (stage == MobileShiChenZhiStage.ChooseEffect)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiChenZhi = r });
            PublishShiCz(f);
            return SkillProgramStepOutcome.AwaitChoice;
        }

        if (stage == MobileShiChenZhiStage.ExtraStartChildren)
        {
            var grant = ShiCzPendingExtra(f.OwnerSeat) ?? throw new InvalidOperationException("Quanchong lost its exact pending extra-turn lease.");
            var lose = _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != f.OwnerSeat && p.Hp >= _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiChenZhi = r with { GrantFrameId = grant.FrameId } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileShiChenZhiExtraEnteredEvent(grant.FrameId, f.OwnerSeat, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber + 1, lose));
            ContinueShiCz(f.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        var cards = ShiCzPaymentChoices(f, f.OwnerSeat).SelectMany(c => c.Cards).Distinct().ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiChenZhi = r with { Cards = cards }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        if (cards.Length > 0)
            _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, cards, new(ShiCzDiscard));
        SealShiCzMovement(f.Id);
        ContinueShiCz(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal IReadOnlyList<PromptChoice> ShiCzChoices(ProgramSkillFrame f)
    {
        var r = f.MobileShiChenZhi!;
        PromptChoice C(string branch, string label, int? target = null) => new(new($"shi-chen-zhi.{(f.Id)}.{(r.Stage)}.{(branch)}.{(target ?? -1)}"), label, [], target is { } seat ? [seat] : [], new Dictionary<string, string> { ["program-action"] = "mobile-shi-chen-zhi", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch });
        if (r.Stage == MobileShiChenZhiStage.ChooseEffect)
        {
            var choices = new List<PromptChoice>
            {
                C("draw", r.TurnOwner == f.OwnerSeat ? "摸两张牌" : $"你与 {(_coreMatchParticipantsPlayersAccess._players[r.TurnOwner].Name)} 各摸一张牌")
            };
            if (ShiCzDiscardTargets(f).Count > 0)
                choices.Add(C("discard", "弃置一名本回合未使用或打出过【杀】的角色一张牌"));
            return Array.AsReadOnly(choices.ToArray());
        }

        if (r.Stage == MobileShiChenZhiStage.ChooseTarget)
            return Array.AsReadOnly(ShiCzDiscardTargets(f).Select(seat => C("target", "弃置 " + _coreMatchParticipantsPlayersAccess._players[seat].Name + " 的一张牌", seat)).ToArray());
        if (r.Stage != MobileShiChenZhiStage.ChooseCard)
            return[];
        return Array.AsReadOnly(ShiCzPaymentChoices(f, r.TargetSeat).Select(c => c with { Parameters = new Dictionary<string, string>(c.Parameters) { ["program-action"] = "mobile-shi-chen-zhi", ["branch"] = "card", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture) } }).ToArray());
    }

    internal void PublishShiCz(ProgramSkillFrame f)
    {
        var choices = ShiCzChoices(f);
        if (choices.Count == 0)
        {
            CompleteShiCz(f);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "任行：请选择效果或弃牌目标。", [], [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("mobile-shi-chen-zhi")]
    internal void AnswerShiCz(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MobileShiChenZhi: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !ShiCzChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Renxing lost its current native branch, target or opaque card slot.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var branch = choice.Parameters["branch"];
        if (r.Stage == MobileShiChenZhiStage.ChooseEffect)
        {
            if (branch == "discard")
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiChenZhi = r with { Stage = MobileShiChenZhiStage.ChooseTarget, Branch = branch } });
                PublishShiCz(f);
                return;
            }

            IssueShiCzDraw(f, f.OwnerSeat, MobileShiChenZhiStage.DrawOwnerChildren);
            ContinueShiCz(f.Id);
            return;
        }

        if (r.Stage == MobileShiChenZhiStage.ChooseTarget)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiChenZhi = r with { Stage = MobileShiChenZhiStage.ChooseCard, TargetSeat = choice.Targets.Single() } });
            PublishShiCz(f);
            return;
        }

        var cardId = choice.Cards.Count == 1 ? choice.Cards.Single() : _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat])[int.Parse(choice.Parameters["slot-index"], CultureInfo.InvariantCulture)].Id;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiChenZhi = r with { Stage = MobileShiChenZhiStage.ForeignDiscardChildren, CardId = cardId, BeforeMovement = _coreCardMovementJournalMovementSequenceAccess._movementSequence, AfterMovement = _coreCardMovementJournalMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, [cardId], new(ShiCzDiscard));
        SealShiCzMovement(f.Id);
        ContinueShiCz(f.Id);
    }

    internal void IssueShiCzDraw(ProgramSkillFrame input, int seat, MobileShiChenZhiStage stage)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        var r = f.MobileShiChenZhi!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiChenZhi = r with { Stage = stage, Branch = "draw", BeforeMovement = _coreCardMovementJournalMovementSequenceAccess._movementSequence, AfterMovement = _coreCardMovementJournalMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        if (_coreMatchParticipantsPlayersAccess._players[seat].IsAlive)
            _coreProgramLifecycleOperations.DrawProgramCards(f.Id, seat, 1, null, null, SkillProgramCardSetVisibility.Private, new(ShiCzDraw));
        SealShiCzMovement(f.Id);
    }

    internal void SealShiCzMovement(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _coreRuntimeOperations.ReplaceRuntimeFrame(id, f with { MobileShiChenZhi = f.MobileShiChenZhi!with { AfterMovement = _coreCardMovementJournalMovementSequenceAccess._movementSequence } });
    }

    internal bool DrainShiCz(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    internal void ContinueShiCz(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MobileShiChenZhi: { } r } f || f.Id != id || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            return;
        if (r.Stage is MobileShiChenZhiStage.ChooseEffect or MobileShiChenZhiStage.ChooseTarget or MobileShiChenZhiStage.ChooseCard)
        {
            PublishShiCz(f);
            return;
        }

        if (r.Stage == MobileShiChenZhiStage.ExtraStartChildren && _coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiChenZhiExtraEnteredEvent>().Single(e => e.GrantFrameId == r.GrantFrameId).LostHp && !_coreProgramEventHistoryOperations.ProgramEventHistory<ProgramSkillHpLostEvent>().Any(e => e.FrameId == f.Id && e.SkillId == f.SkillId))
        {
            // Identity appearance can suspend before the loss is actually paid.
            // A Dying return, by contrast, already carries the native loss fact.
            if (_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _corePrepPoisonOutcomesOperations.ApplyProgramHpLoss(f.Id, f.SkillId, f.OwnerSeat, 1) == SkillProgramStepOutcome.AwaitChild)
                return;
        }

        if (DrainShiCz(f))
            return;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { PendingMovementContinuation = null });
        r = f.MobileShiChenZhi!;
        if (r.Stage == MobileShiChenZhiStage.DiscardChildren && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None)
        {
            _coreSkillProgramsOperations.PendProgramExtraTurn(f, f.OwnerSeat);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileShiChenZhiExtraGrantedEvent(f.Id, r.Source, r.GameplayHash, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, _coreActualRoundProgressRoundNumberAccess._roundNumber));
        }

        if (r.Stage == MobileShiChenZhiStage.DrawOwnerChildren)
        {
            IssueShiCzDraw(f, r.TurnOwner, MobileShiChenZhiStage.DrawActorChildren);
            ContinueShiCz(id);
            return;
        }

        CompleteShiCz(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
    }

    internal void CompleteShiCz(ProgramSkillFrame f)
    {
        var r = f.MobileShiChenZhi!;
        if (f.SkillId == ShiCzRenxing)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileShiChenZhiRenxingCompletedEvent(f.Id, r.Source, r.ActualTurn, r.FirstDiscardBatch, r.Branch ?? "discard", r.TargetSeat));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShiChenZhi = r with { Stage = MobileShiChenZhiStage.Finished }, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 47600)]
    internal bool ResumeShiCz(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MobileShiChenZhi: not null } f || f.Id != id)
            return false;
        ContinueShiCz(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 47600)]
    internal bool ReturnShiCz(ProgramSkillFrame f)
    {
        if (f.MobileShiChenZhi is null)
            return false;
        ContinueShiCz(f.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 47600)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 47600)]
    internal bool PaidShiCzCanReturn(ProgramSkillFrame f) => f.MobileShiChenZhi?.Stage is MobileShiChenZhiStage.DiscardChildren or MobileShiChenZhiStage.ExtraStartChildren or MobileShiChenZhiStage.DrawOwnerChildren or MobileShiChenZhiStage.DrawActorChildren or MobileShiChenZhiStage.ForeignDiscardChildren;
    [ProgramMovementProof(ProgramMovementProofPhase.NativeCost, 47600)]
    internal bool ShiCzMovementProof(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation movement) => f.MobileShiChenZhi is { } r && r.Stage is MobileShiChenZhiStage.DiscardChildren or MobileShiChenZhiStage.DrawOwnerChildren or MobileShiChenZhiStage.DrawActorChildren or MobileShiChenZhiStage.ForeignDiscardChildren && effect?.Op is SkillProgramEffectOp.ResolveMobileShiChenZhiQuanchong or SkillProgramEffectOp.ResolveMobileShiChenZhiRenxing;
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 47600)]
    internal void AssertShiCzFrame(ProgramSkillFrame f)
    {
        if (f.MobileShiChenZhi is not { } r)
            return;
        if (r.Source != ShiCzSource(f) || r.GameplayHash != f.GameplayHash || f.WindowContext?.ParentFrameId != r.WindowId || r.ActualTurn != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber || r.BeforeMovement < 0 || r.AfterMovement < r.BeforeMovement || r.AfterMovement > _coreCardMovementJournalMovementSequenceAccess._movementSequence || !Enum.IsDefined(r.Stage) || r.Cards.Count != r.Cards.Distinct().Count() || f.SkillId == ShiCzRenxing && (r.FirstDiscardBatch <= 0 || !_coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiChenZhiFirstDiscardEvent>().Any(e => e.BatchId == r.FirstDiscardBatch && e.ActualTurn == r.ActualTurn)) || f.TriggerId == "quanchong-extra-start" && (r.GrantFrameId <= 0 || !_coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiChenZhiExtraEnteredEvent>().Any(e => e.GrantFrameId == r.GrantFrameId && e.OwnerSeat == f.OwnerSeat)))
            throw new InvalidOperationException("Shi Chen Zhi lost his original trigger, first-discard opportunity, extra-turn lease or finite paid continuation.");
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveMobileShiChenZhiRenxing)]
    internal PromptChoice ChooseShiCzAi(PendingDecision pending, ProgramSkillFrame f)
    {
        var r = f.MobileShiChenZhi!;
        if (r.Stage == MobileShiChenZhiStage.ChooseEffect)
            return pending.Choices.FirstOrDefault(c => c.Parameters["branch"] == "discard" && !global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[r.TurnOwner]) && ShiCzDiscardTargets(f).Any(s => !global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[s]))) ?? pending.Choices[0];
        if (r.Stage == MobileShiChenZhiStage.ChooseTarget)
            return pending.Choices.OrderBy(c => global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[c.Targets.Single()])).First();
        return pending.Choices.OrderByDescending(c => c.Cards.Count == 1 ? global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards[0]), _coreMatchParticipantsPlayersAccess._players[r.TargetSeat]) : 4).First();
    }

    internal IReadOnlyList<ProgramPublicRuleStateSnapshot> MobileShiChenZhiPublicStates(CharacterState owner, string skillId)
    {
        if (skillId is not (ShiCzQuanchong or ShiCzRenxing))
            return[];
        var binding = skillId == ShiCzQuanchong ? "quanchong-ending" : "renxing-first-discard";
        var limit = skillId == ShiCzQuanchong ? 1 : 2;
        var states = _coreNationalSkillsOperations.GetSkillBindingShard(owner).ProgramInstances.Where(i => i.SkillId == skillId).Select(i => new ProgramPublicRuleStateSnapshot(i.SkillInstanceId, binding, ProgramPublicRuleStateKind.ActivationLimit, limit, _skillRuntimeStateOperations._skillRuntimeState.GetUsage(owner.Seat, skillId, $"{(binding)}@{(i.SkillInstanceId)}", SkillUsageScope.Round), SkillUsageScope.Round)).ToList();
        if (skillId == ShiCzRenxing)
            foreach (var i in _coreNationalSkillsOperations.GetSkillBindingShard(owner).ProgramInstances.Where(i => i.SkillId == skillId))
                states.Add(new(i.SkillInstanceId, "turn-first-discard", ProgramPublicRuleStateKind.ActivationLimit, 1, _coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiChenZhiFirstDiscardEvent>().Any(e => e.ActualTurn == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber) ? 1 : 0, SkillUsageScope.Turn));
        return Array.AsReadOnly(states.ToArray());
    }
}
