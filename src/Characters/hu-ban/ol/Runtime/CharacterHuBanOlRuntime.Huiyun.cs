#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuBanOlRuntime
{
    SkillProgramStepOutcome global::CardGame.Core.IHuBanHost.Resolve(ProgramSkillFrame f) => BeginHuiyun(f);
    internal SkillProgramStepOutcome BeginHuiyun(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.Huiyun is not null || f.InstructionIndex != 1 || f.TriggerId is null || f.WindowContext is not { } c || HuiyunCandidateUse(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, 0), c)is not { HuiyunOrigin: { } origin } use || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0)
            throw new InvalidOperationException("Huiyun requires its exact whole completed original FireAttack and unstarted issued binding.");
        var opportunities = HuiyunOpportunities(use);
        var r = new HuiyunReceipt(origin, c.ParentFrameId, HuiyunStage.OptionChoice, opportunities);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Huiyun = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HuiyunStartedEvent(f.Id, origin, c.ParentFrameId, opportunities.Count));
        for (var i = 0; i < opportunities.Count; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HuiyunOpportunityIssuedEvent(f.Id, i, opportunities[i]));
        ContinueHuiyun(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal static HuiyunOpportunity HuiyunCurrent(HuiyunReceipt r) => global::CardGame.Core.RuntimeSkills.RuntimeRules_hu_ban_ol_Huiyun.HuiyunCurrent(r);
    internal IReadOnlyList<PromptChoice> HuiyunChoices(ProgramSkillFrame f)
    {
        var r = f.Huiyun!;
        var o = HuiyunCurrent(r);
        var args = new Dictionary<string, string>
        {
            {
                "program-action",
                "huiyun"
            },
            {
                "frame-id",
                f.Id.ToString(CultureInfo.InvariantCulture)
            }
        };
        if (r.Stage == HuiyunStage.OptionChoice)
            return Array.AsReadOnly(Enumerable.Range(1, 3).Where(i => HuiyunOptionAvailable(f.OwnerSeat, r.Origin.RoundNumber, i)).Select(i => new PromptChoice(new($"huiyun.{(f.Id)}.{(r.Index)}.option.{(i)}"), i switch
            {
                1 => "使用原展示牌，然后重铸所有手牌",
                2 => "使用一张手牌，然后重铸原展示牌",
                _ => "可以摸一张牌"
            }, [], [o.TargetSeat], new Dictionary<string, string>(args) { { "option", i.ToString(CultureInfo.InvariantCulture) } })).ToArray());
        var result = new List<PromptChoice>();
        if (r.Option == 3)
            result.Add(new(new($"huiyun.{(f.Id)}.{(r.Index)}.draw"), "摸一张牌", [], [], new Dictionary<string, string>(args) { { "choice", "draw" } }));
        else
            result.AddRange(HuiyunActions(f).Select((a, i) => new PromptChoice(new($"huiyun.{(f.Id)}.{(r.Index)}.use.{(i)}"), a.Description, [a.CardId!.Value], a.TargetSeats, new Dictionary<string, string>(args) { { "choice", "use" }, { "action-index", i.ToString(CultureInfo.InvariantCulture) } })));
        result.Add(new(new($"huiyun.{(f.Id)}.{(r.Index)}.decline"), "不执行此项", [], [], new Dictionary<string, string>(args) { { "choice", "decline" } }));
        return Array.AsReadOnly(result.ToArray());
    }

    internal void PublishHuiyun(ProgramSkillFrame f)
    {
        var r = f.Huiyun!;
        var o = HuiyunCurrent(r);
        var actor = r.Stage == HuiyunStage.OptionChoice ? f.OwnerSeat : o.TargetSeat;
        var choices = HuiyunChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, actor, r.Stage == HuiyunStage.OptionChoice ? "晖云：选择本轮尚未选择的一项。" : "晖云：可以执行此项，或不执行。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = r.Stage != HuiyunStage.OptionChoice,
            TargetSeat = actor,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[actor].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("huiyun")]
    internal void AnswerHuiyun(PromptChoice supplied)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Huiyun: { } r } f || r.Stage is not (HuiyunStage.OptionChoice or HuiyunStage.HandUseChoice) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != (r.Stage == HuiyunStage.OptionChoice ? f.OwnerSeat : HuiyunCurrent(r).TargetSeat) || !HuiyunChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [supplied])))
            throw new InvalidOperationException("Huiyun requires its current published original option or actual target's private natural-card choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        if (r.Stage == HuiyunStage.OptionChoice)
        {
            var option = int.Parse(supplied.Parameters["option"], CultureInfo.InvariantCulture);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HuiyunOptionSpentEvent(f.Id, r.Index, r.Origin.Source, r.Origin.RoundNumber, option));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Huiyun = r with { Option = option, Stage = HuiyunStage.HandUseChoice } });
            ContinueHuiyun(f.Id);
            return;
        }

        var branch = supplied.Parameters["choice"];
        if (branch == "decline")
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HuiyunTargetDeclinedEvent(f.Id, r.Index, HuiyunCurrent(r).TargetSeat));
            FinishHuiyunOpportunity(f);
            return;
        }

        if (branch == "draw")
        {
            IssueHuiyunDraw(f, 1, false);
            return;
        }

        var actions = HuiyunActions(f);
        var action = actions[int.Parse(supplied.Parameters["action-index"], CultureInfo.InvariantCulture)];
        var actor = HuiyunCurrent(r).TargetSeat;
        var native = new HuiyunNativeInvoice(f.Id, r.Index, r.Origin.Source, f.GameplayHash, actor, action.CardId!.Value, action.PlayedCardKind ?? _coreCardAttackStateOperations.GetAttackCard(action.CardId.Value).Kind, action, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Huiyun = r with { Stage = HuiyunStage.Issuing, Native = native } });
        _coreNativeActionDispatchOperations.ExecuteAction(_coreMatchParticipantsPlayersAccess._players[actor], action);
    }

    internal void IssueHuiyunRecast(ProgramSkillFrame f)
    {
        var r = f.Huiyun!;
        var o = HuiyunCurrent(r);
        var target = _coreMatchParticipantsPlayersAccess._players[o.TargetSeat];
        var ids = !target.IsAlive ? [] : r.Option == 1 ? _corePhysicalOwnedCardQueriesOperations.GetHand(target).OrderBy(c => c.Id).Select(c => c.Id).ToArray() : o.ShownCardId is { } shown && shown != r.Native!.CardId && _cardZonesOperations._cardZones.GetLocation(shown) == CardLocation.Hand(target.Seat) ? new[]
        {
            shown
        }

        : [];
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        f = _coreNativeRecastInvocationsOperations.BeginFrameNativeRecastInvocation(f, target.Seat, ids, r.Index);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Huiyun = r with { Stage = HuiyunStage.RecastCostChildren, Before = before, After = before, ActualCount = ids.Length }, PendingMovementContinuation = new(target.Seat, 0, null) });
        if (ids.Length > 0)
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, CardLocation.DiscardPile, CardMoveReasons.RecastDiscard, (_, records) =>
            {
                foreach (var m in records)
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HuiyunRecastCardEvent(f.Id, r.Index, m.CardId, m.CardKind, target.Seat, m.Sequence));
            }, destinationForCard: static (card, _) => card.IsGeneralWeapon ? CardLocation.OutsideGame : CardLocation.DiscardPile);
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Huiyun = f.Huiyun!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HuiyunMovementPaidEvent(f.Id, r.Index, HuiyunStage.RecastCostChildren, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, ids.Length));
        ContinueHuiyun(f.Id);
    }

    internal void IssueHuiyunDraw(ProgramSkillFrame f, int requested, bool recast)
    {
        var r = f.Huiyun!;
        var actor = HuiyunCurrent(r).TargetSeat;
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        var stage = recast ? HuiyunStage.RecastDrawChildren : HuiyunStage.DrawChildren;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Huiyun = r with { Stage = stage, Before = before, After = before, ActualCount = 0 }, PendingMovementContinuation = new(actor, 0, null) });
        var actual = requested > 0 && _coreMatchParticipantsPlayersAccess._players[actor].IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None ? _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[actor], requested, true, recast ? CardMoveReasons.RecastDraw : new("skill-program.hu-ban.huiyun.draw")).Count : 0;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Huiyun = f.Huiyun!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, ActualCount = actual } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HuiyunMovementPaidEvent(f.Id, r.Index, stage, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, actual));
        if (recast)
        {
            var costs = _coreProgramEventHistoryOperations.ProgramEventHistory<HuiyunRecastCardEvent>().Where(e => e.FrameId == f.Id && e.Index == r.Index).ToArray();
            for (var i = 0; i < costs.Length; i++)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardRecastEvent(actor, costs[i].CardId, costs[i].Kind, i < actual ? 1 : 0));
            _coreNativeRecastInvocationsOperations.CompleteFrameNativeRecastInvocation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), actual);
        }

        ContinueHuiyun(f.Id);
    }

    internal bool DrainHuiyun(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, f.PendingMovementContinuation is null ? PostEventContinuation.Program : PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, f.PendingMovementContinuation is null ? PostEventContinuation.Program : PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id);
    internal void ContinueHuiyun(long id)
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { Huiyun: { } r } f && f.Id == id)
        {
            if (r.Index == r.Opportunities.Count)
            {
                FinishHuiyun(f);
                return;
            }

            if (r.Stage is HuiyunStage.OptionChoice or HuiyunStage.HandUseChoice)
            {
                if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[HuiyunCurrent(r).TargetSeat].IsAlive || r.Stage == HuiyunStage.OptionChoice && (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !HasHuiyunOption(f.OwnerSeat, r.Origin.RoundNumber)))
                {
                    FinishHuiyunOpportunity(f);
                    return;
                }

                if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                    PublishHuiyun(f);
                return;
            }

            if (r.Stage is HuiyunStage.Issuing or HuiyunStage.NativeUse)
                return;
            if (DrainHuiyun(f))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.Huiyun!;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
            if (r.Stage == HuiyunStage.NativeChildren)
            {
                IssueHuiyunRecast(f);
                return;
            }

            if (r.Stage == HuiyunStage.RecastCostChildren)
            {
                IssueHuiyunDraw(f, _coreProgramEventHistoryOperations.ProgramEventHistory<HuiyunRecastCardEvent>().Count(e => e.FrameId == f.Id && e.Index == r.Index), true);
                return;
            }

            FinishHuiyunOpportunity(f);
            return;
        }
    }

    internal void FinishHuiyunOpportunity(ProgramSkillFrame f)
    {
        var r = f.Huiyun!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HuiyunOpportunityCompletedEvent(f.Id, r.Index));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Huiyun = r with { Index = r.Index + 1, Option = 0, Stage = HuiyunStage.OptionChoice, Native = null, NativeCompleted = false, Before = 0, After = 0, ActualCount = 0 }, PendingMovementContinuation = null });
        ContinueHuiyun(f.Id);
    }

    internal void FinishHuiyun(ProgramSkillFrame f)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HuiyunCompletedEvent(f.Id));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Huiyun = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 27380)]
    internal bool ResumeHuiyun(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Huiyun: not null } f || f.Id != id)
            return false;
        ContinueHuiyun(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 27380)]
    internal bool ReturnHuiyunMovement(ProgramSkillFrame f)
    {
        if (f.Huiyun is null)
            return false;
        ContinueHuiyun(f.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27380)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27380)]
    internal bool ContinuePaidHuiyun(ProgramSkillFrame f)
    {
        if (f.Huiyun is null)
            return false;
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveHuiyunCompletedFireAttack)]
    internal PromptChoice ChooseHuiyun(PendingDecision p, ProgramSkillFrame f)
    {
        if (f.Huiyun!.Stage == HuiyunStage.OptionChoice)
            return p.Choices.OrderByDescending(c => c.Parameters.GetValueOrDefault("option") == "3" ? 3 : c.Parameters.GetValueOrDefault("option") == "2" ? 2 : 1).First();
        var actions = HuiyunActions(f);
        var best = p.Choices.Where(c => c.Parameters.GetValueOrDefault("choice") == "use").Select(c => (Choice: c, Score: _coreAiBrainsAiBrainsAccess._aiBrains[HuiyunCurrent(f.Huiyun).TargetSeat].ScoreProgramTarget(_corePlayerViewProjectionOperations.CreateSnapshot(HuiyunCurrent(f.Huiyun).TargetSeat), actions[int.Parse(c.Parameters["action-index"], CultureInfo.InvariantCulture)].TargetSeats.FirstOrDefault(HuiyunCurrent(f.Huiyun).TargetSeat), new SkillProgramAiHint(1, 0, 0, 0, 0, 0, false, false)))).OrderByDescending(x => x.Score).FirstOrDefault();
        return p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("choice") == "draw") ?? best.Choice ?? p.Choices.Last();
    }
}
