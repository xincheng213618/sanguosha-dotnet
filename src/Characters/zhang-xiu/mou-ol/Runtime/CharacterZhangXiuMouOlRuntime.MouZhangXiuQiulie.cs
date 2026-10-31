#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangXiuMouOlRuntime
{
    internal const string MouZhangXiuOlCostReason = "skill-program.mou-ol:qiulie.cost", MouZhangXiuOlDefenseReason = "skill-program.mou-ol:qiulie.defense", MouZhangXiuOlDrawReason = "skill-program.mou-ol:zhuijiao.draw", MouZhangXiuOlPenaltyReason = "skill-program.mou-ol:zhuijiao.discard";
    internal static CardConversionSource MouZhangXiuOlSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal bool HasMouZhangXiuOl => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMouZhangXiuZhuijiao);

    internal MouZhangXiuOlHuntChosenEvent? MouZhangXiuOlHunt(int owner) => _coreProgramEventHistoryOperations.ProgramEventHistory<MouZhangXiuOlHuntChosenEvent>().SingleOrDefault(e => e.Source.OwnerSeat == owner && e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber);
    internal bool CanChooseMouZhangXiuOlHunt(int owner) => _coreMatchParticipantsPlayersAccess._players[owner].IsAlive && owner == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && !_coreProgramEventHistoryOperations.ProgramEventHistory<MouZhangXiuOlHuntChosenEvent>().Any(e => e.Source.OwnerSeat == owner) && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != owner);
    internal ActualDiscardRecoveryPhaseKey? MouZhangXiuOlCurrentPhase(int owner, SkillProgramTriggerWindow window) => _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { } p && p.ActorSeat == owner && p.ActualTurnOwnerSeat == owner && p.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && p.Kind == MouZhangXiuOlComposition.Phase(window) ? p : null;
    internal bool MouZhangXiuOlPhaseAlreadyOffered(int owner, ActualDiscardRecoveryPhaseKey p) => _coreProgramEventHistoryOperations.ProgramEventHistory<MouZhangXiuOlPhaseOfferedEvent>().Any(e => e.Source.OwnerSeat == owner && e.Phase.Token == p.Token);
    internal bool CanUseMouZhangXiuOlHunt(int owner, int target) => _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreTableSeatQueriesOperations.IsValidPlayerSeat(target) && owner != target && _coreMatchParticipantsPlayersAccess._players[owner].IsAlive && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && _coreNativeTargetLegalityOperations.CanUseVirtualSlashTarget(_coreMatchParticipantsPlayersAccess._players[owner], _coreMatchParticipantsPlayersAccess._players[target], CardKind.Slash, Suit.None, effectiveColor: null, effectiveRank: 0, physicalCardIds: []) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(owner, target, CardKind.Slash) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[target], CardKind.Slash, Suit.None, null);
    internal Card[] MouZhangXiuOlDiscardable(int owner, bool defense = false) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment2(owner)).Where(c => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(owner, c, _cardZonesOperations._cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Discard) && !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(owner, c, _cardZonesOperations._cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Discard) && (!defense || global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(_coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(_coreMatchParticipantsPlayersAccess._players[owner], c)) == SkillProgramCardCategory.Basic || _coreEquipmentContentOperations.EquipmentRules.IsEquipment(c.Kind) && _coreEquipmentContentOperations.EquipmentRules.Get(c.Kind).Slot == EquipmentSlot.Weapon)).ToArray();
    SkillProgramStepOutcome global::CardGame.Core.IMouZhangXiuOlHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => op switch
    {
        SkillProgramEffectOp.ResolveMouZhangXiuQiulie or SkillProgramEffectOp.UseMouZhangXiuQiulieSlash => BeginMouZhangXiuOlQiulie(f, op),
        SkillProgramEffectOp.DefendMouZhangXiuQiulieSlash => BeginMouZhangXiuOlDefense(f),
        _ => BeginMouZhangXiuOlZhuijiao(f, op)};
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 33000, SkillProgramEffectOp.ResolveMouZhangXiuQiulie, SkillProgramEffectOp.UseMouZhangXiuQiulieSlash)]
    internal bool QualifyMouZhangXiuOlPhase(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w)
    {
        if (MouZhangXiuOlCurrentPhase(c.OwnerSeat, w.Window)is not { } p || MouZhangXiuOlPhaseAlreadyOffered(c.OwnerSeat, p))
            return false;
        if (t.Effects[0].Op == SkillProgramEffectOp.ResolveMouZhangXiuQiulie)
            return CanChooseMouZhangXiuOlHunt(c.OwnerSeat);
        return MouZhangXiuOlHunt(c.OwnerSeat)is { } hunt && CanUseMouZhangXiuOlHunt(c.OwnerSeat, hunt.TargetSeat) && MouZhangXiuOlDiscardable(c.OwnerSeat).Length > 0;
    }

    internal SkillProgramStepOutcome BeginMouZhangXiuOlQiulie(ProgramSkillFrame supplied, SkillProgramEffectOp operation)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.MouZhangXiuOl is not null || f.InstructionIndex != 1 || f.ActivationId != f.TriggerId || f.WindowContext is not { } w || MouZhangXiuOlCurrentPhase(f.OwnerSeat, w.Window)is not { } phase || MouZhangXiuOlPhaseAlreadyOffered(f.OwnerSeat, phase))
            throw new InvalidOperationException("Qiulie requires its current actual phase and untouched opportunity.");
        var target = operation == SkillProgramEffectOp.ResolveMouZhangXiuQiulie ? -1 : MouZhangXiuOlHunt(f.OwnerSeat)?.TargetSeat ?? -1;
        if (operation == SkillProgramEffectOp.ResolveMouZhangXiuQiulie ? !CanChooseMouZhangXiuOlHunt(f.OwnerSeat) : !CanUseMouZhangXiuOlHunt(f.OwnerSeat, target))
            throw new InvalidOperationException("Qiulie cannot fabricate its limited target or a legal native Slash.");
        var r = new MouZhangXiuOlReceipt(MouZhangXiuOlSource(f), f.GameplayHash, operation, target < 0 ? MouZhangXiuOlStage.Target : MouZhangXiuOlStage.Cost)
        {
            Phase = phase,
            TargetSeat = target
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouZhangXiuOl = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZhangXiuOlStartedEvent(f.Id, r.Source, r.GameplayHash, operation, 0));
        if (target >= 0)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZhangXiuOlPhaseOfferedEvent(f.Id, r.Source, phase, target));
        PublishMouZhangXiuOl(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal int MouZhangXiuOlChooser(ProgramSkillFrame f) => f.MouZhangXiuOl!.Stage == MouZhangXiuOlStage.Defense ? f.MouZhangXiuOl.TargetSeat : f.OwnerSeat;
    internal IReadOnlyList<PromptChoice> MouZhangXiuOlChoices(ProgramSkillFrame f)
    {
        var r = f.MouZhangXiuOl!;
        var result = new List<PromptChoice>();
        PromptChoice Choice(string branch, string label, int? card = null, int? target = null) => new(new($"mou-zhang-xiu.{(f.Id)}.{(r.Stage)}.{(branch)}.{(card)}.{(target)}"), label, card is { } id ? [id] : [], target is { } seat ? [seat] : [], new Dictionary<string, string> { ["program-action"] = "mou-zhang-xiu-ol", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch });
        if (r.Stage == MouZhangXiuOlStage.Target)
        {
            result.AddRange(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat).Select(p => Choice("target", $"本回合追猎{(p.Name)}", target: p.Seat)));
            result.Add(Choice("decline", "不发动仇猎"));
        }
        else if (r.Stage is MouZhangXiuOlStage.Cost or MouZhangXiuOlStage.Defense or MouZhangXiuOlStage.Penalty)
        {
            var defense = r.Stage == MouZhangXiuOlStage.Defense;
            var payer = defense ? r.TargetSeat : f.OwnerSeat;
            if (_coreMatchParticipantsPlayersAccess._players[payer].IsAlive && (r.Stage != MouZhangXiuOlStage.Cost || CanUseMouZhangXiuOlHunt(f.OwnerSeat, r.TargetSeat)))
                result.AddRange(MouZhangXiuOlDiscardable(payer, defense).Select(c => Choice("card", $"弃置【{(c.DisplayName)}】" + (defense ? "，令此【杀】无效" : ""), c.Id)));
            if (r.Stage != MouZhangXiuOlStage.Penalty)
                result.Add(Choice("decline", defense ? "不弃牌，继续结算此【杀】" : "不弃牌使用【杀】"));
        }

        return Array.AsReadOnly(result.ToArray());
    }

    internal void PublishMouZhangXiuOl(ProgramSkillFrame f)
    {
        var r = f.MouZhangXiuOl!;
        var choices = MouZhangXiuOlChoices(f);
        if (choices.Count == 0)
        {
            FinishMouZhangXiuOl(f);
            return;
        }

        var chooser = MouZhangXiuOlChooser(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, r.Stage switch
        {
            MouZhangXiuOlStage.Target => "仇猎：选择本回合追猎目标。",
            MouZhangXiuOlStage.Defense => "可弃置一张基本牌或武器牌，令仇猎的【杀】无效。",
            MouZhangXiuOlStage.Penalty => "追剿：此【杀】未造成伤害，弃置一张牌。",
            _ => "仇猎：可弃置一张牌，视为对原目标使用【杀】。"
        }, choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = chooser,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("mou-zhang-xiu-ol")]
    internal void AnswerMouZhangXiuOl(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouZhangXiuOl: { } r } f || !ValidMouZhangXiuOl(f) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != MouZhangXiuOlChooser(f) || !MouZhangXiuOlChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Mou Zhang Xiu accepts only the original current chooser and live published cost.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var branch = choice.Parameters["branch"];
        if (branch == "decline")
        {
            FinishMouZhangXiuOl(f);
            return;
        }

        if (r.Stage == MouZhangXiuOlStage.Target)
        {
            if (!CanChooseMouZhangXiuOlHunt(f.OwnerSeat))
                throw new InvalidOperationException("Qiulie's limited use was already consumed.");
            var target = choice.Targets.Single();
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZhangXiuOlHuntChosenEvent(f.Id, r.Source, r.GameplayHash, r.Phase!.TurnNumber, target));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZhangXiuOlPhaseOfferedEvent(f.Id, r.Source, r.Phase, target));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouZhangXiuOl = r with { TargetSeat = target, Stage = MouZhangXiuOlStage.Cost } });
            if (!CanUseMouZhangXiuOlHunt(f.OwnerSeat, target) || MouZhangXiuOlDiscardable(f.OwnerSeat).Length == 0)
                FinishMouZhangXiuOl(f);
            else
                PublishMouZhangXiuOl(f);
            return;
        }

        if (branch != "card" || r.Stage is not (MouZhangXiuOlStage.Cost or MouZhangXiuOlStage.Defense or MouZhangXiuOlStage.Penalty))
            throw new InvalidOperationException("Mou Zhang Xiu cannot pay at another cursor.");
        var defense = r.Stage == MouZhangXiuOlStage.Defense;
        var payer = defense ? r.TargetSeat : f.OwnerSeat;
        var cardId = choice.Cards.Single();
        if (!MouZhangXiuOlDiscardable(payer, defense).Any(c => c.Id == cardId) || r.Stage == MouZhangXiuOlStage.Cost && !CanUseMouZhangXiuOlHunt(f.OwnerSeat, r.TargetSeat))
            throw new InvalidOperationException("Qiulie cannot pay an unavailable entity or an illegal Slash.");
        var stage = defense ? MouZhangXiuOlStage.DefenseChildren : r.Stage == MouZhangXiuOlStage.Penalty ? MouZhangXiuOlStage.PenaltyChildren : MouZhangXiuOlStage.CostChildren;
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouZhangXiuOl = r with { Stage = stage, PaidCardId = cardId, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, payer, [cardId], new(defense ? MouZhangXiuOlDefenseReason : stage == MouZhangXiuOlStage.PenaltyChildren ? MouZhangXiuOlPenaltyReason : MouZhangXiuOlCostReason));
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouZhangXiuOl = f.MouZhangXiuOl!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZhangXiuOlDiscardPaidEvent(f.Id, payer, cardId, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, defense));
        ContinueMouZhangXiuOl(f.Id);
    }

    internal bool DrainMouZhangXiuOl(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.AwaitedProgramMovement);
    internal void ContinueMouZhangXiuOl(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouZhangXiuOl: { } r } f || f.Id != id)
            return;
        if (!ValidMouZhangXiuOl(f))
            throw new InvalidOperationException("Mou Zhang Xiu lost its source, phase or paid native return.");
        if (r.Stage is MouZhangXiuOlStage.Target or MouZhangXiuOlStage.Cost or MouZhangXiuOlStage.Defense or MouZhangXiuOlStage.Penalty)
        {
            PublishMouZhangXiuOl(f);
            return;
        }

        if (r.Stage is MouZhangXiuOlStage.Drawing or MouZhangXiuOlStage.Issuing or MouZhangXiuOlStage.NativeUse)
            return;
        if (DrainMouZhangXiuOl(id))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.MouZhangXiuOl!;
        if (r.Stage == MouZhangXiuOlStage.CostChildren)
        {
            IssueMouZhangXiuOlSlash(f);
            return;
        }

        if (r.Stage == MouZhangXiuOlStage.DefenseChildren && _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is { } use)
        {
            foreach (var target in use.TargetSeats)
                _coreProgramCardTargetEffectsOperations.MarkCardEffectIneffective(use.Id, target);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZhangXiuOlQiulieNullifiedEvent(f.Id, use.Id, r.TargetSeat, r.PaidCardId!.Value));
        }

        if (r.Stage == MouZhangXiuOlStage.DrawChildren)
            SetMouZhangXiuOlEntry(r.UseId, r.Source, e => e with { DrawReturned = true });
        if (r.Operation == SkillProgramEffectOp.CompleteMouZhangXiuZhuijiao)
            SetMouZhangXiuOlEntry(r.UseId, r.Source, e => e with { PenaltyReturned = true });
        FinishMouZhangXiuOl(f);
    }

    internal void FinishMouZhangXiuOl(ProgramSkillFrame f)
    {
        var r = f.MouZhangXiuOl!;
        if (r.Operation == SkillProgramEffectOp.CompleteMouZhangXiuZhuijiao && _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is not null)
            SetMouZhangXiuOlEntry(r.UseId, r.Source, e => e with { PenaltyReturned = true });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZhangXiuOlCompletedEvent(f.Id, r.Operation));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouZhangXiuOl = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 33000)]
    internal bool ResumeMouZhangXiuOl(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouZhangXiuOl: not null } f || f.Id != id)
            return false;
        ContinueMouZhangXiuOl(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 33000)]
    internal bool ReturnMouZhangXiuOlMovement(ProgramSkillFrame f) => f.MouZhangXiuOl is not null && ResumeMouZhangXiuOl(f.Id);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveMouZhangXiuQiulie, SkillProgramEffectOp.UseMouZhangXiuQiulieSlash, SkillProgramEffectOp.DefendMouZhangXiuQiulieSlash, SkillProgramEffectOp.CompleteMouZhangXiuZhuijiao)]
    internal PromptChoice AiMouZhangXiuOl(PendingDecision p, ProgramSkillFrame f)
    {
        if (f.MouZhangXiuOl!.Stage == MouZhangXiuOlStage.Target)
            return p.Choices.FirstOrDefault(c => c.Targets is [var target] && !global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[target])) ?? p.Choices.Single(c => c.Parameters["branch"] == "decline");
        if (f.MouZhangXiuOl.Stage == MouZhangXiuOlStage.Cost && global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[f.MouZhangXiuOl.TargetSeat]))
            return p.Choices.Single(c => c.Parameters["branch"] == "decline");
        return p.Choices.Where(c => c.Cards.Count == 1).OrderBy(c => global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards.Single()), _coreMatchParticipantsPlayersAccess._players[p.PlayerSeat])).FirstOrDefault() ?? p.Choices.Single();
    }
}
