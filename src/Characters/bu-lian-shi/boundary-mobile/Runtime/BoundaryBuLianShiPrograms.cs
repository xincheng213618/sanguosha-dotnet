using System.Text.Json.Serialization;
namespace CardGame.Core;
internal interface IBoundaryBuLianShiHost { void Prepare(ProgramSkillFrame frame); void SelectLeast(ProgramSkillFrame frame); }
internal interface IBoundaryBuLianShiQueries : IProgramSelectionQuery { bool HasHe(int seat); IReadOnlyList<int> Least(int owner); }
[ProgramTargetResolver(SkillProgramTargetKind.BoundaryBuLianShiAnxuLeast)]
internal sealed class BoundaryBuLianShiLeastResolver : IProgramTargetResolver
{ public IReadOnlyList<int> Resolve(ProgramTargetResolverContext context, IProgramSelectionQueries queries) => queries.Get<IBoundaryBuLianShiQueries>().Least(context.OwnerSeat); }
[ProgramSelectionModule(ProgramSelectionStage.OrdinaryTarget, 55900, SkillProgramEffectOp.PrepareBoundaryBuLianShiAnxu)]
internal sealed class BoundaryBuLianShiOtherTargets : IProgramSelectionTargetFilter
{ public bool Allows(ProgramSelectionContext context, int target, IProgramSelectionQueries queries) => target != context.OwnerSeat; }
[ProgramSelectionModule(ProgramSelectionStage.AfterTargets, 55900, SkillProgramEffectOp.PrepareBoundaryBuLianShiAnxu)]
[ProgramSelectionModule(ProgramSelectionStage.Relationships, 55900, SkillProgramEffectOp.PrepareBoundaryBuLianShiAnxu)]
internal sealed class BoundaryBuLianShiPairs : IProgramSelectionCandidateFilter
{
    public bool Apply(ProgramSelectionContext context, ProgramSelectionCandidates candidates, IProgramSelectionQueries queries)
    {
        // LivingPairDistinct activations receive their pair domain from their
        // owning selection module, just like the existing pair-Obtain flow.
        var players = queries.Get<ISelectionParticipantQueries>();
        candidates.Targets = players.Seats.Where(p => p != context.OwnerSeat && players.IsAlive(p)).ToArray();
        candidates.TargetSequences = Array.AsReadOnly(candidates.Targets.Where(a => a != context.OwnerSeat).SelectMany(recipient => candidates.Targets
            .Where(donor => donor != context.OwnerSeat && donor != recipient && queries.Get<IBoundaryBuLianShiQueries>().HasHe(donor))
            .Select(donor => (IReadOnlyList<int>)Array.AsReadOnly(new[] { recipient, donor }))).ToArray());
        return candidates.TargetSequences.Count > 0;
    }
}
internal abstract class BoundaryBuLianShiDescriptor : ProgramOperationDescriptorBase
{
    public override ISkillProgramEffectHandler Handler => new BoundaryBuLianShiHandler(Op);
    public override ProgramOperationAiPolicy AiPolicy => new(ProgramOperationAiSemantic.GainCards, static (_, c) => c.PublicControlValue(2));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader)
    {
        reader.AllowOnly("op", "target", "condition");
        var effect = new SkillProgramEffect(Op, FilterBoundCardsProgramOperationDescriptor.Owner(reader), 0, reader.Condition(),
            sourceBind: Op == SkillProgramEffectOp.PrepareBoundaryBuLianShiAnxu ? "anxu-obtained" : null,
            resultBind: Op == SkillProgramEffectOp.PrepareBoundaryBuLianShiAnxu ? "anxu-hand-reward" : null);
        RequireAlways(effect, reader.Path); return effect;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => Op == SkillProgramEffectOp.PrepareBoundaryBuLianShiAnxu
        ? [new ReadTargetSet(2, 2), new ReadCardSet("anxu-obtained"), new CreateCardSet("anxu-hand-reward", 1, false)]
        : [new ReadTargetSet(2, 2), new NarrowTargetSetToSingle()];
}
internal sealed class PrepareBoundaryBuLianShiAnxuDescriptor : BoundaryBuLianShiDescriptor { public override SkillProgramEffectOp Op => SkillProgramEffectOp.PrepareBoundaryBuLianShiAnxu; }
internal sealed class SelectBoundaryBuLianShiAnxuLeastDescriptor : BoundaryBuLianShiDescriptor { public override SkillProgramEffectOp Op => SkillProgramEffectOp.SelectBoundaryBuLianShiAnxuLeast; }
internal sealed class BoundaryBuLianShiHandler(SkillProgramEffectOp op) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => op;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame f, int target, ISkillProgramEffectHost host)
    { if (op == SkillProgramEffectOp.PrepareBoundaryBuLianShiAnxu) host.GetCapability<IBoundaryBuLianShiHost>().Prepare(f); else host.GetCapability<IBoundaryBuLianShiHost>().SelectLeast(f); return SkillProgramStepOutcome.Continue; }
}
internal static class BoundaryBuLianShiComposition
{
    internal const string Anxu = "boundary:anxu-mobile";
    [ProgramCatalogValidation(55900)]
    internal static void Validate(string path, SkillProgram p)
    {
        if (!p.Activations.SelectMany(a => a.Effects).Concat(p.Triggers.SelectMany(t => t.Effects)).Any(e => e.Op is SkillProgramEffectOp.PrepareBoundaryBuLianShiAnxu or SkillProgramEffectOp.SelectBoundaryBuLianShiAnxuLeast)) return;
        if (p.Id != Anxu || p.Triggers.Count != 0 || p.Activations is not [{ MinCards: 0, MaxCards: 0, MinTargets: 2, MaxTargets: 2, TargetKind: SkillProgramTargetKind.LivingPairDistinct, UsesPerPhase: 1,
            Effects: [{ Op: SkillProgramEffectOp.SelectAndMoveOwnedCard, ChooserRef.Kind: ProgramParticipantRef.SelectedFirst, CardOwnerRef.Kind: ProgramParticipantRef.SelectedSecond,
                TargetReference.Kind: ProgramParticipantRef.SelectedFirst, Zones: [CardZoneKind.Hand, CardZoneKind.Equipment], Destination: SkillProgramCardDestination.SelectedTargetHand, ResultBind: "anxu-obtained", AwaitMovementTriggers: true },
                { Op: SkillProgramEffectOp.PrepareBoundaryBuLianShiAnxu }, { Op: SkillProgramEffectOp.Draw, Target: SkillProgramEffectTarget.Owner, NumberExpression: SkillProgramNumberExpression.BoundCardCount, SourceBind: "anxu-hand-reward" },
                { Op: SkillProgramEffectOp.SelectBoundaryBuLianShiAnxuLeast },
                { Op: SkillProgramEffectOp.ChooseOption, Target: SkillProgramEffectTarget.Owner, ResultBind: "anxu-benefit", Condition.Kind: SkillProgramConditionKind.Always, Options:
                    [{ Id: "draw", RequiredTargetKind: SkillProgramTargetKind.BoundaryBuLianShiAnxuLeast, Condition.Kind: SkillProgramConditionKind.Always },
                     { Id: "skip", RequiredTargetKind: null, Condition.Kind: SkillProgramConditionKind.Always }] },
                { Op: SkillProgramEffectOp.Draw, Target: SkillProgramEffectTarget.SelectedTarget, Amount: 1,
                    Condition: { Kind: SkillProgramConditionKind.ChoiceIs, SourceBind: "anxu-benefit", OptionId: "draw" } }] }])
            throw new InvalidOperationException(path + ": mobile Anxu requires one original recipient/donor Obtain and its ordered native rewards.");
    }
}
public sealed record BoundaryBuLianShiAnxuReceipt(int RecipientSeat, int DonorSeat, int CardId, CardLocation From, long MovementSequence, bool Delivered)
{ public long DeliverySequence { get; init; } public bool LeastSelected { get; init; } public int? LeastSeat { get; init; } }
public sealed record BoundaryBuLianShiAnxuPreparedEvent(long FrameId, int OwnerSeat, string SkillInstanceId, string GameplayHash, int RecipientSeat, int DonorSeat, CardZoneKind SourceZone, long MovementSequence) : IGameEvent;
public sealed record BoundaryBuLianShiAnxuDeliveredEvent(long FrameId, long SourceSequence, long DeliverySequence) : IGameEvent;
public sealed record BoundaryBuLianShiAnxuLeastEvent(long FrameId, int RecipientSeat, int DonorSeat, int RecipientHand, int DonorHand, int? LeastSeat) : IGameEvent;
public sealed partial record ProgramSkillFrame
{ [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public BoundaryBuLianShiAnxuReceipt? BoundaryBuLianShiAnxu { get; init; } }
