using System.Globalization;

namespace CardGame.Core;

internal interface IGhostActivityProgramHost
{
    SkillProgramStepOutcome ExecuteGhostActivity(ProgramSkillFrame frame, SkillProgramEffect effect);
}

internal abstract class GhostActivityOperationDescriptor : ProgramOperationDescriptorBase
{
    public override ISkillProgramEffectHandler Handler => new GhostActivityEffectHandler(Op);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards, static (_, _) => { });
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [];
}

internal sealed class DiscardRandomJudgmentDescriptor : GhostActivityOperationDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.DiscardRandomJudgment;
}

internal sealed class RewardOtherRecoverySourceDescriptor : GhostActivityOperationDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.RewardOtherRecoverySource;
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.AfterHpRecovered)];
}

internal sealed class DiscardDeathSourceCardsDescriptor : GhostActivityOperationDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.DiscardDeathSourceCards;
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "zones", "condition");
        var zones = r.RequiredEnumArray<CardZoneKind>("zones");
        if (zones.Count == 0 || zones.Distinct().Count() != zones.Count ||
            zones.Any(zone => zone is not (CardZoneKind.Hand or CardZoneKind.Equipment)))
            throw new InvalidOperationException($"{r.Path}: death-source discard requires distinct hand/equipment areas.");
        var effect = new SkillProgramEffect(Op, FilterBoundCardsProgramOperationDescriptor.Owner(r), 0, r.Condition(), zones: zones);
        RequireAlways(effect, r.Path);
        return effect;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.OwnerDied)];

    public override bool AllowsDeadTarget(in ProgramEffectContinuationContext context) => context.Frame.WindowContext?.Window == SkillProgramTriggerWindow.OwnerDied;
}

internal sealed class SelectRedJudgmentFireTargetDescriptor : GhostActivityOperationDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.SelectRedJudgmentFireTarget;
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.SelectTarget,
        static (effect, context) => context.SelectTarget(effect));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "sourceBind", "condition");
        var effect = new SkillProgramEffect(Op, FilterBoundCardsProgramOperationDescriptor.Owner(r), 0, r.Condition(),
            sourceBind: r.RequiredIdentifier("sourceBind"), targetKind: SkillProgramTargetKind.OtherLiving);
        RequireAlways(effect, r.Path);
        return effect;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new ReadCardSet(effect.SourceBind!), new SelectSingleTarget(), new RequireTriggerWindow(SkillProgramTriggerWindow.CardsMoved)];
}

internal sealed class GhostActivityEffectHandler(SkillProgramEffectOp op) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => op;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host) =>
        host.GetCapability<IGhostActivityProgramHost>().ExecuteGhostActivity(frame, effect);
}

public sealed record RandomJudgmentDiscardedEvent(long ProgramFrameId, int OwnerSeat, int CardId) : IGameEvent;
public sealed record DeathSourceCardsDiscardedEvent(long ProgramFrameId, int OwnerSeat, int SourceSeat, int Count) : IGameEvent;

public sealed partial class GameEngine
{
}
