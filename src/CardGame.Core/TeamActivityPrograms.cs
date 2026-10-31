namespace CardGame.Core;

internal interface ITeamActivityProgramHost
{
    SkillProgramStepOutcome ExecuteTeamActivity(ProgramSkillFrame frame, SkillProgramEffect effect);
}

internal sealed class SelectAllPublicTeamTargetsDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.SelectAllPublicTeamTargets;
    public override bool AllowsDeadTarget(in ProgramEffectContinuationContext context) =>
        context.Frame.WindowContext?.Window == SkillProgramTriggerWindow.OwnerDied;
    public override ISkillProgramEffectHandler Handler { get; } = new TeamActivityEffectHandler(SkillProgramEffectOp.SelectAllPublicTeamTargets);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.SelectTargets,
        static (effect, context) => context.SelectTargets(effect));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "targetKind", "allowEmpty", "condition");
        var kind = r.RequiredEnum<SkillProgramTargetKind>("targetKind");
        if (kind is not (SkillProgramTargetKind.PublicTeamEnemies or SkillProgramTargetKind.PublicTeamWoundedAllies or
            SkillProgramTargetKind.OtherPublicTeamWoundedAllies or SkillProgramTargetKind.PublicTeamUnchainedEnemies or
            SkillProgramTargetKind.OtherPublicTeamAllies))
            throw new InvalidOperationException($"{r.Path}: a public-team target set is required.");
        var minimum = r.Has("allowEmpty") && r.RequiredBool("allowEmpty") ? 0 : 1;
        var effect = new SkillProgramEffect(Op, FilterBoundCardsProgramOperationDescriptor.Owner(r), 0, r.Condition(), targetKind: kind,
            minimumTargets: minimum, maximumTargets: 8);
        RequireAlways(effect, r.Path);
        return effect;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [new SelectTargetSet(effect.MinimumTargets, 8)];
}

internal sealed class SelectRandomPublicTeamTargetDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.SelectRandomPublicTeamTarget;
    public override ISkillProgramEffectHandler Handler { get; } = new TeamActivityEffectHandler(SkillProgramEffectOp.SelectRandomPublicTeamTarget);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.SelectTargets,
        static (effect, context) => context.SelectTargets(effect));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "targetKind", "allowEmpty", "condition");
        var kind = r.RequiredEnum<SkillProgramTargetKind>("targetKind");
        if (kind is not (SkillProgramTargetKind.PublicTeamEnemies or SkillProgramTargetKind.PublicTeamWoundedAllies or
            SkillProgramTargetKind.OtherPublicTeamWoundedAllies or SkillProgramTargetKind.PublicTeamUnchainedEnemies or
            SkillProgramTargetKind.OtherPublicTeamAllies))
            throw new InvalidOperationException($"{r.Path}: a public-team target set is required.");
        var minimum = r.Has("allowEmpty") && r.RequiredBool("allowEmpty") ? 0 : 1;
        var effect = new SkillProgramEffect(Op, FilterBoundCardsProgramOperationDescriptor.Owner(r), 0, r.Condition(),
            targetKind: kind, minimumTargets: minimum, maximumTargets: 1);
        RequireAlways(effect, r.Path);
        return effect;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new SelectTargetSet(effect.MinimumTargets, 1)];
}

internal sealed class RepeatCurrentRedTrickDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.RepeatCurrentRedTrick;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.UseSelectedCardsAs,
        static (_, _) => { });
    public override ISkillProgramEffectHandler Handler { get; } = new TeamActivityEffectHandler(SkillProgramEffectOp.RepeatCurrentRedTrick);
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "condition");
        var effect = new SkillProgramEffect(Op, FilterBoundCardsProgramOperationDescriptor.Owner(r), 0, r.Condition());
        RequireAlways(effect, r.Path);
        return effect;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.CardUseCommitted)];
}

internal sealed class TeamActivityEffectHandler(SkillProgramEffectOp op) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => op;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int seat, ISkillProgramEffectHost host) =>
        host.GetCapability<ITeamActivityProgramHost>().ExecuteTeamActivity(frame, effect);
}

public sealed record PublicTeamTargetsBoundEvent(long ProgramFrameId, int OwnerSeat, SkillProgramTargetKind Kind, int Count) : IGameEvent
{
    public int InstructionIndex { get; init; }
    public SkillProgramEffectOp SelectionOp { get; init; } = SkillProgramEffectOp.SelectAllPublicTeamTargets;
    // Scalar provenance keeps the issued target order immutable even after a
    // target dies or changes teams while an earlier damage child resolves.
    public string TargetSeatsKey { get; init; } = "";
}
public sealed record AutomaticExtraCardEffectIssuedEvent(long ProgramFrameId, CardConversionSource Source,
    long CardUseFrameId, long ActionId, string GameplayHash) : IGameEvent;

public sealed partial class GameEngine
{
}
