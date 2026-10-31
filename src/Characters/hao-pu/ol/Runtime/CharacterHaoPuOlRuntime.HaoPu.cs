#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHaoPuOlRuntime
{
    internal const string ZhenyingDiscardReason = RuntimeSkills.RuntimeRules_Core_ProgramDiscardTopPlacement.ZhenyingDiscardReason, ZhenyingDrawReason = "skill-program.hao-pu.zhenying.draw";
    internal static CardConversionSource ZhenyingSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_hao_pu_ol_HaoPu.ZhenyingSource(f);
    internal ZhenyingHandPlan FreezeZhenyingHand(int seat)
    {
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]);
        return new(seat, Array.AsReadOnly(hand.Select(c => c.Id).ToArray()), ProtectedIds: Array.AsReadOnly(hand.Where(c => _coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(seat, c, CardLocation.Hand(seat), OwnedCardMoveIntent.Discard)).Select(c => c.Id).ToArray()));
    }

    internal SkillProgramStepOutcome BeginZhenying(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var activation = ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).Activation;
        if (activation is null || f.Zhenying is not null || f.TriggerId is not null || f.WindowContext is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats is not [var other] || other == f.OwnerSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(other) || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[other].IsAlive || _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[other]).Count > _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count || f.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || _coreGameEngineSkillProgramsProgramPhaseUsesAccess._programPhaseUses.GetValueOrDefault((f.OwnerSeat, f.SkillId, activation.UsageGroup))is < 1 or > 2)
            throw new InvalidOperationException("Zhenying requires its exact zero-card twice-per-Play activation and actual hand-count target.");
        HaoPuContract.Activation(f.SkillId, activation);
        var r = new ZhenyingReceipt(ZhenyingSource(f), f.GameplayHash, f.InstructionIndex, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, other, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, FreezeZhenyingHand(f.OwnerSeat), FreezeZhenyingHand(other));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Zhenying = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhenyingStartedEvent(f.Id, r.Source, r.GameplayHash, r.ActualTurn, r.PhaseInstanceId, other, r.StartMovementSequence, r.OwnerPlan.OriginalIds.Count, r.OtherPlan.OriginalIds.Count));
        PublishZhenyingChoice(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal static ZhenyingHandPlan ZhenyingPlan(ZhenyingReceipt r) => RuntimeSkills.RuntimeRules_hao_pu_ol_HaoPu.ZhenyingPlan(r);
    internal static ZhenyingReceipt WithZhenyingPlan(ZhenyingReceipt r, ZhenyingHandPlan plan) => RuntimeSkills.RuntimeRules_hao_pu_ol_HaoPu.WithZhenyingPlan(r, plan);
    internal static int ZhenyingKeptCount(ZhenyingHandPlan p) => RuntimeSkills.RuntimeRules_hao_pu_ol_HaoPu.ZhenyingKeptCount(p);
    internal IReadOnlyList<PromptChoice> ZhenyingChoices(ProgramSkillFrame f)
    {
        var r = f.Zhenying!;
        var p = ZhenyingPlan(r);
        Dictionary<string, string> Params(string branch) => new()
        {
            ["program-action"] = "zhenying",
            ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture),
            ["chooser-seat"] = p.Seat.ToString(CultureInfo.InvariantCulture),
            ["slot"] = r.SlotIndex.ToString(CultureInfo.InvariantCulture),
            ["branch"] = branch
        };
        if (r.Stage == ZhenyingStage.ChoosingCount)
            return Array.AsReadOnly(Enumerable.Range(0, 3).Select(count =>
            {
                var parameters = Params("count");
                parameters["count"] = count.ToString(CultureInfo.InvariantCulture);
                return new PromptChoice(new($"zhenying.{(f.Id)}.{(p.Seat)}.count.{(count)}"), $"将手牌调整至{(count)}张", [], [], parameters);
            }).ToArray());
        if (r.Stage != ZhenyingStage.ChoosingKeptCards || p.DesiredCount is null || p.KeptIds is null)
            throw new InvalidOperationException("Zhenying has no uncommitted private hand choice.");
        if (p.KeptIds.Count == ZhenyingKeptCount(p))
            return Array.AsReadOnly(new[] { new PromptChoice(new($"zhenying.{(f.Id)}.{(p.Seat)}.empty.{(r.SlotIndex)}"), "确认此保留位留空", [], [], Params("empty")) });
        return Array.AsReadOnly(p.OriginalIds.Except(p.KeptIds).Order().Select(id => new PromptChoice(new($"zhenying.{(f.Id)}.{(p.Seat)}.keep.{(r.SlotIndex)}.{(id)}"), $"保留【{(_coreCardAttackStateOperations.GetAttackCard(id).DisplayName)}】", [id], [], Params("keep"))).ToArray());
    }

    internal void PublishZhenyingChoice(ProgramSkillFrame f)
    {
        var p = ZhenyingPlan(f.Zhenying!);
        var choices = ZhenyingChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        if (choices.Count == 0)
            throw new InvalidOperationException("A sealed Zhenying choice has no legal continuation.");
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, p.Seat, f.Zhenying!.Stage == ZhenyingStage.ChoosingCount ? "私下选择手牌调整至0、1或2张；双方选择完成后同时执行。" : "私下选择要保留的原手牌；双方选定前不移动或公开选择。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = p.Seat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[p.Seat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("zhenying")]
    internal void AnswerZhenying(PromptChoice choice)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Zhenying lost its choice owner.");
        var r = f.Zhenying!;
        var p = ZhenyingPlan(r);
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { IsPrivate: true } prompt || prompt.PlayerSeat != p.Seat || !ZhenyingChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Zhenying requires the exact published chooser-owned private commitment.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (r.Stage == ZhenyingStage.ChoosingCount)
        {
            var count = int.Parse(choice.Parameters["count"], CultureInfo.InvariantCulture);
            p = p with
            {
                DesiredCount = count,
                KeptIds = p.OriginalIds.Count <= count ? p.OriginalIds : p.ProtectedIds ?? []
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Zhenying = WithZhenyingPlan(r, p)with { Stage = ZhenyingStage.ChoosingKeptCards, SlotIndex = 0 } });
            PublishZhenyingChoice(f);
            return;
        }
        else
            p = p with
            {
                KeptIds = Array.AsReadOnly(p.KeptIds!.Concat(choice.Cards).ToArray())
            };
        if (r.SlotIndex == 0)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Zhenying = WithZhenyingPlan(r, p)with { SlotIndex = 1 } });
            PublishZhenyingChoice(f);
            return;
        }

        if (p.KeptIds!.Count != ZhenyingKeptCount(p))
            throw new InvalidOperationException("Two sealed Zhenying slots must finish the selected retained set.");
        p = p with
        {
            Committed = true
        };
        r = WithZhenyingPlan(r, p);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Zhenying = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhenyingChoiceCommittedEvent(f.Id, p.Seat));
        if (r.SelectionIndex == 0)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Zhenying = r with { SelectionIndex = 1, SlotIndex = 0, Stage = ZhenyingStage.ChoosingCount } });
            PublishZhenyingChoice(f);
            return;
        }

        PayZhenyingAdjustment(f);
    }

    internal void PayZhenyingAdjustment(ProgramSkillFrame f)
    {
        var r = f.Zhenying!;
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        if (!r.OwnerPlan.Committed || !r.OtherPlan.Committed)
            throw new InvalidOperationException("Both Zhenying hands must be sealed before either moves.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Zhenying = r with { Stage = ZhenyingStage.AdjustmentChildren, PaymentBefore = before, PaymentAfter = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        foreach (var p in new[]
        {
            r.OwnerPlan,
            r.OtherPlan
        }

        )
        {
            var discarded = p.OriginalIds.Except(p.KeptIds!).Select(_coreCardAttackStateOperations.GetAttackCard).ToArray();
            if (discarded.Length > 0)
                _coreNativeLogicalDiscardsOperations.MoveLogicalDiscardCards(f.OwnerSeat, p.Seat, discarded, CardLocation.Hand(p.Seat), new(ZhenyingDiscardReason));
        }

        var ownerDraw = Math.Max(0, r.OwnerPlan.DesiredCount!.Value - r.OwnerPlan.OriginalIds.Count);
        var otherDraw = Math.Max(0, r.OtherPlan.DesiredCount!.Value - r.OtherPlan.OriginalIds.Count);
        var drawnOwner = ownerDraw == 0 ? 0 : _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], ownerDraw, true, new(ZhenyingDrawReason)).Count;
        var drawnOther = otherDraw == 0 ? 0 : _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[r.OtherSeat], otherDraw, true, new(ZhenyingDrawReason)).Count;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Zhenying = f.Zhenying!with { PaymentAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, OwnerDrawCount = drawnOwner, OtherDrawCount = drawnOther } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhenyingAdjustmentPaidEvent(f.Id, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, r.OwnerPlan.DesiredCount.Value, r.OtherPlan.DesiredCount.Value, drawnOwner, drawnOther, _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count, _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.OtherSeat]).Count));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal bool DrainZhenyingChildren(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.AwaitedProgramMovement);
    [ProgramContinuation(ProgramContinuationKind.Resume, 26500)]
    internal bool ResumeZhenying(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Zhenying: { } r } f || f.Id != id)
            return false;
        if (r.Stage is ZhenyingStage.ChoosingCount or ZhenyingStage.ChoosingKeptCards)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishZhenyingChoice(f);
            return true;
        }

        if (r.Stage == ZhenyingStage.DuelIssued)
            throw new InvalidOperationException("An issued Zhenying Duel needs its exact native return.");
        if (DrainZhenyingChildren(id))
            return true;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.Zhenying!;
        var ownerCount = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count;
        var otherCount = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.OtherSeat]).Count;
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[r.OtherSeat].IsAlive || ownerCount == otherCount)
        {
            FinishZhenying(f, false);
            return true;
        }

        var actor = ownerCount < otherCount ? f.OwnerSeat : r.OtherSeat;
        var target = actor == f.OwnerSeat ? r.OtherSeat : f.OwnerSeat;
        if (!_coreDamageColorDuelProgramsOperations.CanIssueSelectedActorDuel(target, actor))
        {
            FinishZhenying(f, false);
            return true;
        }

        var useId = _coreResolutionIdentityBoundaryOperations._resolutionSequence + 1;
        var origin = new ZhenyingDuelOrigin(f.Id, r.InstructionIndex, useId, r.Source, r.GameplayHash, r.ActualTurn, r.PhaseInstanceId, actor, target, ownerCount, otherCount);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null, Zhenying = r with { Stage = ZhenyingStage.DuelIssued, Duel = origin } });
        var card = new Card(0, CardKind.Duel, Suit.None, 0);
        if (_coreNativeCardUseOperations.BeginCardUse(card, actor, [target], CardKind.Duel, physicalCardIds: [], zhenyingDuel: origin) != useId)
            throw new InvalidOperationException("Zhenying lost its reserved exact native Duel use.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhenyingDuelIssuedEvent(origin));
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(useId, card, actor, [target], LegalActionKind.Duel, playedCardKind: CardKind.Duel);
        return true;
    }

    internal void FinishZhenying(ProgramSkillFrame f, bool duel)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhenyingCompletedEvent(f.Id, duel));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Zhenying = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 26500)]
    internal bool ReturnZhenyingMovement(ProgramSkillFrame f) => f.Zhenying is not null && ResumeZhenying(f.Id);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveZhenyingSimultaneousHands)]
    internal PromptChoice ChooseZhenyingAi(PendingDecision prompt, ProgramSkillFrame f)
    {
        if (f.Zhenying!.Stage == ZhenyingStage.ChoosingCount)
            return prompt.Choices.Single(c => c.Parameters.GetValueOrDefault("count") == "2");
        // Only this chooser's original cards are examined; the other commitment
        // and private hand never affect the AI's sealed choice.
        return prompt.Choices.OrderByDescending(c => c.Cards is [var id] && CoreNativeCardClassificationsRuntime.IsSlashCard(_coreCardAttackStateOperations.GetAttackCard(id).Kind)).ThenBy(c => c.Id.Value, StringComparer.Ordinal).First();
    }

    SkillProgramStepOutcome IHaoPuProgramHost.ResolveZhenying(ProgramSkillFrame f) => BeginZhenying(f);
}
