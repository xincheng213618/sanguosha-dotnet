namespace CardGame.Core;

internal sealed class ClaimDeathCleanupCardsProgramOperationDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ClaimDeathCleanupCards;
    public override ISkillProgramEffectHandler Handler { get; } = new ClaimDeathCleanupCardsSkillProgramEffectHandler();
    public override ProgramContextCapability RequiredCapabilities => ProgramContextCapability.Death;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.ClaimDeathCleanupCards,
        static (_, context) => context.ClaimDeathCleanupCards());

    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "zones", "condition");
        var target = r.RequiredEnum<SkillProgramEffectTarget>("target");
        if (target != SkillProgramEffectTarget.Owner)
            throw new InvalidOperationException(
                $"Invalid skill program at {r.Path}.target: death cleanup cards always belong to the skill owner.");
        var zones = r.OptionalEnumArray<CardZoneKind>("zones") ?? [];
        if (r.Has("zones") && (zones.Count == 0 || zones.Any(z => z is not (CardZoneKind.Hand or CardZoneKind.Equipment or CardZoneKind.Judgment))))
            throw new InvalidOperationException($"Invalid skill program at {r.Path}.zones: death claims accept a nonempty hand/equipment/judgment source filter.");
        return new(Op, target, 0, r.Condition(), zones: zones);
    }

    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [];
}

public sealed class ClaimDeathCleanupCardsSkillProgramEffectHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.ClaimDeathCleanupCards;

    public SkillProgramStepOutcome Execute(
        SkillProgramEffect effect,
        ProgramSkillFrame frame,
        int targetSeat,
        ISkillProgramEffectHost host)
    {
        host.ClaimDeathCleanupCards(frame);
        return effect.Zones.Count > 0
            ? host.GetCapability<IBoundCardMovementContinuationHost>().AwaitBoundCardMovements(frame.Id, frame.OwnerSeat)
            : SkillProgramStepOutcome.Continue;
    }
}
