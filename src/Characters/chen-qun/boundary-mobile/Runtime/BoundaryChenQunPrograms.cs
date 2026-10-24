using System.Text.Json.Serialization;

namespace CardGame.Core;

internal static class BoundaryChenQunComposition
{
    internal const string Skill = "boundary:dingpin-mobile", Activation = "judge-unused-category", Cost = "dingpin-cost", Card = "dingpin-card", Outcome = "dingpin-outcome", Reason = "skill.boundary.dingpin-mobile";
    [ProgramCatalogValidation(53600)]
    internal static void Validate(string path, SkillProgram p)
    {
        if (!p.Activations.SelectMany(a => a.Effects).Concat(p.Triggers.SelectMany(t => t.Effects)).Any(e => e.Op is SkillProgramEffectOp.PrepareBoundaryChenQunDingpin or SkillProgramEffectOp.ResolveBoundaryChenQunDingpin)) return;
        if (p.Id != Skill || p.Triggers.Count != 0 || p.ViewAs.Count != 0 || p.Modifiers.Count != 0 || p.CardPolicies.Count != 0 || p.CardIdentities.Count != 0 || p.HpSkillGrants.Count != 0 || p.DamageModifiers.Count != 0 ||
            p.Activations is not [{ Id: Activation, MinCards: 1, MaxCards: 1, MinTargets: 1, MaxTargets: 1, TargetKind: SkillProgramTargetKind.AnyLiving, UsesPerTurn: null,
                Effects: [{ Op: SkillProgramEffectOp.PrepareBoundaryChenQunDingpin }, { Op: SkillProgramEffectOp.CaptureSelectedCards, ResultBind: Cost },
                    { Op: SkillProgramEffectOp.MoveBoundCards, SourceBind: Cost, Destination: SkillProgramCardDestination.DiscardPile, AwaitMovementTriggers: true },
                    { Op: SkillProgramEffectOp.StartJudgment, Target: SkillProgramEffectTarget.SelectedTarget, JudgmentReason: Reason, ResultBind: Card, OutcomeBind: Outcome, Visibility: SkillProgramCardSetVisibility.Public },
                    { Op: SkillProgramEffectOp.ResolveBoundaryChenQunDingpin }, { Op: SkillProgramEffectOp.TurnOver, Target: SkillProgramEffectTarget.Owner, Condition: { Kind: SkillProgramConditionKind.JudgmentOutcomeMatchesSuits, SourceBind: Outcome } },
                    { Op: SkillProgramEffectOp.MoveBoundCards, SourceBind: Card, Destination: SkillProgramCardDestination.DiscardPile, AwaitMovementTriggers: true }] } a] ||
            !a.SourceZones.SequenceEqual(new[] { CardZoneKind.Hand, CardZoneKind.Equipment }) || a.UsesPerPhase is not null ||
            !a.Effects[5].Condition.Suits.SequenceEqual(new[] { Suit.Diamond }) || a.Effects.Where((_, i) => i != 5).Any(e => e.Condition.Kind != SkillProgramConditionKind.Always) ||
            a.Condition.Kind != SkillProgramConditionKind.Always)
            throw new InvalidOperationException(path + ": mobile Dingpin requires its original one-card HE discard, native target judgment, exact heart exception, bounded HP draw and diamond turnover.");
    }
}
internal interface IBoundaryChenQunHost { SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op); }
internal interface IBoundaryChenQunSelection : IProgramSelectionQuery
{ bool CanDiscard(int owner, Card card); bool CanTarget(int owner, int target); }
[ProgramSelectionModule(ProgramSelectionStage.CardAfterDiscard, 53600, SkillProgramEffectOp.PrepareBoundaryChenQunDingpin)]
internal sealed class BoundaryChenQunCards : IProgramSelectionCardFilter
{ public bool Allows(ProgramSelectionContext c, Card card, IProgramSelectionQueries q) => q.Get<IBoundaryChenQunSelection>().CanDiscard(c.OwnerSeat, card); }
[ProgramSelectionModule(ProgramSelectionStage.OrdinaryTarget, 53600, SkillProgramEffectOp.PrepareBoundaryChenQunDingpin)]
internal sealed class BoundaryChenQunTargets : IProgramSelectionTargetFilter
{ public bool Allows(ProgramSelectionContext c, int target, IProgramSelectionQueries q) => q.Get<IBoundaryChenQunSelection>().CanTarget(c.OwnerSeat, target); }
internal abstract class BoundaryChenQunDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override ISkillProgramEffectHandler Handler => new BoundaryChenQunHandler(Op);
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Automatic;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards, static (_, c) => c.PublicControlValue(2));
    public override ProgramFactRequirements FactRequirements => new(Activated: ProgramTrackedFact.NativeLogicalDiscard | ProgramTrackedFact.ActualDiscardRecoveryPhase | ProgramTrackedFact.WholeActualUseDeclaration);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => Op == SkillProgramEffectOp.PrepareBoundaryChenQunDingpin
        ? [new ReadSelectedTarget()] : [new ReadSelectedTarget(), new ReadCardSet(BoundaryChenQunComposition.Cost)];
    public override bool AllowsDeadTarget(in ProgramEffectContinuationContext context) => context.Frame.BoundaryChenQunDingpin is not null;
}
internal sealed class PrepareBoundaryChenQunDingpinDescriptor : BoundaryChenQunDescriptor { public override SkillProgramEffectOp Op => SkillProgramEffectOp.PrepareBoundaryChenQunDingpin; }
internal sealed class ResolveBoundaryChenQunDingpinDescriptor : BoundaryChenQunDescriptor { public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveBoundaryChenQunDingpin; }
internal sealed class BoundaryChenQunHandler(SkillProgramEffectOp op) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => op;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e, ProgramSkillFrame f, int target, ISkillProgramEffectHost h) => h.GetCapability<IBoundaryChenQunHost>().Resolve(f, op);
}
public enum BoundaryDingpinStage { Prepared, Drawing, DrawChildren, Resolved }
public sealed record BoundaryDingpinReceipt(CardConversionSource Source, string Hash, ActualDiscardRecoveryPhaseKey Phase, int Target, int CostId, CardLocation From, SkillProgramCardCategory Category, long Before)
{
    public BoundaryDingpinStage Stage { get; init; }
    public long? CostInvocationId { get; init; }
    public long? CostSequence { get; init; }
    public ProgramJudgmentOutcomeBinding? Judgment { get; init; }
    public int Requested { get; init; }
    public NativeDrawOutcome? Draw { get; init; }
}
public sealed partial record ProgramSkillFrame
{ [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public BoundaryDingpinReceipt? BoundaryChenQunDingpin { get; init; } }
public sealed record BoundaryDingpinPreparedEvent(long FrameId, CardConversionSource Source, string Hash, ActualDiscardRecoveryPhaseKey Phase, int Target, int CardId, CardLocation From, SkillProgramCardCategory Category, long Before) : IGameEvent;
public sealed record BoundaryDingpinResultEvent(long FrameId, int Target, long JudgmentId, Suit? Suit, long CostInvocationId, long CostSequence, int Requested) : IGameEvent;
public sealed record BoundaryDingpinHeartExceptionEvent(long FrameId, int Owner, int Turn, int TurnOwner, long InvocationId, int CardId, long CostSequence) : IGameEvent;
public sealed record BoundaryDingpinTargetBannedEvent(long FrameId, int Owner, int Target, int Turn, int TurnOwner) : IGameEvent;
public sealed record BoundaryDingpinDrawReturnedEvent(long FrameId, int Target, int Requested, int Actual, int Replaced, long Before, long After) : IGameEvent;
