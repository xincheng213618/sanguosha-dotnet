using System.Text.Json;
using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record EquipmentCardTemplate(CardKind Kind, Suit Suit, int Rank);
public sealed partial class SkillProgramEffect
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<EquipmentCardTemplate>? EquipmentTemplates { get; internal init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? EquipmentOriginSource { get; internal init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? DistinctEquipmentCategories { get; internal init; }
}

public sealed record ProgramEquipmentOffer(string ResultBind, string Source,
    IReadOnlyList<EquipmentCardTemplate> Templates)
{
    private readonly IReadOnlyList<EquipmentCardTemplate> _templates = Array.AsReadOnly(Templates.ToArray());
    public IReadOnlyList<EquipmentCardTemplate> Templates { get => _templates; init => _templates = Array.AsReadOnly(value.ToArray()); }
}
public sealed record ProgramEquipmentPlacement(string SourceBind, int Index, int? RecipientSeat, bool AwaitingMovement);
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public ProgramEquipmentOffer? EquipmentOffer { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public ProgramEquipmentPlacement? EquipmentPlacement { get; init; }
}

internal interface IProgramEquipmentHost
{
    SkillProgramStepOutcome Offer(ProgramSkillFrame frame, SkillProgramEffect effect);
    void Select(ProgramSkillFrame frame, SkillProgramEffect effect);
    SkillProgramStepOutcome Place(ProgramSkillFrame frame, SkillProgramEffect effect);
}

internal sealed class OfferGeneratedEquipmentDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.OfferGeneratedEquipment;
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override ISkillProgramEffectHandler Handler { get; } = new EquipmentHandler(SkillProgramEffectOp.OfferGeneratedEquipment);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards, static (_, _) => { });
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "amount", "amountExpr", "resultBind", "source", "templates", "condition");
        var amount = r.AmountExpression();
        var count = amount is null ? r.RequiredInt("amount") : 0;
        var templates = r.RequiredObjects("templates").Select(template =>
        {
            template.AllowOnly("kind", "suit", "rank");
            var kind = template.RequiredEnum<CardKind>("kind");
            var suit = template.RequiredEnum<Suit>("suit");
            var rank = template.RequiredInt("rank");
            if (suit == Suit.None || rank is < 1 or > 13 || !EquipmentCatalog.IsEquipment(kind))
                throw new InvalidOperationException(template.Path + ": requires an executable equipment kind and a physical suit/rank.");
            return new EquipmentCardTemplate(kind, suit, rank);
        }).ToArray();
        if (templates.Length is < 1 or > 32 || templates.Select(t => t.Kind).Distinct().Count() != templates.Length ||
            amount is null && (count < 1 || count > templates.Length))
            throw new InvalidOperationException(r.Path + ": invalid distinct equipment pool or candidate count.");
        var source = r.RequiredIdentifier("source");
        if (!BuiltInEquipmentContent.All.SelectMany(p => p.DestinationPolicies).Any(p => p.SourceId == source))
            throw new InvalidOperationException(r.Path + ": equipment source requires a registered lifetime policy.");
        var effect = new SkillProgramEffect(Op, FilterBoundCardsProgramOperationDescriptor.Owner(r), count, r.Condition(),
            resultBind: r.RequiredIdentifier("resultBind"), cardKinds: Array.AsReadOnly(templates.Select(t => t.Kind).ToArray()))
        { AmountExpression = amount, EquipmentTemplates = Array.AsReadOnly(templates), EquipmentOriginSource = source };
        RequireAlways(effect, r.Path);
        return effect;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new CaptureSourceCard(e.ResultBind!, 1)];
}

internal sealed class SelectRandomPileEquipmentDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.SelectRandomPileEquipment;
    public override ISkillProgramEffectHandler Handler { get; } = new EquipmentHandler(SkillProgramEffectOp.SelectRandomPileEquipment);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards, static (_, _) => { });
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "amount", "resultBind", "zones", "equipmentSlots", "distinctEquipmentCategories", "condition");
        var zones = r.RequiredEnumArray<CardZoneKind>("zones");
        var count = r.RequiredInt("amount");
        var slots = r.OptionalEnumArray<EquipmentSlot>("equipmentSlots") ?? [];
        if (zones.Count == 0 || zones.Any(z => z is not (CardZoneKind.DrawPile or CardZoneKind.DiscardPile)) || count is < 1 or > 5)
            throw new InvalidOperationException(r.Path + ": equipment selection requires physical draw/discard piles and 1..5 cards.");
        return new(Op, FilterBoundCardsProgramOperationDescriptor.Owner(r), count, r.Condition(),
            resultBind: r.RequiredIdentifier("resultBind"), zones: zones, equipmentSlots: slots)
        { DistinctEquipmentCategories = r.Has("distinctEquipmentCategories") ? r.RequiredBool("distinctEquipmentCategories") : null };
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new CaptureSourceCard(e.ResultBind!, e.Amount)];
}

internal sealed class PlaceBoundEquipmentDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.PlaceBoundEquipment;
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override ISkillProgramEffectHandler Handler { get; } = new EquipmentHandler(SkillProgramEffectOp.PlaceBoundEquipment);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards, static (_, _) => { });
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "sourceBind", "chooseRecipient", "condition");
        return new(Op, FilterBoundCardsProgramOperationDescriptor.Owner(r), 0, r.Condition(), sourceBind: r.RequiredIdentifier("sourceBind"))
        { ChooseEquipmentRecipient = r.Has("chooseRecipient") && r.RequiredBool("chooseRecipient") };
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) =>
        [new ReadCardSet(e.SourceBind!), new MoveCardSet(e.SourceBind!, null, SkillProgramCardDestination.SelectedTargetEquipment)];
}
public sealed partial class SkillProgramEffect
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ChooseEquipmentRecipient { get; internal init; }
}
internal sealed class EquipmentHandler(SkillProgramEffectOp op) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => op;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host)
    {
        var equipment = host.GetCapability<IProgramEquipmentHost>();
        if (op == SkillProgramEffectOp.OfferGeneratedEquipment) return equipment.Offer(frame, effect);
        if (op == SkillProgramEffectOp.PlaceBoundEquipment) return equipment.Place(frame, effect);
        equipment.Select(frame, effect);
        return SkillProgramStepOutcome.Continue;
    }
}
