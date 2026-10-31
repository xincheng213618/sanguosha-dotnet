namespace CardGame.Core;

/// <summary>A paid one-card recast owned by its exact program instruction.</summary>
public sealed record ProgramBoundCardRecastReceipt(int InstructionIndex, string SourceBind,
    int CardId, CardKind CardKind, CardLocation SourceLocation,
    bool DrawApplied = false, int DrawCount = 0);

public interface IRecastBoundCardProgramEffectHost
{
    SkillProgramStepOutcome RecastBoundCard(ProgramSkillFrame frame, string sourceBind);
}

public sealed class RecastBoundCardSkillProgramEffectHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.RecastBoundCard;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame,
        int targetSeat, ISkillProgramEffectHost host) =>
        host.GetCapability<IRecastBoundCardProgramEffectHost>().RecastBoundCard(frame, effect.SourceBind!);
}

internal sealed class RecastBoundCardProgramOperationDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.RecastBoundCard;
    public override ISkillProgramEffectHandler Handler { get; } = new RecastBoundCardSkillProgramEffectHandler();
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.Move,
        static (effect, context) =>
        {
            context.Move(new(SkillProgramEffectOp.MoveBoundCards, SkillProgramEffectTarget.Owner, 0,
                new(SkillProgramConditionKind.Always, 0, []), sourceBind: effect.SourceBind,
                destination: SkillProgramCardDestination.DiscardPile));
            context.Draw(new(SkillProgramEffectOp.Draw, SkillProgramEffectTarget.Owner, 1, new(SkillProgramConditionKind.Always, 0, [])));
        });

    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "sourceBind", "condition");
        var target = r.RequiredEnum<SkillProgramEffectTarget>("target");
        if (target != SkillProgramEffectTarget.Owner)
            throw new InvalidOperationException($"Invalid skill program at {r.Path}.target: recastBoundCard requires owner.");
        var effect = new SkillProgramEffect(Op, target, 0, r.Condition(), sourceBind: r.RequiredIdentifier("sourceBind"));
        RequireAlways(effect, r.Path);
        return effect;
    }

    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.DrawPhaseEnded),
         new RequireOwnedCardSet(effect.SourceBind!, SkillProgramEffectTarget.Owner, 1,
            [CardZoneKind.Hand, CardZoneKind.Equipment]),
         new ReadSingleCardSet(effect.SourceBind!),
         new MoveCardSet(effect.SourceBind!, null, SkillProgramCardDestination.DiscardPile)];
}

public sealed partial class GameEngine
{
}
