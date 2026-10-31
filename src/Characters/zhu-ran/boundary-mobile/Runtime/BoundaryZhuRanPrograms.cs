using System.Text.Json.Serialization;

namespace CardGame.Core;

internal static class BoundaryZhuRanComposition
{
    internal const string Skill = "boundary:danshou-mobile",Draw = "untargeted-ending-draw",Pay = "targeted-ending-payment";
    [ProgramCatalogValidation(53900)]
    internal static void Validate(string path, SkillProgram p)
    {
        if (!p.Triggers.SelectMany(t => t.Effects).Concat(p.Activations.SelectMany(a => a.Effects)).Any(e => e.Op == SkillProgramEffectOp.PrepareBoundaryZhuRanDanshou)) return;
        if (p.Id != Skill || p.Activations.Count != 0 || p.Triggers.Count != 2 || p.ViewAs.Count != 0 || p.Modifiers.Count != 0 || p.CardPolicies.Count != 0 || p.CardIdentities.Count != 0 || p.HpSkillGrants.Count != 0 || p.DamageModifiers.Count != 0 ||
            p.Triggers.SingleOrDefault(t => t.Id == Draw) is not { Window: SkillProgramTriggerWindow.TurnEnding, Subject: SkillProgramTriggerSubject.Owner, TurnOwnerScope: SkillProgramTurnOwnerScope.OtherLiving, Optional: false,
                Effects: [{ Op: SkillProgramEffectOp.PrepareBoundaryZhuRanDanshou, Target: SkillProgramEffectTarget.Owner }, { Op: SkillProgramEffectOp.Draw, Target: SkillProgramEffectTarget.Owner, Amount: 1 }] } ||
            p.Triggers.SingleOrDefault(t => t.Id == Pay) is not { Window: SkillProgramTriggerWindow.TurnEnding, Subject: SkillProgramTriggerSubject.Owner, TurnOwnerScope: SkillProgramTurnOwnerScope.OtherLiving, Optional: true,
                Effects: [{ Op: SkillProgramEffectOp.PrepareBoundaryZhuRanDanshou, Target: SkillProgramEffectTarget.Owner }, { Op: SkillProgramEffectOp.Damage, Target: SkillProgramEffectTarget.Owner, TargetReference.Kind: ProgramParticipantRef.EventTarget, Amount: 1 }] } ||
            p.Triggers.Any(t => t.Priority != 0 || t.UsageScope is not null || t.UsageLimit is not null || t.MarkerCost is not null || t.Condition.Kind != SkillProgramTriggerConditionKind.Always ||
                t.Effects.Any(e => e.Condition.Kind != SkillProgramConditionKind.Always)))
            throw new InvalidOperationException(path + ": mobile Danshou requires one mandatory no-target draw or one exact optional whole-HE payment followed by native damage at another role's actual Ending.");
    }
}
internal interface IBoundaryZhuRanHost { SkillProgramStepOutcome Prepare(ProgramSkillFrame f); }
internal sealed class PrepareBoundaryZhuRanDanshouDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.PrepareBoundaryZhuRanDanshou;
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override ISkillProgramEffectHandler Handler { get; } = new BoundaryZhuRanHandler();
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards, static (_, c) => c.PublicControlValue(2));
    public override ProgramFactRequirements FactRequirements => new(Triggered: ProgramTrackedFact.WholeActualUseDeclaration | ProgramTrackedFact.NativeLogicalDiscard | ProgramTrackedFact.ActualDiscardRecoveryPhase);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new RequireTriggerWindow(SkillProgramTriggerWindow.TurnEnding)];
    public override bool AllowsDeadTarget(in ProgramEffectContinuationContext context) => context.Frame.BoundaryDanshou is { Paid: true };
}
internal sealed class BoundaryZhuRanHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.PrepareBoundaryZhuRanDanshou;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e, ProgramSkillFrame f, int target, ISkillProgramEffectHost h) => h.GetCapability<IBoundaryZhuRanHost>().Prepare(f);
}
public enum BoundaryDanshouStage { Selecting, PaidChildren, Complete }
public sealed record BoundaryDanshouUseActorEvent(long UseId, long? ActionId, int Turn, int TurnOwner, int Actor, int Provider) : IGameEvent;
public sealed record BoundaryDanshouUseTargetEvent(long UseId, int Turn, int TurnOwner, int Target) : IGameEvent;
public sealed record BoundaryDanshouDirectDuelTargetEvent(long ProducerFrameId, int InstructionIndex, int Turn, int TurnOwner, int Actor, int Target) : IGameEvent;
public sealed record BoundaryDanshouStartedEvent(long FrameId, CardConversionSource Source, string Hash, long EndingId, int Turn, int TurnOwner, int Actor, long Cutoff, int X) : IGameEvent;
public sealed record BoundaryDanshouPaidEvent(long FrameId, int X, long? DiscardInvocationId, int Actual, long Before, long After) : IGameEvent;
public sealed record BoundaryDanshouCompletedEvent(long FrameId, int Actual, bool DamageAuthorized) : IGameEvent;
public sealed record BoundaryDanshouReceipt(CardConversionSource Source, string Hash, long EndingId, int Turn, int TurnOwner, int Actor, long Cutoff, int X, BoundaryDanshouStage Stage)
{
    private readonly IReadOnlyList<int> _selected = [];
    public IReadOnlyList<int> Selected { get => _selected; init => _selected = Array.AsReadOnly(value.ToArray()); }
    public bool Paid { get; init; }
    public long Before { get; init; }
    public long After { get; init; }
    public long? DiscardInvocationId { get; init; }
    public int Actual { get; init; }
}
public sealed partial record ProgramSkillFrame
{ [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public BoundaryDanshouReceipt? BoundaryDanshou { get; init; } }
