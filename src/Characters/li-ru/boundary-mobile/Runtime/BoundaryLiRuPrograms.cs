using System.Text.Json.Serialization;
namespace CardGame.Core;

internal interface IBoundaryLiRuHost { SkillProgramStepOutcome ResolveMieji(ProgramSkillFrame frame); }
internal interface IBoundaryLiRuQueries : IProgramSelectionQuery { IReadOnlyList<int> LostCardTargets(int owner); bool HasHeCards(int target); }
[ProgramSelectionModule(ProgramSelectionStage.OrdinaryTarget, 55700, SkillProgramEffectOp.ResolveBoundaryLiRuMieji)]
internal sealed class BoundaryLiRuMiejiTargets : IProgramSelectionTargetFilter
{
    public bool Allows(ProgramSelectionContext context, int target, IProgramSelectionQueries queries) => queries.Get<IBoundaryLiRuQueries>().HasHeCards(target);
}
[ProgramTargetResolver(SkillProgramTargetKind.BoundaryLiRuLostCardsThisTurn)]
internal sealed class BoundaryLiRuLostCardResolver : IProgramTargetResolver
{
    public IReadOnlyList<int> Resolve(ProgramTargetResolverContext context, IProgramSelectionQueries queries) => queries.Get<IBoundaryLiRuQueries>().LostCardTargets(context.OwnerSeat);
}
internal sealed class ResolveBoundaryLiRuMiejiDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveBoundaryLiRuMieji;
    public override ISkillProgramEffectHandler Handler { get; } = new BoundaryLiRuMiejiHandler();
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override ProgramOperationAiPolicy AiPolicy => new(ProgramOperationAiSemantic.GainCards, static (_, c) => c.PublicControlValue(3));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [new ReadSelectedTarget()];
}
internal sealed class BoundaryLiRuMiejiHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveBoundaryLiRuMieji;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int target, ISkillProgramEffectHost host) => host.GetCapability<IBoundaryLiRuHost>().ResolveMieji(frame);
}
internal static class BoundaryLiRuComposition
{
    internal const string Mieji = "boundary:mieji-mobile";
    [ProgramCatalogValidation(55700)]
    internal static void Validate(string path, SkillProgram p)
    {
        if (!p.Activations.SelectMany(a => a.Effects).Concat(p.Triggers.SelectMany(t => t.Effects)).Any(e => e.Op == SkillProgramEffectOp.ResolveBoundaryLiRuMieji)) return;
        if (p.Id != Mieji || p.Triggers.Count != 0 || p.Activations is not [{ MinCards: 1, MaxCards: 1, SourceZones: [CardZoneKind.Hand], CardCategories: [SkillProgramCardCategory.Trick],
            MinTargets: 1, MaxTargets: 1, TargetKind: SkillProgramTargetKind.OtherLiving, UsesPerPhase: 1, Effects:
                [{ Op: SkillProgramEffectOp.CaptureSelectedCards, ResultBind: "mieji-cost" }, { Op: SkillProgramEffectOp.RevealBoundCards, SourceBind: "mieji-cost" },
                 { Op: SkillProgramEffectOp.MoveBoundCards, SourceBind: "mieji-cost", Destination: SkillProgramCardDestination.DrawPileTop, AwaitMovementTriggers: true },
                 { Op: SkillProgramEffectOp.ResolveBoundaryLiRuMieji }] }] || !p.Activations[0].CardSuits.Order().SequenceEqual(new[] { Suit.Spade, Suit.Club }.Order()))
            throw new InvalidOperationException(path + ": mobile Mieji requires its original revealed black hand-Trick top-deck payment and one other target.");
    }
}
public enum BoundaryLiRuMiejiStage { Branch, Gift, Discard, Movement }
public sealed record BoundaryLiRuMiejiPayment(int CardId, CardLocation From, long Before, long After, long Batch);
public sealed record BoundaryLiRuMiejiDraft(int TargetSeat, CardConversionSource Source, string GameplayHash, BoundaryLiRuMiejiStage Stage)
{
    public bool Gift { get; init; }
    public int Required { get; init; }
    private readonly IReadOnlyList<int> _cards = Array.AsReadOnly(Array.Empty<int>());
    private readonly IReadOnlyList<CardLocation> _from = Array.AsReadOnly(Array.Empty<CardLocation>());
    public IReadOnlyList<int> Cards { get => _cards; init => _cards = Array.AsReadOnly(value.ToArray()); }
    public IReadOnlyList<CardLocation> From { get => _from; init => _from = Array.AsReadOnly(value.ToArray()); }
    public long Before { get; init; }
    public long After { get; init; }
    public long Batch { get; init; }
    private readonly IReadOnlyList<BoundaryLiRuMiejiPayment> _payments = Array.AsReadOnly(Array.Empty<BoundaryLiRuMiejiPayment>());
    public IReadOnlyList<BoundaryLiRuMiejiPayment> Payments { get => _payments; init => _payments = Array.AsReadOnly(value.ToArray()); }
}
public sealed record BoundaryLiRuMiejiStartedEvent(long FrameId, int OwnerSeat, int TargetSeat, string SkillInstanceId, string GameplayHash) : IGameEvent;
public sealed record BoundaryLiRuMiejiBranchEvent(long FrameId, int TargetSeat, bool Gift, int Required) : IGameEvent;
public sealed record BoundaryLiRuMiejiPaidEvent(long FrameId, int OwnerSeat, int TargetSeat, bool Gift, int Ordinal, int Count, long Before, long After, long Batch) : IGameEvent;
public sealed partial record ProgramSkillFrame
{ [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public BoundaryLiRuMiejiDraft? BoundaryLiRuMieji { get; init; } }
