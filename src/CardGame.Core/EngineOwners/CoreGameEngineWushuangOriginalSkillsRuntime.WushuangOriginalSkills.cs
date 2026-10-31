#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineWushuangOriginalSkillsRuntime
{
    SkillProgramStepOutcome IWushuangOriginalSkillHost.Execute(ProgramSkillFrame frame, SkillProgramEffectOp operation) => BeginWushuangOriginalSkill(frame, operation);
    internal bool WushuangOriginalEnabled(CharacterState owner, string id) => owner.IsAlive && _contentRegistryOperations._contentRegistry.Skills.ContainsKey(id) && _coreSkillProgramsOperations.EnabledSkillPrograms(owner).Any(p => p.Id == id);
    internal SkillProgramStepOutcome BeginWushuangOriginalSkill(ProgramSkillFrame supplied, SkillProgramEffectOp operation)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.WushuangOriginalSkill is not null || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != operation)
            throw new InvalidOperationException("An original Wushuang skill requires its exact unpaid instruction.");
        var r = new WushuangOriginalSkillReceipt(operation, f.InstructionIndex, _coreActualTurnProgressTurnNumberAccess._turnNumber, f.WindowContext?.ParentFrameId, "begin");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WushuangOriginalSkill = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WushuangOriginalSkillIssuedEvent(f.Id, f.SkillId, f.OwnerSeat, operation, f.InstructionIndex, _coreActualTurnProgressTurnNumberAccess._turnNumber, r.WindowId));
        RunWushuangOriginalSkill(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal ProgramSkillFrame UpdateWushuangOriginal(ProgramSkillFrame f, WushuangOriginalSkillReceipt r)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { WushuangOriginalSkill = r });
        return f;
    }

    internal ProgramSkillFrame WushuangOriginalStage(ProgramSkillFrame f, string stage) => UpdateWushuangOriginal(f, f.WushuangOriginalSkill!with { Stage = stage });
    internal bool DrainWushuangOriginal(long id) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.Program) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id);
    [ProgramContinuation(ProgramContinuationKind.Resume, 61010)]
    internal bool RunWushuangOriginalSkill(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { WushuangOriginalSkill: { } r } f || f.Id != id)
            return false;
        if (r.Stage == "draw-attempt")
            return true;
        if (DrainWushuangOriginal(id))
            return true;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.WushuangOriginalSkill!;
        if (r.Stage == "finish")
        {
            FinishWushuangOriginal(f);
            return true;
        }

        switch (r.Operation)
        {
            case SkillProgramEffectOp.ResolveWushuangOriginalYiji:
                _coreWushuangOriginalSkillFlowsOperations.ContinueWushuangYiji(f);
                break;
            case SkillProgramEffectOp.ResolveWushuangYashi:
                _coreWushuangOriginalSkillFlowsOperations.ContinueWushuangYashi(f);
                break;
            case SkillProgramEffectOp.ResolveWushuangGuanxu:
                _coreWushuangOriginalSkillFlowsOperations.ContinueWushuangGuanxu(f);
                break;
            case SkillProgramEffectOp.ResolveWushuangChoufa:
                _coreWushuangOriginalSkillFlowsOperations.ContinueWushuangChoufa(f);
                break;
            case SkillProgramEffectOp.ResolveWushuangShenfu:
                _coreWushuangOriginalSkillFlowsOperations.ContinueWushuangShenfu(f);
                break;
            case SkillProgramEffectOp.ResolveWushuangLingren:
                _coreWushuangOriginalSkillFlowsOperations.ContinueWushuangLingren(f);
                break;
            case SkillProgramEffectOp.ResolveWushuangTairan:
                _coreWushuangOriginalSkillFlowsOperations.ContinueWushuangTairan(f);
                break;
            case SkillProgramEffectOp.SettleWushuangTairan:
                _coreWushuangOriginalSkillFlowsOperations.ContinueWushuangTairanSettlement(f);
                break;
            case SkillProgramEffectOp.NullifyWushuangSecondUse:
                _coreWushuangOriginalSkillRulesOperations.NullifyWushuangCiweiUse(f);
                break;
            case SkillProgramEffectOp.ResolveWushuangOriginalJieming:
                _coreWushuangOriginalSkillFlowsOperations.ContinueWushuangJieming(f);
                break;
            case SkillProgramEffectOp.PayAndDrawWushuangJinzhi:
                _coreWushuangJinzhiOperations.ContinueWushuangJinzhi(f);
                break;
            case SkillProgramEffectOp.SkipWushuangFenliPhase:
                _coreWushuangOriginalSkillRulesOperations.SkipWushuangFenliPhase(f);
                break;
            case SkillProgramEffectOp.RestrictWushuangQianxiJudgmentColor:
                _coreWushuangOriginalSkillRulesOperations.RestrictWushuangQianxiJudgmentColor(f);
                break;
            default:
                throw new InvalidOperationException("Unknown official Wushuang skill operation.");
        }

        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 61010)]
    internal bool ReturnWushuangOriginalMovement(ProgramSkillFrame f) => f.WushuangOriginalSkill is not null && RunWushuangOriginalSkill(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 61010)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 61010)]
    internal bool ContinuePaidWushuangOriginal(ProgramSkillFrame f) => f.WushuangOriginalSkill is { Stage: not "begin" };
    internal void FinishWushuangOriginal(ProgramSkillFrame f, bool completed = true)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WushuangOriginalSkillFinishedEvent(f.Id, completed));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { WushuangOriginalSkill = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal void DrawWushuangOriginal(ProgramSkillFrame f, int recipient, int count, string next)
    {
        f = UpdateWushuangOriginal(f, f.WushuangOriginalSkill!with { Stage = "draw-attempt", DrawOrdinal = f.WushuangOriginalSkill.DrawOrdinal + 1, DrawRecipient = recipient, DrawRequested = Math.Max(0, count), NextStage = next, Draw = null });
        _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, recipient, Math.Max(0, count), new("skill-program." + f.SkillId + ".draw"));
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveWushuangShenfu, SkillProgramEffectOp.ResolveWushuangLingren, SkillProgramEffectOp.ResolveWushuangTairan, SkillProgramEffectOp.ResolveWushuangOriginalJieming, SkillProgramEffectOp.PayAndDrawWushuangJinzhi)]
    internal void ReturnWushuangOriginalDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        var r = f.WushuangOriginalSkill ?? throw new InvalidOperationException("The official skill draw lost its producer.");
        if (r.Stage != "draw-attempt" || outcome.RecipientSeat != r.DrawRecipient || outcome.RequestedCount != r.DrawRequested || _coreProgramEventHistoryOperations.ProgramEventHistory<WushuangOriginalSkillDrawEvent>().Any(e => e.FrameId == f.Id && e.Ordinal == r.DrawOrdinal))
            throw new InvalidOperationException("The official skill draw changed its paid recipient or requested count.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WushuangOriginalSkillDrawEvent(f.Id, r.DrawOrdinal, outcome.RecipientSeat, outcome.RequestedCount, outcome.ActualDrawIds.Count, outcome.SequenceBefore, outcome.SequenceAfter, outcome.AttemptFrameId, outcome.Replacement is not null));
        if (r.Operation == SkillProgramEffectOp.ResolveWushuangTairan)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WushuangTairanDrawnEvent(f.Id, f.OwnerSeat, f.SkillInstanceId, _coreActualTurnProgressTurnNumberAccess._turnNumber, outcome.ActualDrawIds));
        UpdateWushuangOriginal(f, r with { Stage = r.NextStage!, NextStage = null, Draw = outcome });
        RunWushuangOriginalSkill(f.Id);
    }

    internal PromptChoice WushuangOriginalChoice(ProgramSkillFrame f, string option, string label, IReadOnlyList<int>? cards = null, IReadOnlyList<int>? targets = null) => new(new($"wushuang-original.{(f.Id)}.{(f.WushuangOriginalSkill!.Stage)}.{(option)}.{(string.Join('-', cards ?? []))}.{(string.Join('-', targets ?? []))}"), label, cards ?? [], targets ?? [], new Dictionary<string, string> { ["program-action"] = "wushuang-original", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["option"] = option });
    internal void PublishWushuangOriginalChoice(ProgramSkillFrame f, IReadOnlyList<PromptChoice> choices)
    {
        if (choices.Count == 0)
        {
            FinishWushuangOriginal(f, false);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, $"【{(skill.Name)}】请选择。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = f.OwnerSeat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    internal PromptChoice[] WushuangOtherTargets(ProgramSkillFrame f, string option, Func<CharacterState, bool>? condition = null) => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat && (condition?.Invoke(p) ?? true)).Select(p => WushuangOriginalChoice(f, option, p.Name, targets: [p.Seat])).ToArray();
    [ProgramViewContributor(ProgramViewContribution.PrivateCards, 61010)]
    internal IEnumerable<CardSnapshot> WushuangOriginalPrivateCards(int viewer) => _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Where(f => f.OwnerSeat == viewer && f.WushuangOriginalSkill is { Operation: SkillProgramEffectOp.ResolveWushuangOriginalYiji or SkillProgramEffectOp.ResolveWushuangGuanxu or SkillProgramEffectOp.ResolveWushuangYashi }).SelectMany(f => f.WushuangOriginalSkill!.Cards.Concat(f.WushuangOriginalSkill.Top)).Distinct().Select(id => _coreSnapshotValueProjectionOperations.ToSnapshot(_coreAdvancedLifecycleOperations.GetAdvancedCard(id)));
}
