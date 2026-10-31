#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDiaoChanOlMagicRuntime
{
    SkillProgramStepOutcome IMoDiaoChanHost.Resolve(ProgramSkillFrame frame, SkillProgramEffectOp op) => BeginMoDc(frame, op);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 31400)]
    internal bool CanOfferMoDc(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context)
    {
        if (!t.Effects.Any(e => MoDiaoChanComposition.Handles(e.Op)))
            return true;
        if (t.Effects.Count != 1)
            return false;
        var op = t.Effects[0].Op;
        if (op == SkillProgramEffectOp.ResolveMoDiaoChanHuanhuoUse)
            return IsIssuedMoDiaoChanCandidate(c, context);
        if (op == SkillProgramEffectOp.ResolveMoDiaoChanHuanhuoRound)
            return context.Window == SkillProgramTriggerWindow.RoundStarting && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<MoDiaoChanStartedEvent>().Any(e => e.Source.OwnerSeat == c.OwnerSeat && e.Source.SkillInstanceId == c.SkillInstanceId && e.Op == op && e.Round == _coreActualRoundProgressRoundNumberAccess._roundNumber);
        if (op == SkillProgramEffectOp.ResolveMoDiaoChanQingshiPrep)
            return context.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow && context.SourceSeat == c.OwnerSeat && _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Preparation } phase && phase.ActorSeat == c.OwnerSeat && (!_characterSharedGameEngineOlMoRumoOperations.IsOlMoRumoEntered(c.OwnerSeat) || MoDcRefreshReady(c.OwnerSeat));
        if (op == SkillProgramEffectOp.ResolveMoDiaoChanQingshiRedirect)
            return MoDcCurrentMarkedUse(c.OwnerSeat, context)is { } use && MoDcDiscardable(c.OwnerSeat).Length > 0 && MoDcRedirectTargets(use).Length > 0 && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<MoDiaoChanStartedEvent>().Any(e => e.Source.OwnerSeat == c.OwnerSeat && e.Op == op && e.WindowId == context.ParentFrameId);
        if (op == SkillProgramEffectOp.ResolveMoDiaoChanQingshiDamage)
            return MoDcDamageEligible(c.OwnerSeat, context);
        return MoDcRecaptureCards(c.OwnerSeat, context).Length > 0;
    }

    internal SkillProgramStepOutcome BeginMoDc(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var c = f.WindowContext ?? throw new InvalidOperationException("Magic Diao Chan requires her native trigger window.");
        var candidate = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, c.OccurrenceIndex);
        if (f.MoDiaoChan is not null || f.InstructionIndex != 1 || !CanOfferMoDc(candidate, _coreProgramLifecycleOperations.GetProgramTrigger(f), c))
            throw new InvalidOperationException("Magic Diao Chan lost her exact unissued trigger.");
        var stage = op switch
        {
            SkillProgramEffectOp.ResolveMoDiaoChanHuanhuoRound or SkillProgramEffectOp.ResolveMoDiaoChanQingshiDamage => MoDiaoChanStage.Drawing,
            SkillProgramEffectOp.ResolveMoDiaoChanQingshiPrep => MoDiaoChanStage.EntryChoice,
            _ => MoDiaoChanStage.CostChildren
        };
        var r = new MoDiaoChanReceipt(MoDcSource(f), f.GameplayHash, op, c.ParentFrameId, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, _coreActualRoundProgressRoundNumberAccess._roundNumber, stage);
        if (op == SkillProgramEffectOp.ResolveMoDiaoChanHuanhuoUse)
        {
            var q = MoDcCompletedClaim(candidate, c)!;
            r = r with
            {
                UseId = q.ParentFrameId,
                ActionId = q.ActionId,
                ActorSeat = q.ActorSeat,
                ProviderSeat = q.ProviderSeat,
                PromiseId = q.PromiseId,
                Paid = true
            };
        }

        if (op == SkillProgramEffectOp.ResolveMoDiaoChanQingshiRedirect)
        {
            var use = MoDcCurrentMarkedUse(f.OwnerSeat, c)!;
            r = r with
            {
                Stage = MoDiaoChanStage.DiscardChoice,
                UseId = use.Id,
                ActionId = use.Action!.ActionId,
                ActorSeat = use.SourceSeat,
                ProviderSeat = use.Action.ProviderSeat,
                PreviousTarget = use.TargetSeats.Single()
            };
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MoDiaoChan = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoDiaoChanStartedEvent(f.Id, r.Source, r.GameplayHash, op, r.WindowId, r.ActualTurn, r.Round));
        if (op == SkillProgramEffectOp.ResolveMoDiaoChanHuanhuoUse)
            PayMoDcRandomDiscard(f);
        else if (op == SkillProgramEffectOp.ResolveMoDiaoChanQingshiDiscard)
            RecaptureMoDcCards(f);
        else if (op == SkillProgramEffectOp.ResolveMoDiaoChanQingshiPrep && _characterSharedGameEngineOlMoRumoOperations.IsOlMoRumoEntered(f.OwnerSeat))
        {
            StartMoDcCohort(f);
            return SkillProgramStepOutcome.AwaitChild;
        }

        ContinueMoDc(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal PromptChoice MoDcChoice(ProgramSkillFrame f, string branch, string text, int? card = null, int? target = null) => new(new($"mo-diao-chan.{(f.Id)}.{(branch)}.{(card)}.{(target)}"), text, card is { } id ? [id] : [], target is { } seat ? [seat] : [], new Dictionary<string, string> { ["program-action"] = "mo-diao-chan", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch });
    internal IReadOnlyList<PromptChoice> MoDcChoices(ProgramSkillFrame f)
    {
        var r = f.MoDiaoChan!;
        var result = new List<PromptChoice>();
        if (r.Stage == MoDiaoChanStage.DiscardChoice)
        {
            var max = r.Op == SkillProgramEffectOp.ResolveMoDiaoChanHuanhuoRound ? 2 : 1;
            if (r.Cards.Count < max)
                foreach (var card in MoDcDiscardable(f.OwnerSeat).Where(c => !r.Cards.Contains(c.Id)))
                    result.Add(MoDcChoice(f, "card", $"选择弃置【{(card.DisplayName)}】", card.Id));
            result.Add(MoDcChoice(f, "confirm", r.Cards.Count == 0 ? "不弃置" : $"弃置所选{(r.Cards.Count)}张牌"));
        }

        if (r.Stage == MoDiaoChanStage.TargetChoice)
            foreach (var p in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat && !r.Targets.Contains(p.Seat)))
                result.Add(MoDcChoice(f, "target", $"令{(p.Name)}下回合受到幻惑", target: p.Seat));
        if (r.Stage == MoDiaoChanStage.EntryChoice)
        {
            result.Add(MoDcChoice(f, "enter", "入魔，令所有角色获得单目标伤害牌"));
            result.Add(MoDcChoice(f, "decline", "不入魔"));
        }

        if (r.Stage == MoDiaoChanStage.RedirectChoice && _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is { } use)
            foreach (var target in MoDcRedirectTargets(use))
                result.Add(MoDcChoice(f, "redirect", $"重新指定{(_coreMatchParticipantsPlayersAccess._players[target].Name)}为目标", target: target));
        return Array.AsReadOnly(result.ToArray());
    }

    internal void PublishMoDc(ProgramSkillFrame f)
    {
        var choices = MoDcChoices(f);
        if (choices.Count == 0)
        {
            FinishMoDc(f);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, skill.Name + "：选择本次处理。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            Choices = choices,
            IsPrivate = true,
            TargetSeat = f.OwnerSeat,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("mo-diao-chan")]
    internal void AnswerMoDc(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MoDiaoChan: { } r } f || !ValidMoDc(f) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !MoDcChoices(f).Any(c => c.Id == choice.Id && CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Magic Diao Chan choice changed its original private frame.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var branch = choice.Parameters["branch"];
        if (branch == "card")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MoDiaoChan = r with { Cards = r.Cards.Append(choice.Cards.Single()).ToArray() } });
            PublishMoDc(f);
            return;
        }

        if (branch == "confirm")
        {
            if (r.Cards.Count == 0)
            {
                FinishMoDc(f);
                return;
            }

            if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
            {
                FinishMoDc(f);
                return;
            }

            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MoDiaoChan = r = r with { Paid = true, Before = before, After = before, Stage = MoDiaoChanStage.CostChildren }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            foreach (var card in r.Cards.Select(_coreAdvancedLifecycleOperations.GetAdvancedCard))
                MoDcDiscardOne(f, f.OwnerSeat, card);
            SealMoDcMovement(f.Id, before);
            ContinueMoDc(f.Id);
            return;
        }

        if (branch == "target")
        {
            var target = choice.Targets.Single();
            var targets = r.Targets.Append(target).ToArray();
            var promise = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MoDiaoChanPromiseEvent>().Select(e => e.PromiseId).DefaultIfEmpty().Max() + 1;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoDiaoChanPromiseEvent(promise, f.Id, r.Source, r.GameplayHash, target, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<TurnStartedEvent>().Count()));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MoDiaoChan = r with { Targets = targets } });
            if (targets.Length >= r.CostCount)
                FinishMoDc(f);
            else
                PublishMoDc(f);
            return;
        }

        if (branch == "decline")
        {
            FinishMoDc(f);
            return;
        }

        if (branch == "enter")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MoDiaoChan = r with { Paid = true } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoDiaoChanQingshiEnteredEvent(f.Id, r.Source, r.GameplayHash, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, _coreActualRoundProgressRoundNumberAccess._roundNumber));
            _characterSharedGameEngineOlMoRumoOperations.EnterOlMoRumo(f);
            StartMoDcCohort(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
            return;
        }

        if (branch == "redirect")
        {
            ReplaceMoDcUniqueTarget(f, choice.Targets.Single());
            return;
        }

        throw new InvalidOperationException("Unknown Magic Diao Chan choice.");
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveMoDiaoChanHuanhuoRound, SkillProgramEffectOp.ResolveMoDiaoChanQingshiDamage)]
    internal void ReturnMoDcDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        if (f.MoDiaoChan is not { Stage: MoDiaoChanStage.Drawing } r || r.DrawOutcome is not null || outcome.RecipientSeat != f.OwnerSeat || outcome.RequestedCount != (r.Op == SkillProgramEffectOp.ResolveMoDiaoChanHuanhuoRound ? 2 : 1))
            throw new InvalidOperationException("Magic Diao Chan lost its original native Draw invoice.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MoDiaoChan = r with { Stage = MoDiaoChanStage.DrawChildren, DrawOutcome = outcome, Before = outcome.SequenceBefore, After = outcome.SequenceAfter }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoDiaoChanDrawReturnedEvent(f.Id, outcome.RequestedCount, outcome.ActualDrawIds.Count, outcome.ReplacementGainedIds.Count, outcome.SequenceBefore, outcome.SequenceAfter, outcome.AttemptFrameId));
        SealMoDcMovement(f.Id, outcome.SequenceBefore);
        ContinueMoDc(f.Id);
    }

    internal void ContinueMoDc(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MoDiaoChan: { } r } f || f.Id != id)
            return;
        if (!ValidMoDc(f))
            throw new InvalidOperationException("Magic Diao Chan lost its typed native receipt.");
        if (r.Stage is MoDiaoChanStage.EntryChoice or MoDiaoChanStage.DiscardChoice or MoDiaoChanStage.TargetChoice or MoDiaoChanStage.RedirectChoice)
        {
            PublishMoDc(f);
            return;
        }

        if (r.Stage == MoDiaoChanStage.Drawing)
        {
            if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            {
                FinishMoDc(f);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MoDiaoChan = r with { Paid = true } });
            if (f.NativeDraw is null)
                _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, r.Op == SkillProgramEffectOp.ResolveMoDiaoChanHuanhuoRound ? 2 : 1, new(r.Op == SkillProgramEffectOp.ResolveMoDiaoChanHuanhuoRound ? "skill-program.ol:mo-huanhuo.draw" : "skill-program.ol:mo-qingshi.draw"));
            return;
        }

        if (r.Stage == MoDiaoChanStage.Obtaining)
        {
            if (r.Cursor >= r.Targets.Count || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                FinishMoDc(f);
                return;
            }

            ObtainMoDcCohortCard(f);
            ContinueMoDc(id);
            return;
        }

        if (DrainMoDc(f))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.MoDiaoChan!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            FinishMoDc(f);
            return;
        }

        if (r.Stage == MoDiaoChanStage.DrawChildren && r.Op == SkillProgramEffectOp.ResolveMoDiaoChanHuanhuoRound && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MoDiaoChan = r with { Stage = MoDiaoChanStage.DiscardChoice } });
            PublishMoDc(f);
            return;
        }

        if (r.Stage == MoDiaoChanStage.CostChildren && r.Op == SkillProgramEffectOp.ResolveMoDiaoChanHuanhuoRound && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            var count = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MoDiaoChanCostEvent>().Count(e => e.FrameId == f.Id);
            if (count > 0)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MoDiaoChan = r with { Stage = MoDiaoChanStage.TargetChoice, CostCount = Math.Min(count, _coreMatchParticipantsPlayersAccess._players.Count(p => p.IsAlive && p.Seat != f.OwnerSeat)) } });
                PublishMoDc(f);
                return;
            }
        }

        if (r.Stage == MoDiaoChanStage.CostChildren && r.Op == SkillProgramEffectOp.ResolveMoDiaoChanQingshiRedirect && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MoDiaoChan = r with { Stage = MoDiaoChanStage.RedirectChoice } });
            PublishMoDc(f);
            return;
        }

        if (r.Stage == MoDiaoChanStage.GainChildren && r.Op == SkillProgramEffectOp.ResolveMoDiaoChanQingshiPrep)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MoDiaoChan = r with { Stage = MoDiaoChanStage.Obtaining, Cursor = r.Cursor + 1 } });
            ContinueMoDc(id);
            return;
        }

        FinishMoDc(f);
    }

    internal void FinishMoDc(ProgramSkillFrame f)
    {
        var r = f.MoDiaoChan!;
        if (r.Op == SkillProgramEffectOp.ResolveMoDiaoChanHuanhuoUse)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoDiaoChanUseSettledEvent(f.Id, r.PromiseId, r.ActionId));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoDiaoChanCompletedEvent(f.Id, r.Op, r.Paid));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MoDiaoChan = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveMoDiaoChanHuanhuoRound, SkillProgramEffectOp.ResolveMoDiaoChanQingshiPrep, SkillProgramEffectOp.ResolveMoDiaoChanQingshiRedirect)]
    internal PromptChoice ChooseMoDcAi(PendingDecision p, ProgramSkillFrame f)
    {
        var r = f.MoDiaoChan!;
        if (r.Stage == MoDiaoChanStage.DiscardChoice)
        {
            var max = r.Op == SkillProgramEffectOp.ResolveMoDiaoChanHuanhuoRound ? Math.Min(2, _coreMatchParticipantsPlayersAccess._players.Count(x => x.IsAlive && x.Seat != f.OwnerSeat)) : 1;
            return r.Cards.Count < max ? p.Choices.Where(c => c.Cards.Count == 1).OrderBy(c => CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards[0]), _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])).FirstOrDefault() ?? p.Choices.Single(c => c.Parameters["branch"] == "confirm") : p.Choices.Single(c => c.Parameters["branch"] == "confirm");
        }

        if (r.Stage == MoDiaoChanStage.TargetChoice || r.Stage == MoDiaoChanStage.RedirectChoice)
            return p.Choices.OrderBy(c => CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[c.Targets.Single()]) ? 1 : 0).ThenBy(c => c.Targets.Single()).First();
        return p.Choices.First(c => c.Parameters["branch"] == "enter");
    }
}
