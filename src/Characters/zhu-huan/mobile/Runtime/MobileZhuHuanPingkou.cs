using System.Text.Json.Serialization;

namespace CardGame.Core;

internal interface IMobileZhuHuanPingkouHost { SkillProgramStepOutcome Begin(ProgramSkillFrame frame); }
internal sealed class ResolveMobileZhuHuanPingkouDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveMobileZhuHuanPingkou;
    public override ISkillProgramEffectHandler Handler { get; } = new MobileZhuHuanPingkouHandler();
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override ProgramOperationAiPolicy AiPolicy => new(ProgramOperationAiSemantic.SelectTarget, static (_, c) => c.PublicControlValue(3));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [new RequireTriggerWindow(SkillProgramTriggerWindow.AfterTurnEnded)];
}
internal sealed class MobileZhuHuanPingkouHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveMobileZhuHuanPingkou;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int target, ISkillProgramEffectHost host) => host.GetCapability<IMobileZhuHuanPingkouHost>().Begin(frame);
}
public sealed record MobileZhuHuanPingkouReceipt(CardConversionSource Source, string GameplayHash, int ActualTurn, long EndedFrame, int Maximum, bool Sealed = false, int Index = 0)
{
    private readonly IReadOnlyList<int> _targets = Array.AsReadOnly(Array.Empty<int>());
    public IReadOnlyList<int> Targets { get => _targets; init => _targets = Array.AsReadOnly(value.ToArray()); }
}
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public MobileZhuHuanPingkouReceipt? MobileZhuHuanPingkou { get; init; }
}
public sealed record MobileZhuHuanPingkouStartedEvent(long FrameId, int OwnerSeat, string SkillInstanceId, string GameplayHash, int ActualTurn, long EndedFrame, int Maximum) : IGameEvent;
public sealed record MobileZhuHuanPingkouTargetEvent(long FrameId, int Target) : IGameEvent;
public sealed record MobileZhuHuanPingkouSealedEvent(long FrameId, int Count) : IGameEvent;
public sealed record MobileZhuHuanPingkouDamageIssuedEvent(long FrameId, int Index, int Target) : IGameEvent;
public sealed record MobileZhuHuanPingkouDamageReturnedEvent(long FrameId, int Index, int Target) : IGameEvent;
public sealed record MobileZhuHuanPingkouCompletedEvent(long FrameId) : IGameEvent;

public sealed partial class GameEngine
{
    private const string MobileZhuHuanPingkouSkill = "mobile:pingkou", MobileZhuHuanPingkouAction = "mobile-zhu-huan-pingkou";
    private static CardConversionSource MobileZhuHuanPingkouSource(ProgramSkillFrame f) => new(f.SkillId, GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    [ProgramCapability]
    private sealed class MobileZhuHuanPingkouHost(GameEngine engine) : IMobileZhuHuanPingkouHost
    { public SkillProgramStepOutcome Begin(ProgramSkillFrame frame) => engine.BeginMobileZhuHuanPingkou(frame); }
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 72500)]
    private bool CanOfferMobileZhuHuanPingkou(ProgramTriggerLegalityContext gate) => gate.SkillId != MobileZhuHuanPingkouSkill ||
        gate.Owner.Seat == _currentSeat && BoundaryPingkouMaximum(gate.Owner.Seat) > 0 && _players.Any(p => p.IsAlive && p.Seat != gate.Owner.Seat);
    private SkillProgramStepOutcome BeginMobileZhuHuanPingkou(ProgramSkillFrame supplied)
    {
        var f = GetActiveProgramFrame(supplied.Id);
        if (f.SkillId != MobileZhuHuanPingkouSkill || f.TriggerId != "actual-ended-skips" || f.InstructionIndex != 1 || f.MobileZhuHuanPingkou is not null ||
            f.WindowContext is not { Window: SkillProgramTriggerWindow.AfterTurnEnded } context || f.OwnerSeat != _currentSeat ||
            !_resolutionStack.OfType<DeferredTurnEndFrame>().Any(p => p.Id == context.ParentFrameId && p.OwnerSeat == f.OwnerSeat && p.TurnNumber == _turnNumber) || BoundaryPingkouMaximum(f.OwnerSeat) < 1)
            throw new InvalidOperationException("Mobile Pingkou starts on its owner's genuine ended turn and actual native skipped-phase history.");
        var r = new MobileZhuHuanPingkouReceipt(MobileZhuHuanPingkouSource(f), f.GameplayHash, _turnNumber, context.ParentFrameId, BoundaryPingkouMaximum(f.OwnerSeat));
        ReplaceRuntimeTop(f = f with { MobileZhuHuanPingkou = r });
        AdvanceEventRulesAndQueueFact(new MobileZhuHuanPingkouStartedEvent(f.Id, f.OwnerSeat, f.SkillInstanceId, f.GameplayHash, r.ActualTurn, r.EndedFrame, r.Maximum));
        PublishMobileZhuHuanPingkou(f); return SkillProgramStepOutcome.AwaitChoice;
    }
    private IReadOnlyList<PromptChoice> MobileZhuHuanPingkouChoices(ProgramSkillFrame f)
    {
        var r = f.MobileZhuHuanPingkou!; var choices = new List<PromptChoice>();
        if (r.Sealed) return [];
        if (r.Targets.Count < r.Maximum)
            choices.AddRange(_players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat && !r.Targets.Contains(p.Seat)).Select(p =>
                new PromptChoice(new($"mobile-pingkou.{f.Id}.target-{p.Seat}"), "选择 " + p.Name, [], [p.Seat], new Dictionary<string, string> { ["program-action"] = MobileZhuHuanPingkouAction, ["branch"] = "target" })));
        choices.Add(new(new($"mobile-pingkou.{f.Id}.finish"), $"依次造成伤害（已选{r.Targets.Count}/{r.Maximum}）", [], [], new Dictionary<string, string> { ["program-action"] = MobileZhuHuanPingkouAction, ["branch"] = "finish" }));
        return Array.AsReadOnly(choices.ToArray());
    }
    private void PublishMobileZhuHuanPingkou(ProgramSkillFrame f)
    {
        var choices = MobileZhuHuanPingkouChoices(f); var skill = _contentRegistry.GetSkill(f.SkillId);
        ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, $"平寇：选择至多{f.MobileZhuHuanPingkou!.Maximum}名其他角色。", [], choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        { PromptId = CreatePromptId(), Choices = choices, IsPrivate = true, SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description) };
        _status = _players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running; AdvanceRulesAndPublishState();
    }
    [ProgramAnswer(MobileZhuHuanPingkouAction)]
    private void AnswerMobileZhuHuanPingkou(PromptChoice chosen)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { MobileZhuHuanPingkou: { Sealed: false } r } f || !ValidMobileZhuHuanPingkou(f) ||
            _pendingDecision?.PlayerSeat != f.OwnerSeat || !AssistedChoicesEqual([chosen], MobileZhuHuanPingkouChoices(f).Where(c => c.Id == chosen.Id).ToArray()))
            throw new InvalidOperationException("Mobile Pingkou requires its exact original owner and a current distinct living target.");
        ClearPendingDecision(); ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        if (chosen.Parameters["branch"] == "target")
        {
            var target = chosen.Targets.Single(); ReplaceRuntimeTop(f = f with { MobileZhuHuanPingkou = r with { Targets = r.Targets.Append(target).ToArray() } });
            AdvanceEventRulesAndQueueFact(new MobileZhuHuanPingkouTargetEvent(f.Id, target)); PublishMobileZhuHuanPingkou(f); return;
        }
        ReplaceRuntimeTop(f = f with { MobileZhuHuanPingkou = r with { Sealed = true, Targets = r.Targets.OrderBy(seat => RelativeTableOrder(f.OwnerSeat, seat)).ToArray() } });
        AdvanceEventRulesAndQueueFact(new MobileZhuHuanPingkouSealedEvent(f.Id, r.Targets.Count)); IssueMobileZhuHuanPingkouDamage(f);
    }
    private void IssueMobileZhuHuanPingkouDamage(ProgramSkillFrame f)
    {
        var r = f.MobileZhuHuanPingkou!;
        while (r.Index < r.Targets.Count && !_players[r.Targets[r.Index]].IsAlive) r = r with { Index = r.Index + 1 };
        ReplaceRuntimeTop(f = f with { MobileZhuHuanPingkou = r });
        if (r.Index >= r.Targets.Count)
        {
            AdvanceEventRulesAndQueueFact(new MobileZhuHuanPingkouCompletedEvent(f.Id));
            ReplaceRuntimeTop(f with { MobileZhuHuanPingkou = null }); AdvanceRuntimeProgram(f.Id); return;
        }
        AdvanceEventRulesAndQueueFact(new MobileZhuHuanPingkouDamageIssuedEvent(f.Id, r.Index, r.Targets[r.Index]));
        BeginProgramSkillDamage(f, r.Targets[r.Index], 1);
    }
    [ProgramContinuation(ProgramContinuationKind.Resume, 72500)]
    private bool ResumeMobileZhuHuanPingkou(long id)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { MobileZhuHuanPingkou: { } r } f || f.Id != id) return false;
        AssertMobileZhuHuanPingkou(f);
        if (f.AttackAttempt is not null) return true;
        if (!r.Sealed) { if (_pendingDecision is null) PublishMobileZhuHuanPingkou(f); return true; }
        if (TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) ||
            TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || TryBeginCardsMovedProgramWindow(id) || TryBeginAdvancedSkillsChanged(id)) return true;
        f = GetActiveProgramFrame(id); r = f.MobileZhuHuanPingkou!;
        if (r.Index >= r.Targets.Count || ProgramEventHistory<MobileZhuHuanPingkouDamageIssuedEvent>().Count(e => e.FrameId == f.Id && e.Index == r.Index && e.Target == r.Targets[r.Index]) != 1 ||
            ProgramEventHistory<MobileZhuHuanPingkouDamageReturnedEvent>().Any(e => e.FrameId == f.Id && e.Index == r.Index))
            throw new InvalidOperationException("Mobile Pingkou accepts only its original native damage return once.");
        AdvanceEventRulesAndQueueFact(new MobileZhuHuanPingkouDamageReturnedEvent(f.Id, r.Index, r.Targets[r.Index]));
        ReplaceRuntimeTop(f = f with { MobileZhuHuanPingkou = r with { Index = r.Index + 1 } }); IssueMobileZhuHuanPingkouDamage(f); return true;
    }
    private bool ValidMobileZhuHuanPingkou(ProgramSkillFrame f) => f.MobileZhuHuanPingkou is { } r && f.SkillId == MobileZhuHuanPingkouSkill && f.InstructionIndex == 1 &&
        r.Source == MobileZhuHuanPingkouSource(f) && r.GameplayHash == f.GameplayHash && r.ActualTurn == _turnNumber && r.EndedFrame == f.WindowContext?.ParentFrameId &&
        f.WindowContext?.Window == SkillProgramTriggerWindow.AfterTurnEnded && _resolutionStack.OfType<DeferredTurnEndFrame>().Any(p => p.Id == r.EndedFrame && p.OwnerSeat == f.OwnerSeat && p.TurnNumber == r.ActualTurn) &&
        r.Maximum > 0 && r.Maximum == BoundaryPingkouMaximum(f.OwnerSeat) && r.Targets.Count <= r.Maximum && r.Targets.Distinct().Count() == r.Targets.Count &&
        r.Targets.All(seat => IsValidPlayerSeat(seat) && seat != f.OwnerSeat) && r.Index >= 0 && r.Index <= r.Targets.Count &&
        ProgramEventHistory<MobileZhuHuanPingkouStartedEvent>().Count(e => e == new MobileZhuHuanPingkouStartedEvent(f.Id, f.OwnerSeat, f.SkillInstanceId, f.GameplayHash, r.ActualTurn, r.EndedFrame, r.Maximum)) == 1 &&
        r.Targets.OrderBy(seat => RelativeTableOrder(f.OwnerSeat, seat)).SequenceEqual(ProgramEventHistory<MobileZhuHuanPingkouTargetEvent>().Where(e => e.FrameId == f.Id).Select(e => e.Target).OrderBy(seat => RelativeTableOrder(f.OwnerSeat, seat))) &&
        (!r.Sealed || ProgramEventHistory<MobileZhuHuanPingkouSealedEvent>().Count(e => e.FrameId == f.Id && e.Count == r.Targets.Count) == 1);
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 72500)]
    private void AssertMobileZhuHuanPingkou(ProgramSkillFrame f)
    {
        if (f.MobileZhuHuanPingkou is not { } r) return;
        if (!ValidMobileZhuHuanPingkou(f) || f.AttackAttempt is not null && (!r.Sealed || r.Index >= r.Targets.Count || f.AttackAttempt.TargetSeat != r.Targets[r.Index] ||
            ProgramEventHistory<MobileZhuHuanPingkouDamageIssuedEvent>().Count(e => e.FrameId == f.Id && e.Index == r.Index && e.Target == r.Targets[r.Index]) != 1))
            throw new InvalidOperationException("Mobile Pingkou lost its actual-turn quota, original targets or issued native damage.");
    }
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 72500)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 72500)]
    private bool KeepMobileZhuHuanPingkouIssued(ProgramSkillFrame f) => f.MobileZhuHuanPingkou is { Sealed: true } && ValidMobileZhuHuanPingkou(f);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveMobileZhuHuanPingkou)]
    private PromptChoice MobileZhuHuanPingkouAi(PendingDecision p, ProgramSkillFrame f) => p.Choices.FirstOrDefault(c => c.Targets is [var target] &&
        !AreProgramDistributionAllies(_players[f.OwnerSeat], _players[target])) ?? p.Choices.Single(c => c.Parameters["branch"] == "finish");
}
