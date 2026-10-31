using System.Text.Json.Serialization;
namespace CardGame.Core;

internal static class BoundaryZhangNiComposition
{
    internal const string Skill = "boundary:wurong-mobile",Activation = "private-mouyi",Skip = "issued-next-draw-skip",Result = "wurong-result";
    internal static readonly string[] Outcomes = ["suppress-resist", "suppress-submit", "appease-resist", "appease-submit"];
    [ProgramCatalogValidation(54200)]
    internal static void Validate(string path, SkillProgram p)
    {
        if (!p.Activations.SelectMany(a => a.Effects).Concat(p.Triggers.SelectMany(t => t.Effects)).Any(e => e.Op is SkillProgramEffectOp.ResolveBoundaryZhangNiWurong or SkillProgramEffectOp.ConsumeBoundaryZhangNiDrawSkip)) return;
        if (p.Id != Skill || p.Activations is not [{ Id: Activation, MinCards: 0, MaxCards: 0, MinTargets: 1, MaxTargets: 1, TargetKind: SkillProgramTargetKind.OtherLiving, UsesPerTurn: null, UsesPerPhase: 1 } a] ||
            a.Effects is not [{ Op: SkillProgramEffectOp.ResolveBoundaryZhangNiWurong, StateId: "strategies", ResultBind: Result },
                { Op: SkillProgramEffectOp.Damage, Target: SkillProgramEffectTarget.SelectedTarget, Amount: 1 }, { Op: SkillProgramEffectOp.Draw, Target: SkillProgramEffectTarget.Owner, Amount: 1 },
                { Op: SkillProgramEffectOp.SelectAndMoveOwnedCard, ChooserRef.Kind: ProgramParticipantRef.Owner, CardOwnerRef.Kind: ProgramParticipantRef.SelectedTarget, Amount: 1, Destination: SkillProgramCardDestination.OwnerHand, SkipIfNoCards: true, AwaitMovementTriggers: true },
                { Op: SkillProgramEffectOp.ResolveBoundaryZhangNiWurong, StateId: "gifts" },
                { Op: SkillProgramEffectOp.ReceiveOwnerDamage, Target: SkillProgramEffectTarget.Owner, Amount: 1 }, { Op: SkillProgramEffectOp.Draw, Target: SkillProgramEffectTarget.Owner, Amount: 1 }] ||
            !a.Effects[3].Zones.SequenceEqual([CardZoneKind.Hand, CardZoneKind.Equipment]) ||
            !Branch(a.Effects[1], Outcomes[0]) || !Branch(a.Effects[2], Outcomes[0]) || !Branch(a.Effects[3], Outcomes[1]) || !Branch(a.Effects[5], Outcomes[2]) || !Branch(a.Effects[6], Outcomes[2]) ||
            a.Effects[0].Condition.Kind != SkillProgramConditionKind.Always || a.Effects[4].Condition.Kind != SkillProgramConditionKind.Always ||
            p.Triggers is not [{ Id: Skip, Window: SkillProgramTriggerWindow.DrawPhaseStarting, Subject: SkillProgramTriggerSubject.Owner, Optional: false, IsSkillInvocation: false, Priority: 1000,
                Effects: [{ Op: SkillProgramEffectOp.ConsumeBoundaryZhangNiDrawSkip, Target: SkillProgramEffectTarget.Owner, Condition.Kind: SkillProgramConditionKind.Always } ] }])
            throw new InvalidOperationException(path + ": mobile Wurong requires its exact private four-way strategy, native consequences and issued next actual Draw skip.");
    }
    private static bool Branch(SkillProgramEffect e, string value) => e.Condition is { Kind: SkillProgramConditionKind.ChoiceIs, SourceBind: Result } && e.Condition.OptionId == value;
}
internal interface IBoundaryZhangNiHost { SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffect e); }
internal sealed class ResolveBoundaryZhangNiWurongDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveBoundaryZhangNiWurong;
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "stateId", "resultBind", "condition"); var stage = r.RequiredIdentifier("stateId"); var bind = r.OptionalIdentifier("resultBind");
        if (stage is not ("strategies" or "gifts") || (stage == "strategies") != (bind == BoundaryZhangNiComposition.Result)) throw new InvalidOperationException(r.Path + ": requires strategies/result or gifts without result.");
        var e = new SkillProgramEffect(Op, FilterBoundCardsProgramOperationDescriptor.Owner(r), 0, r.Condition(), stateId: stage, resultBind: bind); RequireAlways(e, r.Path); return e;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => e.StateId == "strategies" ? [new ReadSelectedTarget(), new CreateChoiceResult(e.ResultBind!, BoundaryZhangNiComposition.Outcomes)] : [new ReadSelectedTarget()];
    public override ProgramFactRequirements FactRequirements => new(Activated: ProgramTrackedFact.ActualDiscardRecoveryPhase);
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override ISkillProgramEffectHandler Handler { get; } = new BoundaryZhangNiHandler();
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.ChooseOption, static (_, c) => c.PublicControlValue(4));
}
internal sealed class ConsumeBoundaryZhangNiDrawSkipDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ConsumeBoundaryZhangNiDrawSkip;
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new RequireTriggerWindow(SkillProgramTriggerWindow.DrawPhaseStarting)];
    public override ISkillProgramEffectHandler Handler { get; } = new BoundaryZhangNiHandler(SkillProgramEffectOp.ConsumeBoundaryZhangNiDrawSkip);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.ChooseOption, static (_, _) => { });
}
internal sealed class BoundaryZhangNiHandler(SkillProgramEffectOp op = SkillProgramEffectOp.ResolveBoundaryZhangNiWurong) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => op;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e, ProgramSkillFrame f, int seat, ISkillProgramEffectHost h) => h.GetCapability<IBoundaryZhangNiHost>().Resolve(f, e);
}
public enum BoundaryWurongStage { OwnerChoice, TargetChoice, Revealed, GiftChoice, GiftChildren, Complete }
public sealed record BoundaryWurongReceipt(CardConversionSource Source, string Hash, ActualDiscardRecoveryPhaseKey Phase, int Target)
{
    public BoundaryWurongStage Stage { get; init; }
    public bool? Suppress { get; init; }
    public bool? Resist { get; init; }
    public string? Outcome { get; init; }
    public int Giver { get; init; }
    public int Recipient { get; init; }
    public int Required { get; init; }
    private readonly IReadOnlyList<int> _selected = [];
    public IReadOnlyList<int> Selected { get => _selected; init => _selected = Array.AsReadOnly(value.ToArray()); }
    public long Before { get; init; }
    public long After { get; init; }
}
public sealed partial record ProgramSkillFrame
{ [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public BoundaryWurongReceipt? BoundaryWurong { get; init; } }
public sealed record BoundaryWurongStartedEvent(long FrameId, CardConversionSource Source, string Hash, ActualDiscardRecoveryPhaseKey Phase, int Target) : IGameEvent;
public sealed record BoundaryWurongCommittedEvent(long FrameId, int Participant, bool First) : IGameEvent;
public sealed record BoundaryWurongRevealedEvent(long FrameId, bool Suppress, bool Resist, string Outcome) : IGameEvent;
public sealed record BoundaryWurongGiftPaidEvent(long FrameId, int Giver, int Recipient, int Count, long Before, long After) : IGameEvent;
public sealed record BoundaryWurongGiftEntityEvent(long FrameId, int CardId, CardLocation From, CardLocation To, long Sequence) : IGameEvent;
public sealed record BoundaryWurongSkipIssuedEvent(long FrameId, CardConversionSource Source, string Hash, int Target) : IGameEvent;
public sealed record BoundaryWurongSkipConsumedEvent(long IssuerFrameId, long ConsumerFrameId, long DrawWindowId, int Target, int ActualTurn) : IGameEvent;
