using System.Text.Json.Serialization;

namespace CardGame.Core;

public enum ProgramBoundCardsRecastStage { CostChildren, Drawing, DrawChildren }

public sealed record ProgramBoundCardsRecastReceipt(int InstructionIndex, string SourceBind,
    CardConversionSource Source, string GameplayHash, int TurnNumber, int TurnOwnerSeat,
    TurnPhase Phase, int PhaseInstanceId, long SequenceBefore)
{
    private readonly IReadOnlyList<NativeRecastMaterial> _materials = Array.AsReadOnly(Array.Empty<NativeRecastMaterial>());
    public IReadOnlyList<NativeRecastMaterial> Materials
    { get => _materials; init => _materials = Array.AsReadOnly(value.ToArray()); }
    public ProgramBoundCardsRecastStage Stage { get; init; }
    public long CostBatchId { get; init; }
    public long SequenceAfter { get; init; }
    public long DrawSequenceBefore { get; init; }
    public long DrawSequenceAfter { get; init; }
    public int DrawCount { get; init; }
}

public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ProgramBoundCardsRecastReceipt? BoundCardsRecast { get; init; }
}

internal interface IBoundCardsRecastHost
{
    SkillProgramStepOutcome RecastBoundCards(ProgramSkillFrame frame, string sourceBind);
}

internal sealed class RecastBoundCardsDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.RecastBoundCards;
    public override ISkillProgramEffectHandler Handler { get; } = new RecastBoundCardsHandler();
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.Move,
        static (effect, context) =>
        {
            context.Move(new(SkillProgramEffectOp.MoveBoundCards, SkillProgramEffectTarget.Owner, 0,
                new(SkillProgramConditionKind.Always, 0, []), sourceBind: effect.SourceBind,
                destination: SkillProgramCardDestination.DiscardPile));
            context.Draw(new(SkillProgramEffectOp.Draw, SkillProgramEffectTarget.Owner, 0,
                new(SkillProgramConditionKind.Always, 0, []),
                numberExpression: SkillProgramNumberExpression.BoundCardCount, sourceBind: effect.SourceBind));
        });

    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader)
    {
        reader.AllowOnly("op", "target", "sourceBind", "condition");
        var effect = new SkillProgramEffect(Op, FilterBoundCardsProgramOperationDescriptor.Owner(reader), 0,
            reader.Condition(), sourceBind: reader.RequiredIdentifier("sourceBind"));
        RequireAlways(effect, reader.Path);
        return effect;
    }

    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireOwnedCardSet(effect.SourceBind!, SkillProgramEffectTarget.Owner, 2,
            [CardZoneKind.Hand, CardZoneKind.Equipment], MinimumCount: 1),
         new ReadCardSet(effect.SourceBind!),
         new MoveCardSet(effect.SourceBind!, null, SkillProgramCardDestination.DiscardPile)];
}

public sealed class RecastBoundCardsHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.RecastBoundCards;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame,
        int targetSeat, ISkillProgramEffectHost host) =>
        host.GetCapability<IBoundCardsRecastHost>().RecastBoundCards(frame, effect.SourceBind!);
}
