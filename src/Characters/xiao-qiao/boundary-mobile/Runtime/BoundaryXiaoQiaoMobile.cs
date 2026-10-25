using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record BoundaryXiaoQiaoJudgmentOverride(long ProgramFrameId, long JudgmentFrameId,
    CardConversionSource Source, string GameplayHash, int CardId, Suit Suit);
public sealed record BoundaryXiaoQiaoJudgmentRewrittenEvent(BoundaryXiaoQiaoJudgmentOverride Receipt) : IGameEvent;
public sealed partial record JudgmentFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BoundaryXiaoQiaoJudgmentOverride? BoundaryXiaoQiaoOverride { get; init; }
}

internal interface IBoundaryXiaoQiaoMobileHost { SkillProgramStepOutcome Rewrite(ProgramSkillFrame frame); }
internal sealed class RewriteBoundaryXiaoQiaoJudgmentDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.RewriteBoundaryXiaoQiaoJudgment;
    public override ISkillProgramEffectHandler Handler { get; } = new BoundaryXiaoQiaoJudgmentHandler();
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.ReplaceJudgment, static (_, c) => c.PublicControlValue(1));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new RequireTriggerWindow(SkillProgramTriggerWindow.JudgmentBeforeResult)];
}
internal sealed class BoundaryXiaoQiaoJudgmentHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.RewriteBoundaryXiaoQiaoJudgment;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e, ProgramSkillFrame f, int target, ISkillProgramEffectHost host) => host.GetCapability<IBoundaryXiaoQiaoMobileHost>().Rewrite(f);
}

public sealed partial class GameEngine
{
    private const string BoundaryXiaoQiaoHongyan = "boundary:hongyan-mobile";
    private Suit? BoundaryXiaoQiaoEffectiveSuit(CharacterState owner, Card card, CardLocation? identityLocation)
    {
        if (card.Suit != Suit.Spade || !EnabledSkillPrograms(owner).Any(p => p.Id == BoundaryXiaoQiaoHongyan)) return null;
        var location = identityLocation ?? _cardZones.GetLocation(card.Id);
        return location == CardLocation.Hand(owner.Seat) ||
            _resolutionStack.OfType<JudgmentFrame>().Any(j => j.TargetSeat == owner.Seat && j.CardId == card.Id && location == CardLocation.Processing)
            ? Suit.Heart : null;
    }
    private Suit EffectiveBoundaryXiaoQiaoJudgmentSuit(JudgmentFrame frame, Suit fallback)
    {
        if (frame.BoundaryXiaoQiaoOverride is not { } r) return fallback;
        if (r.JudgmentFrameId != frame.Id || r.CardId != frame.CardId || r.Source.SkillId != BoundaryXiaoQiaoHongyan ||
            r.Suit is not (Suit.Spade or Suit.Heart or Suit.Club or Suit.Diamond) ||
            r.GameplayHash != _contentRegistry.GetSkill(r.Source.SkillId).Program?.GameplayHash ||
            ProgramEventHistory<BoundaryXiaoQiaoJudgmentRewrittenEvent>().Count(e => e.Receipt == r) != 1)
            throw new InvalidOperationException("Mobile Hongyan lost its exact original judgment card and committed result.");
        // These before-result children are issued in native frame order. A later
        // result rewrite must supersede this one on the same judgment.
        return frame.ZhenyiOverride is { } zhenyi && zhenyi.ProgramFrameId > r.ProgramFrameId ||
            frame.HellDayuanOverride is { } dayuan && dayuan.ProgramFrameId > r.ProgramFrameId ? fallback : r.Suit;
    }
    private bool IsExactBoundaryXiaoQiaoBeforeResultProgramChild(ProgramJudgmentTriggerWindowFrame w, ProgramSkillFrame f) =>
        f.SkillId == BoundaryXiaoQiaoHongyan && w.ZhenyiBeforeResult == true && w.Activated &&
        f.WindowContext is { Window: SkillProgramTriggerWindow.JudgmentBeforeResult, Judgment: { } j } c && c.ParentFrameId == w.Id && j == w.Judgment &&
        _resolutionStack.OfType<JudgmentFrame>().SingleOrDefault(p => p.Id == w.ParentFrameId) is { ZhenyiBeforeResultCompleted: not true } pending &&
        pending.Id == j.JudgmentFrameId && GetJudgmentCard(pending)?.Id == j.CardId &&
        w.CandidateIndex >= 0 && w.CandidateIndex < w.Candidates.Count && w.Candidates[w.CandidateIndex] is { } b &&
        b.OwnerSeat == f.OwnerSeat && b.SkillId == f.SkillId && b.TriggerId == f.TriggerId && b.SkillInstanceId == f.SkillInstanceId && b.GameplayHash == f.GameplayHash;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 66700)]
    private bool QualifyBoundaryXiaoQiaoMobile(ProgramTriggerCandidate b, ProgramSkillWindowContext c) => b.SkillId != BoundaryXiaoQiaoHongyan ||
        c is { Window: SkillProgramTriggerWindow.JudgmentBeforeResult, Judgment.Suit: Suit.Heart };
    [ProgramCapability]
    private sealed class BoundaryXiaoQiaoMobileHost(GameEngine engine) : IBoundaryXiaoQiaoMobileHost
    { public SkillProgramStepOutcome Rewrite(ProgramSkillFrame f) => engine.BeginBoundaryXiaoQiaoJudgment(f); }
    private SkillProgramStepOutcome BeginBoundaryXiaoQiaoJudgment(ProgramSkillFrame f)
    {
        if (f.WindowContext?.Judgment?.Suit != Suit.Heart) throw new InvalidOperationException("Mobile Hongyan requires the current heart result before it takes effect.");
        var skill = _contentRegistry.GetSkill(f.SkillId);
        _pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "红颜：指定本次判定结果的花色。", [], [], f.OwnerSeat)
        { PromptId = CreatePromptId(), Choices = BoundaryXiaoQiaoJudgmentChoices(f), SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description) };
        _status = _players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        AdvanceRulesAndPublishState(); return SkillProgramStepOutcome.AwaitChoice;
    }
    private static IReadOnlyList<PromptChoice> BoundaryXiaoQiaoJudgmentChoices(ProgramSkillFrame f) => Array.AsReadOnly(new[] { Suit.Heart, Suit.Spade, Suit.Club, Suit.Diamond }
        .Select(s => new PromptChoice(new($"mobile-hongyan.{f.Id}.{s}"), s switch { Suit.Heart => "红桃", Suit.Spade => "黑桃", Suit.Club => "梅花", _ => "方块" }, [], [],
            new Dictionary<string, string> { ["program-action"] = "boundary-xiao-qiao-judgment", ["suit"] = s.ToString() })).ToArray());
    [ProgramAnswer("boundary-xiao-qiao-judgment")]
    private void AnswerBoundaryXiaoQiaoJudgment(PromptChoice choice)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { SkillId: BoundaryXiaoQiaoHongyan } f ||
            _pendingDecision?.PlayerSeat != f.OwnerSeat || !BoundaryXiaoQiaoJudgmentChoices(f).Any(c => c.Id == choice.Id) ||
            _resolutionStack.Count < 3 || _resolutionStack[^2] is not ProgramJudgmentTriggerWindowFrame w ||
            !IsExactBoundaryXiaoQiaoBeforeResultProgramChild(w, f) || f.WindowContext?.Judgment is not { Suit: Suit.Heart } j ||
            ActiveJudgment is not { } pending || pending.Id != j.JudgmentFrameId || GetJudgmentCard(pending)?.Id != j.CardId ||
            !HasRuntimeSkillInstance(_players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
            throw new InvalidOperationException("Mobile Hongyan choice must belong to the exact living source and unchanged native judgment.");
        var r = new BoundaryXiaoQiaoJudgmentOverride(f.Id, pending.Id, new(f.SkillId, GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, j.CardId, Enum.Parse<Suit>(choice.Parameters["suit"]));
        ClearPendingDecision(); ReplaceRuntimeFrame(pending.Id, pending with { BoundaryXiaoQiaoOverride = r, Suit = r.Suit });
        AdvanceEventRulesAndQueueFact(new BoundaryXiaoQiaoJudgmentRewrittenEvent(r)); FinishProgramSkill(f, true);
    }
    [ProgramAiChoice(SkillProgramEffectOp.RewriteBoundaryXiaoQiaoJudgment)]
    private static PromptChoice BoundaryXiaoQiaoJudgmentAi(ProgramSkillFrame f) => BoundaryXiaoQiaoJudgmentChoices(f).First();
}
