using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record DeckCardGainReceipt(int InstructionIndex, CardConversionSource Source,
    string GameplayHash, int Requested, SkillProgramCardCategory? Category, long SequenceBefore)
{
    private readonly IReadOnlyList<int> _cardIds = Array.AsReadOnly(Array.Empty<int>());
    private readonly IReadOnlyList<CardKind> _selectedKinds = Array.AsReadOnly(Array.Empty<CardKind>());
    private readonly IReadOnlyList<CardKind> _filterKinds = Array.AsReadOnly(Array.Empty<CardKind>());
    public IReadOnlyList<int> CardIds { get => _cardIds; init => _cardIds = Array.AsReadOnly(value.ToArray()); }
    public IReadOnlyList<CardKind> SelectedKinds { get => _selectedKinds; init => _selectedKinds = Array.AsReadOnly(value.ToArray()); }
    public IReadOnlyList<CardKind> FilterKinds { get => _filterKinds; init => _filterKinds = Array.AsReadOnly(value.ToArray()); }
    public long BatchId { get; init; }
    public long SequenceAfter { get; init; }
}

public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DeckCardGainReceipt? DeckCardGain { get; init; }
}

public sealed record DeckCardsObtainedEvent(long ProgramFrameId, int InstructionIndex,
    int OwnerSeat, int Count, long BatchId, long SequenceBefore, long SequenceAfter) : IGameEvent;

internal interface IDeckCardsHost
{
    SkillProgramStepOutcome ObtainDeckCards(ProgramSkillFrame frame, SkillProgramEffect effect);
}

internal sealed class ObtainDeckCardsDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ObtainDeckCards;
    public override ISkillProgramEffectHandler Handler { get; } = new ObtainDeckCardsHandler();
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards, static (_, _) => { });
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader)
    {
        reader.AllowOnly("op", "target", "amount", "cardKinds", "category", "condition");
        var amount = reader.RequiredInt("amount");
        if (amount is < 1 or > 4 || reader.Has("cardKinds") == reader.Has("category"))
            throw new InvalidOperationException($"Invalid skill program at {reader.Path}: a deck gain requires 1..4 cards and exactly one kind-list or category filter.");
        var kinds = reader.OptionalEnumArray<CardKind>("cardKinds") ?? [];
        if (reader.Has("cardKinds") && kinds.Count == 0)
            throw new InvalidOperationException($"Invalid skill program at {reader.Path}.cardKinds: requires a nonempty distinct kind list.");
        var effect = new SkillProgramEffect(Op, FilterBoundCardsProgramOperationDescriptor.Owner(reader), amount,
            reader.Condition(), cardKinds: kinds,
            cardCategories: reader.Has("category") ? [reader.RequiredEnum<SkillProgramCardCategory>("category")] : []);
        RequireAlways(effect, reader.Path);
        return effect;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [];
}

public sealed class ObtainDeckCardsHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.ObtainDeckCards;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host) =>
        host.GetCapability<IDeckCardsHost>().ObtainDeckCards(frame, effect);
}
