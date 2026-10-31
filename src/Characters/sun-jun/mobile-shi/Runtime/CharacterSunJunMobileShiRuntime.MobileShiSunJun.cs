#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunJunMobileShiRuntime
{
    internal const string ShiSjXiongtu = "mobile:shi-xiongtu", ShiSjXianshuai = "mobile:shi-xianshuai";
    internal const string ShiSjDiscard = "skill.mobile-shi-sun-jun.discard", ShiSjDraw = "skill-program.mobile-shi-sun-jun.draw";
    internal static CardConversionSource ShiSjSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    SkillProgramStepOutcome IShiSunJunHost.Resolve(ProgramSkillFrame f, bool benefit) => BeginShiSj(f, benefit);
    internal int ShiSjPhaseLimit(int owner) => _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } phase && phase.ActorSeat == owner && _coreProgramEventHistoryOperations.ProgramEventHistory<ShiSunJunBenefitIssuedEvent>().Any(e => e.Owner == owner && e.Turn == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber && e.Phase == phase) ? 2 : 1;
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCondition, 49200, SkillProgramEffectOp.ResolveMobileShiXiongtu)]
    internal bool AllowsShiSjActivation(ProgramActivationLegalityContext gate) => _coreGameEngineSkillProgramsProgramPhaseUsesAccess._programPhaseUses.GetValueOrDefault((gate.OwnerSeat, gate.SkillId, gate.Activation.UsageGroup)) < ShiSjPhaseLimit(gate.OwnerSeat);
    internal bool ShiSjDamageBenefitEligible(int owner, string instance, ProgramSkillWindowContext c) => c is { Window: SkillProgramTriggerWindow.AfterDamageApplied, SourceSeat: { } source, DamageFrameId: { } damageId, Amount: > 0 } && source == owner && _coreProgramPersistentStateOperations.GetProgramBooleanState2(owner, ShiSjXiongtu, instance, "armed") && _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == damageId)is DamageFrame damage && !(_resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == damage.ParentFrameId)is ProgramSkillFrame { SkillId: ShiSjXiongtu, ShiSunJun: { DamageBranch: true } }) && !_coreProgramEventHistoryOperations.ProgramEventHistory<ShiSunJunBenefitIssuedEvent>().Any(e => e.Owner == owner && e.Turn == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber && e.DamageId == damageId);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 49200, SkillProgramEffectOp.ResolveMobileShiXiongtuDamageBenefit)]
    internal bool AllowsShiSjDamageBenefit(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => ShiSjDamageBenefitEligible(c.OwnerSeat, c.SkillInstanceId, context);
    internal SkillProgramStepOutcome BeginShiSj(ProgramSkillFrame input, bool benefit)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.ShiSunJun is not null || f.InstructionIndex != 1 || f.SkillId != ShiSjXiongtu)
            throw new InvalidOperationException("Shi Sun Jun requires the original unpaid Xiongtu instruction.");
        var phase = _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase();
        if (benefit)
        {
            if (f.TriggerId != "other-damage" || f.WindowContext is not { } context || !ShiSjDamageBenefitEligible(f.OwnerSeat, f.SkillInstanceId, context))
                throw new InvalidOperationException("Xiongtu benefit requires its exact different-source real damage occurrence.");
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiSunJun = new(ShiSjSource(f), f.GameplayHash, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, phase, f.OwnerSeat, ShiSunJunStage.Drawing) { DamageId = context.DamageFrameId!.Value, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiSunJunStartedEvent(f.Id, ShiSjSource(f), f.GameplayHash, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, f.OwnerSeat));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiSunJunBenefitIssuedEvent(f.Id, f.OwnerSeat, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, context.DamageFrameId.Value, phase));
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, 1, new(ShiSjDraw));
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (f.TriggerId is not null || f.ActivationId != "show-hand" || f.SelectedTargetSeats is not [var target] || target == f.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Count == 0 || phase is not { Kind: ActualDiscardRecoveryPhaseKind.Play } || phase.ActorSeat != f.OwnerSeat)
            throw new InvalidOperationException("Xiongtu requires one other living actual Hand target in its own actual Play phase.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiSunJun = new(ShiSjSource(f), f.GameplayHash, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, phase, target, ShiSunJunStage.Hand) { Hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Select(c => c.Id).ToArray() } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiSunJunStartedEvent(f.Id, ShiSjSource(f), f.GameplayHash, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, target));
        PublishShiSj(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> ShiSjOwnDiscardChoices(ProgramSkillFrame f) => _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, f.OwnerSeat, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Discard, canSelect: (zone, card) => !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(f.OwnerSeat, card, new(zone, f.OwnerSeat), OwnedCardMoveIntent.Discard));
    internal bool ShiSjCanDiscardRevealed(ProgramSkillFrame f) => f.ShiSunJun is { Revealed: > 0 } r && _cardZonesOperations._cardZones.GetLocation(r.Revealed) == CardLocation.Hand(r.Target) && _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, r.Target, [CardZoneKind.Hand], OwnedCardMoveIntent.Discard, canSelect: (_, card) => card.Id == r.Revealed).Count > 0;
    internal IReadOnlyList<PromptChoice> ShiSjChoices(ProgramSkillFrame f)
    {
        var r = f.ShiSunJun!;
        PromptChoice C(string branch, string token, string label, IReadOnlyList<int>? cards = null) => new(new($"shi-sun-jun.{(f.Id)}.{(r.Stage)}.{(branch)}.{(token)}"), label, cards ?? [], [], new Dictionary<string, string> { ["program-action"] = "mobile-shi-sun-jun", ["branch"] = branch, ["token"] = token, ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture) });
        if (r.Stage == ShiSunJunStage.Hand)
            return r.Hand.Select((id, slot) => C("hand", slot.ToString(CultureInfo.InvariantCulture), $"展示 {(_coreMatchParticipantsPlayersAccess._players[r.Target].Name)} 的第 {(slot + 1)} 张手牌")).ToArray();
        if (r.Stage == ShiSunJunStage.Branch)
        {
            var choices = new List<PromptChoice>();
            if (ShiSjCanDiscardRevealed(f))
                choices.Add(C("discard", "discard", "弃置已展示的这张手牌"));
            if (ShiSjOwnDiscardChoices(f).Count >= r.Required)
                choices.Add(C("damage", "damage", $"弃置 {(r.Required)} 张牌，对其造成1点伤害"));
            return choices.ToArray();
        }

        if (r.Stage != ShiSunJunStage.Payment)
            return[];
        return ShiSjOwnDiscardChoices(f).Where(c => c.Cards is [var id] && !r.Payment.Contains(id)).Select(c => C("cost", c.Cards.Single().ToString(CultureInfo.InvariantCulture), c.Description, c.Cards)).ToArray();
    }

    internal void PublishShiSj(ProgramSkillFrame f)
    {
        var choices = ShiSjChoices(f);
        if (choices.Count == 0)
        {
            FinishShiSj(f);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, f.ShiSunJun!.Stage == ShiSunJunStage.Hand ? "凶图：选择一个不公开身份的手牌位置，随后公开展示该实体。" : f.ShiSunJun.Stage == ShiSunJunStage.Payment ? $"凶图：选择弃置 {(f.ShiSunJun.Required)} 张牌（已选 {(f.ShiSunJun.Payment.Count)} 张）。" : "凶图：选择弃置展示牌或支付冻结费用造成伤害。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("mobile-shi-sun-jun")]
    internal void AnswerShiSj(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShiSunJun: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !ShiSjChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Xiongtu lost its exact private slot, frozen branch or actual cost choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var branch = choice.Parameters["branch"];
        if (r.Stage == ShiSunJunStage.Hand)
        {
            if (!_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.Target]).Select(c => c.Id).SequenceEqual(r.Hand))
                throw new InvalidOperationException("Xiongtu's opaque Hand slots changed before selection.");
            var card = r.Hand[int.Parse(choice.Parameters["token"], CultureInfo.InvariantCulture)];
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiSunJun = r with { Stage = ShiSunJunStage.RevealChildren, Revealed = card } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiSunJunRevealedEvent(f.Id, r.Target, card));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramCardsRevealedEvent(f.Id, f.SkillId, f.ActivationId, f.OwnerSeat, "xiongtu-hand", Array.AsReadOnly(new[] { _coreSnapshotValueProjectionOperations.ToSnapshot(_coreAdvancedLifecycleOperations.GetAdvancedCard(card)) })));
            if (!_characterXiaoQiaoMouOlOperations.TryBeginPublicHandRevealWindow2(f.Id, "mobile-shi-sun-jun-hand"))
                AfterShiSjPublicReveal(f);
            return;
        }

        if (r.Stage == ShiSunJunStage.Branch)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiSunJunBranchIssuedEvent(f.Id, r.Required, branch == "damage"));
            if (branch == "damage")
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiSunJun = r with { Stage = ShiSunJunStage.Payment, DamageBranch = true } });
                if (r.Required == 0)
                    PayShiSjCost(f);
                else
                    PublishShiSj(f);
                return;
            }

            _coreProgramSkillChangesOperations.SetProgramBooleanState(f, "armed", true);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiSunJun = r with { Stage = ShiSunJunStage.DiscardChildren, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, [r.Revealed], new(ShiSjDiscard), (batch, moves) =>
            {
                foreach (var m in moves)
                    _characterBianFuRenOlOperations.RecordBianFuRenForeignDiscardProducer(m, f.OwnerSeat, f.Id);
            });
            SealShiSj(f.Id);
            ContinueShiSj(f.Id);
            return;
        }

        if (r.Stage != ShiSunJunStage.Payment || branch != "cost" || choice.Cards is not [var paid])
            throw new InvalidOperationException("Xiongtu cannot pay outside its frozen cost selection.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiSunJun = r with { Payment = r.Payment.Append(paid).ToArray() } });
        if (f.ShiSunJun!.Payment.Count == r.Required)
            PayShiSjCost(f);
        else
            PublishShiSj(f);
    }

    [PublicHandRevealReturn("mobile-shi-sun-jun-hand")]
    internal void AfterShiSjPublicReveal(ProgramSkillFrame f)
    {
        if (f.ShiSunJun is not { Stage: ShiSunJunStage.RevealChildren } r)
            throw new InvalidOperationException("Xiongtu public reveal lost its original selected Hand entity.");
        // Freeze before the branch and all actual payment movements. The cost cannot reduce itself.
        if (!_coreTurnDiscardSuitHistoryOperations.TracksTurnDiscardSuits)
            throw new InvalidOperationException("Xiongtu requires the enabled real turn discard-suit history.");
        var x = 4 - System.Numerics.BitOperations.PopCount((uint)(_coreTurnDiscardSuitHistoryOperations.CurrentTurnDiscardSuitMask & 15));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiSunJun = r with { Stage = ShiSunJunStage.Branch, Required = x } });
        PublishShiSj(f);
    }

    internal void PayShiSjCost(ProgramSkillFrame f)
    {
        var r = f.ShiSunJun!;
        if (r.Stage != ShiSunJunStage.Payment || r.Payment.Count != r.Required || r.Payment.Distinct().Count() != r.Required || r.Payment.Any(id => !ShiSjOwnDiscardChoices(f).Any(c => c.Cards.SequenceEqual([id]))))
            throw new InvalidOperationException("Xiongtu must pay its complete originally frozen actual owned cost.");
        _coreProgramSkillChangesOperations.SetProgramBooleanState(f, "armed", true);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiSunJun = r with { Stage = ShiSunJunStage.CostChildren, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        if (r.Payment.Count > 0)
            _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, r.Payment, new(ShiSjDiscard));
        SealShiSj(f.Id);
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiSunJunCostPaidEvent(f.Id, r.Required, active.ShiSunJun!.Before, active.ShiSunJun.After));
        ContinueShiSj(f.Id);
    }

    internal void SealShiSj(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _coreRuntimeOperations.ReplaceRuntimeFrame(id, f with { ShiSunJun = f.ShiSunJun!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
    }

    internal bool DrainShiSj(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    internal void ContinueShiSj(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShiSunJun: { } r } f || f.Id != id)
            return;
        if (r.Stage is ShiSunJunStage.Hand or ShiSunJunStage.Branch or ShiSunJunStage.Payment)
        {
            PublishShiSj(f);
            return;
        }

        if (r.Stage == ShiSunJunStage.RevealChildren)
        {
            if (!_characterXiaoQiaoMouOlOperations.IsPublicHandRevealProducerSuspended(id, "mobile-shi-sun-jun-hand"))
                AfterShiSjPublicReveal(f);
            return;
        }

        if (r.Stage == ShiSunJunStage.Drawing)
            return;
        if (r.Stage == ShiSunJunStage.DamageIssued && f.AttackAttempt is not null)
        {
            _coreNativeDamageCompletionOperations.CompleteDamageAttack(_kernelCoreGameEngineProgramAttackAttemptOperations.CreateProgramAttackHandle(id));
            return;
        }

        if (DrainShiSj(f))
            return;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { PendingMovementContinuation = null });
        r = f.ShiSunJun!;
        if (r.Stage == ShiSunJunStage.CostChildren && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[r.Target].IsAlive)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiSunJun = r with { Stage = ShiSunJunStage.DamageIssued } });
            _coreSkillProgramsOperations.BeginProgramSkillDamage(f, r.Target, 1);
            return;
        }

        FinishShiSj(f);
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveMobileShiXiongtuDamageBenefit)]
    internal void ReturnShiSjDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        if (f.ShiSunJun is not { Stage: ShiSunJunStage.Drawing } r || outcome.RecipientSeat != f.OwnerSeat || outcome.RequestedCount != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<ShiSunJunDrawReturnedEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("Xiongtu reward lost its once-issued exact native draw return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiSunJun = r with { Stage = ShiSunJunStage.DrawChildren, Before = outcome.SequenceBefore, After = outcome.SequenceAfter }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiSunJunDrawReturnedEvent(f.Id, r.DamageId, outcome.ActualDrawIds.Count));
        ContinueShiSj(f.Id);
    }

    internal void FinishShiSj(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { ShiSunJun = f.ShiSunJun!with { Stage = ShiSunJunStage.Finished }, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 49200)]
    internal bool ResumeShiSj(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShiSunJun: not null } f || f.Id != id)
            return false;
        ContinueShiSj(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 49200)]
    internal bool ReturnShiSjMovement(ProgramSkillFrame f)
    {
        if (f.ShiSunJun is null)
            return false;
        ContinueShiSj(f.Id);
        return true;
    }

    [ProgramViewContributor(ProgramViewContribution.ProgramPublicCards, 49200)]
    internal IEnumerable<CardSnapshot> ShiSjPublicHandCard() => _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Where(f => f.ShiSunJun is { Revealed: > 0 }).Select(f => f.ShiSunJun!).Where(r => _cardZonesOperations._cardZones.GetLocation(r.Revealed) == CardLocation.Hand(r.Target)).Select(r => _coreSnapshotValueProjectionOperations.ToSnapshot(_coreAdvancedLifecycleOperations.GetAdvancedCard(r.Revealed)));
    [ProgramAiChoice(SkillProgramEffectOp.ResolveMobileShiXiongtu)]
    internal PromptChoice ChooseShiSj(PendingDecision p) => p.Choices[0];
    internal PendingDecision? ProjectShiSjCardSelection(PendingDecision decision, ProgramSkillFrame f) => f.ShiSunJun is { Stage: ShiSunJunStage.Payment } r && decision.PlayerSeat == f.OwnerSeat ? _corePlayerViewNativeCardSelectionsOperations.NativeOwnedSelection(decision, f, "shi-sun-jun:cost", "branch", ["cost"], [], r.Payment, r.Required - r.Payment.Count, r.Required - r.Payment.Count) : null;
    internal IReadOnlyList<ProgramPublicRuleStateSnapshot> MobileShiSunJunPublicStates(CharacterState owner, string id)
    {
        if (id == ShiSjXianshuai)
            return _coreNationalSkillsOperations.EnabledRuntimeSkillGrants(owner).Where(g => g.SkillId == id).DistinctBy(g => g.SkillInstanceId).SelectMany(g => new[] { new ProgramPublicRuleStateSnapshot(g.SkillInstanceId, "xianshuai-own-turn", ProgramPublicRuleStateKind.GameActivationProgress, ShiSjOwnTurn(owner.Seat) ? 1 : 0) }.Concat(new[] { Suit.Spade, Suit.Heart, Suit.Club, Suit.Diamond }.Select(suit => new ProgramPublicRuleStateSnapshot(g.SkillInstanceId, "xianshuai-suit-" + suit, ProgramPublicRuleStateKind.GameActivationProgress, ShiSjSuitSeen(owner.Seat, suit) ? 1 : 0)))).ToArray();
        if (id != ShiSjXiongtu)
            return[];
        return _coreNationalSkillsOperations.GetSkillBindingShard(owner).ProgramInstances.Where(i => i.SkillId == id).SelectMany(i => new[] { new ProgramPublicRuleStateSnapshot(i.SkillInstanceId, "show-hand", ProgramPublicRuleStateKind.ActivationLimit, ShiSjPhaseLimit(owner.Seat), _coreGameEngineSkillProgramsProgramPhaseUsesAccess._programPhaseUses.GetValueOrDefault((owner.Seat, id, i.Program.Activations.Single().UsageGroup)), SkillUsageScope.Phase), new ProgramPublicRuleStateSnapshot(i.SkillInstanceId, "xiongtu-cost", ProgramPublicRuleStateKind.GameActivationProgress, 4 - System.Numerics.BitOperations.PopCount((uint)(_coreTurnDiscardSuitHistoryOperations.CurrentTurnDiscardSuitMask & 15))), new ProgramPublicRuleStateSnapshot(i.SkillInstanceId, "xiongtu-armed", ProgramPublicRuleStateKind.GameActivationProgress, _coreProgramPersistentStateOperations.GetProgramBooleanState2(owner.Seat, id, i.SkillInstanceId, "armed") ? 1 : 0) }).ToArray();
    }
}
