using System.Text.Json.Serialization;

namespace CardGame.Core;

internal interface IWenheActivityProgramHost
{
    SkillProgramStepOutcome ExecuteWenheActivity(ProgramSkillFrame frame, SkillProgramEffectOp op);
}
internal abstract class WenheActivityOperationDescriptor : ProgramOperationDescriptorBase
{
    public override ISkillProgramEffectHandler Handler => new WenheActivityEffectHandler(Op);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards, static (_, _) => { });
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [];
}
internal sealed class ResolveWenheRoundStartDescriptor : WenheActivityOperationDescriptor
{ public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveWenheRoundStart;
  public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new RequireTriggerWindow(SkillProgramTriggerWindow.RoundStarting)]; }
internal sealed class ResolveWenheTurnStartDescriptor : WenheActivityOperationDescriptor
{ public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveWenheTurnStart;
  public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new RequireTriggerWindow(SkillProgramTriggerWindow.TurnStartBeforeNormalFlow)]; }
internal sealed class ResolveWenheTurnEndDescriptor : WenheActivityOperationDescriptor
{ public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveWenheTurnEnd;
  public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new RequireTriggerWindow(SkillProgramTriggerWindow.TurnEnding)]; }
internal sealed class ResolveWenheRoundEndDescriptor : WenheActivityOperationDescriptor
{ public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveWenheRoundEnd;
  public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new RequireTriggerWindow(SkillProgramTriggerWindow.RoundEnding)]; }
internal sealed class RewardWenheKillerDescriptor : WenheActivityOperationDescriptor
{ public override SkillProgramEffectOp Op => SkillProgramEffectOp.RewardWenheKiller;
  public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new RequireTriggerWindow(SkillProgramTriggerWindow.CharacterDied)]; }
internal sealed class UseWenheChampionMarkDescriptor : WenheActivityOperationDescriptor
{ public override SkillProgramEffectOp Op => SkillProgramEffectOp.UseWenheChampionMark; }
internal sealed class WenheActivityEffectHandler(SkillProgramEffectOp op) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => op;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e, ProgramSkillFrame f, int target, ISkillProgramEffectHost h) =>
        h.GetCapability<IWenheActivityProgramHost>().ExecuteWenheActivity(f, op);
}
public sealed record WenheRuleFlowReceipt(SkillProgramEffectOp Operation, int RoundNumber, int TurnNumber,
    int Amount, int Index, int Stage)
{
    private readonly IReadOnlyList<int> _seats = Array.Empty<int>();
    public IReadOnlyList<int> Seats { get => _seats; init => _seats = Array.AsReadOnly(value.ToArray()); }
}
public sealed record WenheRuleFlowIssuedEvent(long ProgramFrameId, SkillProgramEffectOp Operation, int RoundNumber,
    int TurnNumber, int OwnerSeat, int Count, int Amount) : IGameEvent;
public sealed record WenheRuleParticipantEvent(long ProgramFrameId, int Index, int Seat) : IGameEvent;
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public WenheRuleFlowReceipt? WenheRuleFlow { get; init; }
}

public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class WenheActivityProgramHost(GameEngine engine) : IWenheActivityProgramHost
    { public SkillProgramStepOutcome ExecuteWenheActivity(ProgramSkillFrame frame, SkillProgramEffectOp op) => engine.ExecuteWenheActivity(frame, op); }

    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeOwnerFacts, 41000)]
    private bool AllowsWenheActivation(ProgramActivationLegalityContext gate) =>
        WenheRules?.AllowsActivation(gate.SkillId, gate.OwnerSeat) ??
            gate.SkillId is not ("wenhe:chaos-resolution" or "wenhe:champion-mark");
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 41000)]
    private bool AllowsWenheRuleTrigger(ProgramTriggerLegalityContext gate) =>
        WenheRules?.AllowsTrigger(new(gate.SkillId, gate.Candidate.BindingId, gate.OwnerSeat,
            gate.WindowContext?.Window, gate.WindowContext?.SourceSeat, gate.WindowContext?.TargetSeat)) ?? gate.SkillId != "wenhe:rules";
    private bool HasWenheDeadOwnerEnding(int actualOwner) => WenheRules?.HasDeadOwnerEnding(actualOwner) ?? false;
    private bool IsWenheDeadOwnerEndingCandidate(ProgramTriggerCandidate candidate, int actualOwner) =>
        WenheRules?.IsDeadOwnerEndingCandidate(candidate.SkillId, candidate.BindingId, candidate.OwnerSeat, actualOwner) ?? false;
    private SkillProgramStepOutcome ExecuteWenheActivity(ProgramSkillFrame frame, SkillProgramEffectOp op)
    {
        if (frame.WenheRuleFlow is not null || ProgramEventHistory<WenheRuleFlowIssuedEvent>().Any(fact => fact.ProgramFrameId == frame.Id))
            throw new InvalidOperationException("Wenhe requires its exact once-issued activity operation.");
        var plan = (WenheRules ?? throw new InvalidOperationException("The activity rule feature is not installed."))
            .IssueFlow(frame.Id, op, frame.OwnerSeat);
        ReplaceRuntimeTop(frame = frame with { WenheRuleFlow = new(op, _roundNumber, _turnNumber, plan.Amount, 0, 0) { Seats = plan.Seats } });
        AdvanceEventRulesAndQueueFact(new WenheRuleFlowIssuedEvent(frame.Id, op, _roundNumber, _turnNumber, frame.OwnerSeat, plan.Seats.Count, plan.Amount));
        for (var index = 0; index < plan.Seats.Count; index++) AdvanceEventRulesAndQueueFact(new WenheRuleParticipantEvent(frame.Id, index, plan.Seats[index]));
        if (plan.BeginsChaos)
        {
            ReplaceRuntimeTop(frame = frame with { WenheRuleFlow = frame.WenheRuleFlow! with { Stage = 3 } });
            BeginWenheChaos(frame, null);
            return SkillProgramStepOutcome.AwaitChild;
        }
        return ResumeWenheRuleFlow(frame.Id) ? SkillProgramStepOutcome.AwaitChild : SkillProgramStepOutcome.Continue;
    }
    [ProgramContinuation(ProgramContinuationKind.Resume, 41000)]
    private bool ResumeWenheRuleFlow(long id) => WenheRules?.ResumeFlow(id) ?? false;
    private bool TryBeginWenheRuleChildren(long id) => TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.Program) ||
        TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program) ||
        TryBeginCardsMovedProgramWindow(id) || TryBeginAdvancedSkillsChanged(id);
    [ProgramNativeDrawReturn(SkillProgramEffectOp.RewardWenheKiller, SkillProgramEffectOp.ResolveWenheTurnStart, SkillProgramEffectOp.UseWenheChampionMark)]
    private void ReturnWenheRuleDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        AssertWenheRuleFlow(f);
        var r = f.WenheRuleFlow!;
        var count = (WenheRules ?? throw new InvalidOperationException("The activity rule feature is not installed.")).DrawCount(r.Operation, r.Amount);
        if (r.Stage != 2 || outcome.RecipientSeat != r.Seats[r.Index] || outcome.RequestedCount != count)
            throw new InvalidOperationException("Wenhe draw lost its already paid participant.");
        CompleteProgramDrawCards(f.Id, outcome.RecipientSeat, null, SkillProgramCardSetVisibility.Private, new("wenhe.rule.draw"), 0, outcome.ActualDrawIds);
        AdvanceRuntimeProgram(f.Id);
    }
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 41000)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 41000)]
    private bool RetainWenheIssuedFlow(ProgramSkillFrame f) => f.WenheRuleFlow is not null || f.WenheChaos is not null && IsExactWenheChaos(f);
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 41000)]
    private void AssertWenheRuleFlow(ProgramSkillFrame f)
    {
        if (f.WenheRuleFlow is not { } r) return;
        if (_modeDefinition.WenheActivity is null || r.RoundNumber != _roundNumber || r.TurnNumber != _turnNumber ||
            r.Index < 0 || r.Index > r.Seats.Count || r.Stage is < 0 or > 3 || r.Seats is not System.Collections.IList { IsReadOnly: true } ||
            ProgramEventHistory<WenheRuleFlowIssuedEvent>().Count(e => e.ProgramFrameId == f.Id && e.Operation == r.Operation &&
                e.RoundNumber == r.RoundNumber && e.TurnNumber == r.TurnNumber && e.Count == r.Seats.Count && e.Amount == r.Amount && e.OwnerSeat == f.OwnerSeat) != 1 ||
            r.Seats.Where((seat, index) => ProgramEventHistory<WenheRuleParticipantEvent>().Count(e => e.ProgramFrameId == f.Id && e.Index == index && e.Seat == seat) != 1).Any())
            throw new InvalidOperationException("Wenhe rule lost its frozen participants, paid cursor or originating fact.");
    }
}
