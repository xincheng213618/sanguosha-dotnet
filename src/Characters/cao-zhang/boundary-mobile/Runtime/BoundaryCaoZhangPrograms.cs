using System.Text.Json.Serialization;

namespace CardGame.Core;

internal static class BoundaryCaoZhangComposition
{
    internal const string Skill = "boundary:jiangchi-mobile", Cost = "jiangchi-cost";
    [ProgramCatalogValidation(55500)]
    internal static void Validate(string path, SkillProgram p)
    {
        var uses = p.Triggers.SelectMany(t => t.Effects).Concat(p.Activations.SelectMany(a => a.Effects)).Any(e => e.Op is SkillProgramEffectOp.PrepareBoundaryCaoZhangJiangchi or SkillProgramEffectOp.ArmBoundaryCaoZhangJiangchi);
        if (!uses && p.Id != Skill) return;
        if (p.Id != Skill || p.Activations.Count != 0 || p.Triggers.Count != 2 || p.Triggers.Any(t => t.Window != SkillProgramTriggerWindow.PlayPhaseStarting || t.Subject != SkillProgramTriggerSubject.Owner || t.TurnOwnerScope != SkillProgramTurnOwnerScope.Own || !t.Optional || t.ChoiceGroup != "play-mode" || t.Effects.Count != 3 || t.Effects.Any(e => e.Condition.Kind != SkillProgramConditionKind.Always)) ||
            p.Triggers.SingleOrDefault(t => t.Id == "draw-restraint") is not { Effects: [{ Op: SkillProgramEffectOp.PrepareBoundaryCaoZhangJiangchi, StateId: "restraint" }, { Op: SkillProgramEffectOp.Draw, Target: SkillProgramEffectTarget.Owner, Amount: 1 }, { Op: SkillProgramEffectOp.ArmBoundaryCaoZhangJiangchi, StateId: "restraint" }] } ||
            p.Triggers.SingleOrDefault(t => t.Id == "discard-assault") is not { Effects: [{ Op: SkillProgramEffectOp.PrepareBoundaryCaoZhangJiangchi, StateId: "assault" }, { Op: SkillProgramEffectOp.SelectAndMoveOwnedCard, Amount: 1, ResultBind: Cost, Destination: SkillProgramCardDestination.DiscardPile, AwaitMovementTriggers: true } paid, { Op: SkillProgramEffectOp.ArmBoundaryCaoZhangJiangchi, StateId: "assault" }] } ||
            !paid.Zones.SequenceEqual([CardZoneKind.Hand, CardZoneKind.Equipment]) || paid.CardOwnerRef?.Kind != ProgramParticipantRef.Owner || paid.ChooserRef?.Kind != ProgramParticipantRef.Owner)
            throw new InvalidOperationException(path + ": mobile Jiangchi requires exact own Play-start preparation, native one-card draw or real awaited HE discard, and one original-phase issue.");
    }
}
internal interface IBoundaryCaoZhangHost { SkillProgramStepOutcome Execute(ProgramSkillFrame frame, SkillProgramEffect effect); }
internal abstract class BoundaryCaoZhangDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "mode", "condition"); var mode = r.RequiredIdentifier("mode");
        if (mode is not ("restraint" or "assault")) throw new InvalidOperationException(r.Path + ": unknown mobile Jiangchi mode.");
        var e = new SkillProgramEffect(Op, FilterBoundCardsProgramOperationDescriptor.Owner(r), 0, r.Condition(), stateId: mode); RequireAlways(e, r.Path); return e;
    }
    public override ISkillProgramEffectHandler Handler => new BoundaryCaoZhangHandler(Op);
    public override ProgramFactRequirements FactRequirements => new(Triggered: ProgramTrackedFact.ActualDiscardRecoveryPhase | ProgramTrackedFact.NativeLogicalDiscard);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new RequireTriggerWindow(SkillProgramTriggerWindow.PlayPhaseStarting)];
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards, static (_, c) => c.PublicControlValue(2));
}
internal sealed class PrepareBoundaryCaoZhangDescriptor : BoundaryCaoZhangDescriptor { public override SkillProgramEffectOp Op => SkillProgramEffectOp.PrepareBoundaryCaoZhangJiangchi; }
internal sealed class ArmBoundaryCaoZhangDescriptor : BoundaryCaoZhangDescriptor { public override SkillProgramEffectOp Op => SkillProgramEffectOp.ArmBoundaryCaoZhangJiangchi; }
internal sealed class BoundaryCaoZhangHandler(SkillProgramEffectOp op) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => op;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e, ProgramSkillFrame f, int target, ISkillProgramEffectHost h) => h.GetCapability<IBoundaryCaoZhangHost>().Execute(f, e);
}
public sealed record BoundaryJiangchiPreparation(CardConversionSource Source, string Hash, ActualDiscardRecoveryPhaseKey Phase, int DebitPhase, string Mode, long Before);
public sealed record BoundaryJiangchiIssuedEvent(long FrameId, CardConversionSource Source, string Hash, ActualDiscardRecoveryPhaseKey Phase, int DebitPhase, string Mode, int? CostCard, long Before, long After) : IGameEvent;
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public BoundaryJiangchiPreparation? BoundaryJiangchi { get; init; }
}
