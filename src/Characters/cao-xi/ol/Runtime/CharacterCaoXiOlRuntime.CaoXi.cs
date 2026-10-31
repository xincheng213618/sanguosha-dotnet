#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoXiOlRuntime
{
    internal const string CaoXiDrawReason = "skill-program.cao-xi.jianxuan.draw";
    internal static CardConversionSource CaoXiSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_cao_xi_ol_CaoXi.CaoXiSource(f);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 26810)]
    internal bool CanOfferCaoXi(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context)
    {
        if (t.Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveGangshuAttributeIncrement))
            return context.Window == SkillProgramTriggerWindow.CardUseCompleted && context.CardUse is { } use && use.ActorSeat == c.OwnerSeat && !CoreGameEngineProgramCardPaymentsRuntime.MatchesProgramCardCategory(use.EffectiveKind, [SkillProgramCardCategory.Basic]) && CharacterCaoXiOlDefinitions.CaoXiQueries.Any(q => CurrentCaoXiNumbers(c.OwnerSeat).Value(q) < 5);
        if (t.Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveJianxuanRepeatedDraw))
            return context.Window == SkillProgramTriggerWindow.AfterDamageApplied && context.TargetSeat == c.OwnerSeat && context.Amount > 0 && context.DamageFrameId is { } id && _resolutionStackOperations._resolutionStack.OfType<DamageTriggerWindowFrame>().Any(w => w.Id == context.ParentFrameId && w.ParentFrameId == id && w.TargetSeat == c.OwnerSeat && w.TriggerWindow == SkillProgramTriggerWindow.AfterDamageApplied);
        return true;
    }

    internal SkillProgramStepOutcome BeginCaoXi(ProgramSkillFrame input, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        var gangshu = op == SkillProgramEffectOp.ResolveGangshuAttributeIncrement;
        if (f.CaoXiGangshu is not null || f.CaoXiJianxuan is not null || f.InstructionIndex != 1 || f.WindowContext is not { } c || f.SkillId != (gangshu ? "ol:gangshu" : "ol:jianxuan") || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || !CanOfferCaoXi(new(f.OwnerSeat, f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, 0), _coreProgramLifecycleOperations.GetProgramTrigger(f), c))
            throw new InvalidOperationException("Cao Xi requires its exact completed nonbasic use or one actual positive damage instance.");
        if (gangshu)
        {
            var use = c.CardUse!;
            var r = new CaoXiGangshuReceipt(CaoXiSource(f), f.GameplayHash, f.InstructionIndex, c.ParentFrameId, use.ParentCardUseFrameId, use.CardActionId, CurrentCaoXiNumbers(f.OwnerSeat));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CaoXiGangshu = r });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoXiGangshuStartedEvent(f.Id, r.Source, r.GameplayHash, r.ParentWindowId, r.CardUseFrameId, r.ActionId, r.Numbers));
        }
        else
        {
            var r = new CaoXiJianxuanReceipt(CaoXiSource(f), f.GameplayHash, f.InstructionIndex, c.ParentFrameId, c.DamageFrameId!.Value, _coreActualTurnProgressTurnNumberAccess._turnNumber);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CaoXiJianxuan = r });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoXiJianxuanStartedEvent(f.Id, r.Source, r.GameplayHash, r.ParentWindowId, r.DamageFrameId, r.ActualTurn));
        }

        PublishCaoXi(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> CaoXiChoices(ProgramSkillFrame f)
    {
        Dictionary<string, string> Params() => new()
        {
            ["program-action"] = "cao-xi",
            ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture)
        };
        if (f.CaoXiGangshu is { } r)
            return Array.AsReadOnly(CharacterCaoXiOlDefinitions.CaoXiQueries.Where(q => r.Numbers.Value(q) < 5).Select(q => new PromptChoice(new($"cao-xi.{(f.Id)}.{(q)}"), (q == SkillRuleQuery.AttackRange ? "攻击范围" : q == SkillRuleQuery.DrawCount ? "下个摸牌阶段摸牌数" : "出牌阶段使用杀的次数上限") + $"：{(r.Numbers.Value(q))}→{(r.Numbers.Value(q) + 1)}", [], [], new Dictionary<string, string>(Params()) { ["query"] = q.ToString() })).ToArray());
        return Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).OrderBy(p => p.Seat).Select(p => new PromptChoice(new($"cao-xi.{(f.Id)}.recipient.{(p.Seat)}"), $"令{(p.Name)}摸牌", [], [p.Seat], new Dictionary<string, string>(Params()) { ["recipient"] = p.Seat.ToString(CultureInfo.InvariantCulture) })).ToArray());
    }

    internal void PublishCaoXi(ProgramSkillFrame f)
    {
        var choices = CaoXiChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        if (choices.Count == 0)
            throw new InvalidOperationException("Cao Xi has no legal original-owner decision.");
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, f.CaoXiGangshu is null ? "谏旋：选择本次始终摸牌的角色。" : "刚述：选择一项当前小于5的数值+1。", [], choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("cao-xi")]
    internal void AnswerCaoXi(PromptChoice choice)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Cao Xi lost the decision owner.");
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { IsPrivate: true } p || p.PlayerSeat != f.OwnerSeat || !CaoXiChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Cao Xi requires its exact source-owned decision.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (f.CaoXiGangshu is { } r)
        {
            var query = Enum.Parse<SkillRuleQuery>(choice.Parameters["query"]);
            var value = CurrentCaoXiNumbers(f.OwnerSeat).Value(query);
            if (value != r.Numbers.Value(query) || value >= 5)
                throw new InvalidOperationException("Gangshu cannot grow a capped or changed numeric value.");
            var token = _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Draw } draw ? draw.Token : (long? )null;
            var latestPhase = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ActualDiscardRecoveryPhaseStartedEvent>().Select(e => e.Phase.Token).DefaultIfEmpty().Max();
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoXiGangshuIncreasedEvent(f.Id, r.Source, r.GameplayHash, query, value, value + 1, token, latestPhase));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CaoXiGangshu = null });
            _coreSkillProgramsOperations.FinishProgramSkill(f, true);
            return;
        }

        var recipient = int.Parse(choice.Parameters["recipient"], CultureInfo.InvariantCulture);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CaoXiJianxuan = f.CaoXiJianxuan!with { RecipientSeat = recipient, Stage = CaoXiJianxuanStage.DrawChildren } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoXiJianxuanRecipientEvent(f.Id, recipient));
        IssueCaoXiDraw(f);
    }

    internal bool DrainCaoXiChildren(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.Program);
    [ProgramContinuation(ProgramContinuationKind.Resume, 26810)]
    internal bool ResumeCaoXi(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id || f.CaoXiGangshu is null && f.CaoXiJianxuan is null)
            return false;
        if (f.CaoXiGangshu is not null || f.CaoXiJianxuan?.Stage == CaoXiJianxuanStage.ChoosingRecipient)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishCaoXi(f);
            return true;
        }

        if (DrainCaoXiChildren(id))
            return true;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.CaoXiJianxuan!;
        var recipient = r.RecipientSeat!.Value;
        var numbers = CurrentCaoXiNumbers(f.OwnerSeat);
        var count = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[recipient]).Count;
        var repeat = _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[recipient].IsAlive && r.ActualDrawCount == 1 && numbers.Contains(count);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoXiJianxuanComparedEvent(f.Id, r.Cursor, recipient, count, numbers, repeat));
        if (repeat)
        {
            IssueCaoXiDraw(f);
            return true;
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoXiJianxuanCompletedEvent(f.Id, recipient, r.Cursor));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CaoXiJianxuan = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveGangshuAttributeIncrement, SkillProgramEffectOp.ResolveJianxuanRepeatedDraw)]
    internal PromptChoice ChooseCaoXiAi(PendingDecision p, ProgramSkillFrame f)
    {
        if (f.CaoXiGangshu is not null)
            return p.Choices.OrderBy(c => c.Parameters.GetValueOrDefault("query") == "DrawCount" ? 0 : c.Parameters.GetValueOrDefault("query") == "SlashLimit" ? 1 : 2).First();
        var numbers = CurrentCaoXiNumbers(f.OwnerSeat);
        return p.Choices.OrderByDescending(c => c.Targets is [var seat] && seat == f.OwnerSeat ? 1 : 0).ThenByDescending(c => c.Targets is [var seat] && numbers.Contains(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Count + 1) ? 1 : 0).First();
    }

    SkillProgramStepOutcome ICaoXiProgramHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginCaoXi(f, op);
}
