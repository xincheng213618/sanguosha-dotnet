#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuYongAnniversaryRuntime
{
    internal int AnniversaryLiuYongLimit(int owner) => _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryLiuYongLimitExpandedEvent>().Any(e => e.OwnerSeat == owner && e.Turn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && e.PhaseInstance == _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId) ? 2 : 1;
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 43400, SkillProgramEffectOp.ResolveAnniversaryLiuYongZhuning)]
    internal bool CanActivateAnniversaryLiuYong(CharacterState owner) => owner.IsAlive && _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryLiuYongStartedEvent>().Count(e => e.Source.OwnerSeat == owner.Seat && e.Mode == "gift" && e.Turn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && e.PhaseInstance == _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId) < AnniversaryLiuYongLimit(owner.Seat);
    SkillProgramStepOutcome global::CardGame.Core.IAnniversaryLiuYongHost.Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginAnniversaryLiuYong(f, op);
    internal SkillProgramStepOutcome BeginAnniversaryLiuYong(ProgramSkillFrame supplied, SkillProgramEffectOp operation)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.AnniversaryLiuYong is not null || f.InstructionIndex != 1)
            throw new InvalidOperationException("Liu Yong requires his original unpaid instruction.");
        var gift = operation == SkillProgramEffectOp.ResolveAnniversaryLiuYongZhuning;
        if (gift && (f.SkillId != AnniversaryLiuYongComposition.Zhuning || f.ActivationId != "gift-and-use" || f.SelectedTargetSeats is not [var target] || target == f.OwnerSeat || !CanActivateAnniversaryLiuYong(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])))
            throw new InvalidOperationException("Zhuning lost its actual Play-phase quota or other receiver.");
        var mode = gift ? "gift" : f.WindowContext?.Window == SkillProgramTriggerWindow.CardsMoved ? "transition" : "damage";
        var leader = mode == "damage" ? AnniversaryLiuYongLeader(f.OwnerSeat) : null;
        var r = new AnniversaryLiuYongReceipt(AnniversaryLiuYongSource(f), f.GameplayHash, f.WindowContext?.ParentFrameId ?? 0, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, mode, gift ? AnniversaryLiuYongStage.Gift : mode == "damage" && leader is not null ? AnniversaryLiuYongStage.HealthChildren : AnniversaryLiuYongStage.Drawing)
        {
            Recipient = gift ? f.SelectedTargetSeats.Single() : leader ?? f.OwnerSeat,
            Leader = leader
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryLiuYong = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryLiuYongStartedEvent(f.Id, r.Source, r.Hash, r.ParentId, r.Turn, r.PhaseInstance, mode));
        if (gift)
            PublishAnniversaryLiuYong(f);
        else if (r.Stage == AnniversaryLiuYongStage.HealthChildren)
        {
            _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(f.Id, f.OwnerSeat, r.Recipient, 1, null, null);
            ContinueAnniversaryLiuYong(f.Id);
        }
        else
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, 1, AnniversaryLiuYongDrawReason);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal static CardMoveReason AnniversaryLiuYongGiftReason => new("skill.anniversary.zhuning.gift");
    internal static CardMoveReason AnniversaryLiuYongDrawReason => new("skill.anniversary.fengxiang.draw");

    internal IReadOnlyList<PromptChoice> AnniversaryLiuYongChoices(ProgramSkillFrame f)
    {
        var r = f.AnniversaryLiuYong!;
        PromptChoice Choice(string id, string label, IReadOnlyList<int> cards, IReadOnlyList<int> targets) => new(new($"anniversary-liuyong.{(f.Id)}.{(id)}"), label, cards, targets, new Dictionary<string, string> { ["program-action"] = "anniversary-liuyong", ["option"] = id });
        if (r.Stage == AnniversaryLiuYongStage.Gift)
        {
            var cards = _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, f.OwnerSeat, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Transfer).Where(c => c.Cards.Count == 1 && !r.Cards.Contains(c.Cards[0]) && _coreLianZhuCountsOperations.CanDeliverLianZhuGift(f.OwnerSeat, r.Recipient, c.Cards[0]));
            return cards.Select(c => Choice("card-" + c.Cards[0], c.Label, c.Cards, [])).Append(Choice("give", $"交给{(_coreMatchParticipantsPlayersAccess._players[r.Recipient].Name)}{(r.Cards.Count)}张牌并标记隙", [], [r.Recipient])).ToArray();
        }

        if (r.Stage != AnniversaryLiuYongStage.UseChoice)
            return[];
        return AnniversaryLiuYongPlans(f.OwnerSeat).Select((p, index) => Choice("use-" + index, $"视为使用【{(CardCatalog.Get(p.Kind).DisplayName)}】" + (p.Targets.Count == 0 ? "" : "：" + string.Join("、", p.Targets.Select(t => _coreMatchParticipantsPlayersAccess._players[t].Name))), [], p.Targets)).Append(Choice("finish", "不使用牌", [], [])).ToArray();
    }

    internal void PublishAnniversaryLiuYong(ProgramSkillFrame f)
    {
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            _coreSkillProgramsOperations.FinishProgramSkill(f, true);
            return;
        }

        var choices = AnniversaryLiuYongChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, f.AnniversaryLiuYong!.Stage == AnniversaryLiuYongStage.Gift ? "诛佞：选择交给目标的牌，完成时一次交付。" : "诛佞：选择原生虚拟牌用法，或结束。", choices.SelectMany(c => c.Cards).ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = f.AnniversaryLiuYong.Stage == AnniversaryLiuYongStage.Gift,
            TargetSeat = f.AnniversaryLiuYong.Recipient,
            Choices = Array.AsReadOnly(choices.ToArray()),
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("anniversary-liuyong")]
    internal void AnswerAnniversaryLiuYong(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryLiuYong: { } r } f || !AnniversaryLiuYongChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Liu Yong rejects a stale private material or native target plan.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var option = choice.Parameters["option"];
        if (r.Stage == AnniversaryLiuYongStage.Gift)
        {
            if (option != "give")
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryLiuYong = r with { Cards = r.Cards.Append(choice.Cards.Single()).ToArray() } });
                PublishAnniversaryLiuYong(f);
                return;
            }

            if (!_coreMatchParticipantsPlayersAccess._players[r.Recipient].IsAlive || r.Cards.Any(id => !_coreLianZhuCountsOperations.CanDeliverLianZhuGift(f.OwnerSeat, r.Recipient, id)))
                throw new InvalidOperationException("Zhuning requires all original HE entities still transferable to its original receiver.");
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            r = r with
            {
                GiftPaid = true,
                Stage = AnniversaryLiuYongStage.GiftChildren,
                From = r.Cards.Select(_cardZonesOperations._cardZones.GetLocation).ToArray(),
                Before = before,
                After = before
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryLiuYong = r });
            _coreMatchParticipantsPlayersAccess._players[r.Recipient].AnniversaryLiuYongXiCards = _coreMatchParticipantsPlayersAccess._players[r.Recipient].AnniversaryLiuYongXiCards.Concat(r.Cards.Select(id => new AnniversaryLiuYongXi(r.Source, f.Id, id, r.Recipient))).ToArray();
            if (r.Cards.Count > 0)
                _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(r.Cards, CardLocation.Hand(r.Recipient), AnniversaryLiuYongGiftReason, (_, _) =>
                {
                    var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                    _coreRuntimeOperations.ReplaceRuntimeTop(current with { AnniversaryLiuYong = current.AnniversaryLiuYong!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
                });
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            r = f.AnniversaryLiuYong!;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryLiuYongGiftPaidEvent(f.Id, r.Source, r.Recipient, r.Cards.Count, before, r.After));
            _ = _coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(f.Id, f.OwnerSeat);
            ContinueAnniversaryLiuYong(f.Id);
            return;
        }

        if (option == "finish")
        {
            _coreSkillProgramsOperations.FinishProgramSkill(f, true);
            return;
        }

        var plan = AnniversaryLiuYongPlans(f.OwnerSeat)[int.Parse(option[4..])];
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryLiuYong = r with { Plan = plan, Stage = AnniversaryLiuYongStage.IssuingUse } });
        IssueAnniversaryLiuYongUse(f);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 43400)]
    internal bool ContinueAnniversaryLiuYong(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryLiuYong: { } r } f || f.Id != id || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            return false;
        if (r.Stage is AnniversaryLiuYongStage.Gift or AnniversaryLiuYongStage.UseChoice)
        {
            PublishAnniversaryLiuYong(f);
            return true;
        }

        if (r.Stage is AnniversaryLiuYongStage.Drawing or AnniversaryLiuYongStage.NativeUse or AnniversaryLiuYongStage.IssuingUse)
            return true;
        if (DrainAnniversaryLiuYong(f))
            return true;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.AnniversaryLiuYong!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        if (r.Stage == AnniversaryLiuYongStage.GiftChildren)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryLiuYong = r with { Stage = AnniversaryLiuYongStage.UseChoice } });
            PublishAnniversaryLiuYong(f);
            return true;
        }

        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
        return true;
    }

    internal bool DrainAnniversaryLiuYong(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, f.AnniversaryLiuYong!.Stage == AnniversaryLiuYongStage.GiftChildren ? PostEventContinuation.AwaitedProgramMovement : PostEventContinuation.Program) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, f.AnniversaryLiuYong!.Stage == AnniversaryLiuYongStage.GiftChildren ? PostEventContinuation.AwaitedProgramMovement : PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id);
    [ProgramContinuation(ProgramContinuationKind.Movement, 43400)]
    internal bool ReturnAnniversaryLiuYongMovement(ProgramSkillFrame f) => f.AnniversaryLiuYong is not null && ContinueAnniversaryLiuYong(f.Id);
    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveAnniversaryLiuYongFengxiang)]
    internal void ReturnAnniversaryLiuYongDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        if (f.AnniversaryLiuYong is not { Stage: AnniversaryLiuYongStage.Drawing, Draw: null } r || outcome.RecipientSeat != f.OwnerSeat || outcome.RequestedCount != 1 || outcome.SequenceAfter != _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
            throw new InvalidOperationException("Fengxiang lost its exact one-card native draw return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { AnniversaryLiuYong = r with { Stage = AnniversaryLiuYongStage.DrawChildren, Draw = outcome, Before = outcome.SequenceBefore, After = outcome.SequenceAfter }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryLiuYongDrawReturnedEvent(f.Id, outcome.AttemptFrameId, outcome.SequenceBefore, outcome.SequenceAfter, outcome.ActualDrawIds.Count));
        ContinueAnniversaryLiuYong(f.Id);
    }

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 43400)]
    internal void CaptureAnniversaryLiuYongDamage(DamageAppliedEvent fact)
    {
        if (fact.SourceCardUseResolutionId is not { } useId || fact.Amount <= 0)
            return;
        foreach (var f in _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Where(f => f.AnniversaryLiuYong is { UseId: > 0, CausedDamage: false } r && r.UseId == useId).ToArray())
            _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { AnniversaryLiuYong = f.AnniversaryLiuYong!with { CausedDamage = true } });
    }

    internal void CompleteAnniversaryLiuYongUse(ProgramSkillFrame f)
    {
        var r = f.AnniversaryLiuYong!;
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is not null || r.Stage != AnniversaryLiuYongStage.NativeReturned)
            throw new InvalidOperationException("Zhuning cannot resume before its whole native use returns.");
        if (!r.CausedDamage && !_coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryLiuYongLimitExpandedEvent>().Any(e => e.FrameId == f.Id))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryLiuYongLimitExpandedEvent(f.Id, f.OwnerSeat, r.Turn, r.PhaseInstance));
        ContinueAnniversaryLiuYong(f.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveAnniversaryLiuYongZhuning)]
    internal PromptChoice ChooseAnniversaryLiuYong(PendingDecision p, ProgramSkillFrame f) => p.Choices.First(c => c.Parameters["option"] == (f.AnniversaryLiuYong!.Stage == AnniversaryLiuYongStage.Gift ? "give" : "finish"));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 43400)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 43400)]
    internal bool RetainAnniversaryLiuYong(ProgramSkillFrame f) => f.AnniversaryLiuYong is not null;
}
