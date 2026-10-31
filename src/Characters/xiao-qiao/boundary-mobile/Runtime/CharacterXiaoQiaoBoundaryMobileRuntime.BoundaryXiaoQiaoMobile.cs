#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXiaoQiaoBoundaryMobileRuntime
{
    internal const string BoundaryXiaoQiaoHongyan = "boundary:hongyan-mobile";
    internal Suit? BoundaryXiaoQiaoEffectiveSuit(CharacterState owner, Card card, CardLocation? identityLocation)
    {
        if (card.Suit != Suit.Spade || !_coreSkillProgramsOperations.EnabledSkillPrograms(owner).Any(p => p.Id == BoundaryXiaoQiaoHongyan))
            return null;
        var location = identityLocation ?? _cardZonesOperations._cardZones.GetLocation(card.Id);
        return location == CardLocation.Hand(owner.Seat) || _resolutionStackOperations._resolutionStack.OfType<JudgmentFrame>().Any(j => j.TargetSeat == owner.Seat && j.CardId == card.Id && location == CardLocation.Processing) ? Suit.Heart : null;
    }

    internal Suit EffectiveBoundaryXiaoQiaoJudgmentSuit(JudgmentFrame frame, Suit fallback)
    {
        if (frame.BoundaryXiaoQiaoOverride is not { } r)
            return fallback;
        if (r.JudgmentFrameId != frame.Id || r.CardId != frame.CardId || r.Source.SkillId != BoundaryXiaoQiaoHongyan || r.Suit is not (Suit.Spade or Suit.Heart or Suit.Club or Suit.Diamond) || r.GameplayHash != _contentRegistryOperations._contentRegistry.GetSkill(r.Source.SkillId).Program?.GameplayHash || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<BoundaryXiaoQiaoJudgmentRewrittenEvent>().Count(e => e.Receipt == r) != 1)
            throw new InvalidOperationException("Mobile Hongyan lost its exact original judgment card and committed result.");
        // These before-result children are issued in native frame order. A later
        // result rewrite must supersede this one on the same judgment.
        return frame.ZhenyiOverride is { } zhenyi && zhenyi.ProgramFrameId > r.ProgramFrameId || frame.HellDayuanOverride is { } dayuan && dayuan.ProgramFrameId > r.ProgramFrameId ? fallback : r.Suit;
    }

    internal bool IsExactBoundaryXiaoQiaoBeforeResultProgramChild(ProgramJudgmentTriggerWindowFrame w, ProgramSkillFrame f) => f.SkillId == BoundaryXiaoQiaoHongyan && w.ZhenyiBeforeResult == true && w.Activated && f.WindowContext is { Window: SkillProgramTriggerWindow.JudgmentBeforeResult, Judgment: { } j } c && c.ParentFrameId == w.Id && j == w.Judgment && _resolutionStackOperations._resolutionStack.OfType<JudgmentFrame>().SingleOrDefault(p => p.Id == w.ParentFrameId)is { ZhenyiBeforeResultCompleted: not true } pending && pending.Id == j.JudgmentFrameId && _coreNativeDamageHandlesOperations.Value.GetJudgmentCard(pending)?.Id == j.CardId && w.CandidateIndex >= 0 && w.CandidateIndex < w.Candidates.Count && w.Candidates[w.CandidateIndex] is { } b && b.OwnerSeat == f.OwnerSeat && b.SkillId == f.SkillId && b.TriggerId == f.TriggerId && b.SkillInstanceId == f.SkillInstanceId && b.GameplayHash == f.GameplayHash;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 66700)]
    internal bool QualifyBoundaryXiaoQiaoMobile(ProgramTriggerCandidate b, ProgramSkillWindowContext c) => b.SkillId != BoundaryXiaoQiaoHongyan || c is { Window: SkillProgramTriggerWindow.JudgmentBeforeResult, Judgment.Suit: Suit.Heart };
    SkillProgramStepOutcome IBoundaryXiaoQiaoMobileHost.Rewrite(ProgramSkillFrame f) => BeginBoundaryXiaoQiaoJudgment(f);
    internal SkillProgramStepOutcome BeginBoundaryXiaoQiaoJudgment(ProgramSkillFrame f)
    {
        if (f.WindowContext?.Judgment?.Suit != Suit.Heart)
            throw new InvalidOperationException("Mobile Hongyan requires the current heart result before it takes effect.");
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "红颜：指定本次判定结果的花色。", [], [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            Choices = BoundaryXiaoQiaoJudgmentChoices(f),
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal static IReadOnlyList<PromptChoice> BoundaryXiaoQiaoJudgmentChoices(ProgramSkillFrame f) => Array.AsReadOnly(new[] { Suit.Heart, Suit.Spade, Suit.Club, Suit.Diamond }.Select(s => new PromptChoice(new($"mobile-hongyan.{(f.Id)}.{(s)}"), s switch
    {
        Suit.Heart => "红桃",
        Suit.Spade => "黑桃",
        Suit.Club => "梅花",
        _ => "方块"
    }, [], [], new Dictionary<string, string> { ["program-action"] = "boundary-xiao-qiao-judgment", ["suit"] = s.ToString() })).ToArray());
    [ProgramAnswer("boundary-xiao-qiao-judgment")]
    internal void AnswerBoundaryXiaoQiaoJudgment(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { SkillId: BoundaryXiaoQiaoHongyan } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !BoundaryXiaoQiaoJudgmentChoices(f).Any(c => c.Id == choice.Id) || _resolutionStackOperations._resolutionStack.Count < 3 || _resolutionStackOperations._resolutionStack[^2] is not ProgramJudgmentTriggerWindowFrame w || !IsExactBoundaryXiaoQiaoBeforeResultProgramChild(w, f) || f.WindowContext?.Judgment is not { Suit: Suit.Heart } j || _coreJudgmentCursorRuntimeOperations.Value.ActiveJudgment is not { } pending || pending.Id != j.JudgmentFrameId || _coreNativeDamageHandlesOperations.Value.GetJudgmentCard(pending)?.Id != j.CardId || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
            throw new InvalidOperationException("Mobile Hongyan choice must belong to the exact living source and unchanged native judgment.");
        var r = new BoundaryXiaoQiaoJudgmentOverride(f.Id, pending.Id, new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, j.CardId, Enum.Parse<Suit>(choice.Parameters["suit"]));
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeFrame(pending.Id, pending with { BoundaryXiaoQiaoOverride = r, Suit = r.Suit });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryXiaoQiaoJudgmentRewrittenEvent(r));
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramAiChoice(SkillProgramEffectOp.RewriteBoundaryXiaoQiaoJudgment)]
    internal static PromptChoice BoundaryXiaoQiaoJudgmentAi(ProgramSkillFrame f) => BoundaryXiaoQiaoJudgmentChoices(f).First();
}
