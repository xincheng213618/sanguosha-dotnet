namespace CardGame.Core;

/// <summary>
/// Hold up to the configured maximum, or the holder's current HP by default, on their own
/// general card until the current turn ends. The skill owner chooses among opaque hand
/// slots and visible equipment; the engine returns held cards to the holder's hand at
/// the end of the turn (equipment included) and discards them if the holder dies first.
/// </summary>
internal sealed class HoldTargetCardsProgramOperationDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.HoldTargetCards;
    public override ISkillProgramEffectHandler Handler { get; } = new HoldTargetCardsSkillProgramEffectHandler();
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(
        ProgramOperationAiSemantic.HoldTargetCards, static (effect, context) => context.HoldTargetCards(effect));

    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "chooserRef", "cardOwnerRef", "zones", "resultBind", "minimumCards", "maximumCards", "condition");
        var target = r.RequiredEnum<SkillProgramEffectTarget>("target");
        if (target != SkillProgramEffectTarget.Owner)
            throw new InvalidOperationException($"Invalid skill program at {r.Path}.target: must be owner.");
        var chooserRef = r.RequiredParticipantReference("chooserRef");
        var cardOwnerRef = r.RequiredParticipantReference("cardOwnerRef");
        if (cardOwnerRef.Kind is not (ProgramParticipantRef.EventTarget or ProgramParticipantRef.SelectedTarget or
                ProgramParticipantRef.SelectedFirst or ProgramParticipantRef.SelectedSecond))
            throw new InvalidOperationException(
                $"Invalid skill program at {r.Path}.cardOwnerRef: the held cards must belong to another participant.");
        var zones = r.RequiredEnumArray<CardZoneKind>("zones");
        if (zones.Count == 0 || zones.Any(zone => zone is not (CardZoneKind.Hand or CardZoneKind.Equipment or CardZoneKind.Judgment)))
            throw new InvalidOperationException($"Invalid skill program at {r.Path}.zones: requires hand, equipment, or judgment.");
        var minimumCards = r.Has("minimumCards") ? r.RequiredInt("minimumCards") : 1;
        if (minimumCards is < 0 or > 1)
            throw new InvalidOperationException($"Invalid skill program at {r.Path}.minimumCards: must be 0 or 1.");
        var maximumCards = r.Has("maximumCards") ? r.RequiredInt("maximumCards") : 0;
        if (r.Has("maximumCards") && (maximumCards < Math.Max(1, minimumCards) || maximumCards > 32))
            throw new InvalidOperationException($"Invalid skill program at {r.Path}.maximumCards: requires a positive count no greater than 32.");
        var effect = new SkillProgramEffect(Op, target, 0, r.Condition(), zones: zones,
            resultBind: r.RequiredIdentifier("resultBind"), minimumCards: minimumCards, maximumCards: maximumCards,
            chooserRef: chooserRef, cardOwnerRef: cardOwnerRef);
        if (effect.Condition.Kind != SkillProgramConditionKind.Always)
            throw new InvalidOperationException(
                $"Invalid skill program at {r.Path}.condition: card holding must be unconditional.");
        return effect;
    }

    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        ParticipantResources(effect.ChooserRef, effect.CardOwnerRef)
            .Concat(new ProgramResourceOperation[] { new CreateCardSet(effect.ResultBind!, effect.MaximumCards > 0 ? effect.MaximumCards : int.MaxValue, false) })
            .ToArray();
}

public sealed class HoldTargetCardsSkillProgramEffectHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.HoldTargetCards;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame,
        int targetSeat, ISkillProgramEffectHost host) =>
        host.HoldTargetCards(frame,
            host.ResolveParticipant(frame, effect.ChooserRef!),
            host.ResolveParticipant(frame, effect.CardOwnerRef!),
            effect.Zones, effect.ResultBind!, effect.MinimumCards, effect.MaximumCards);
}
