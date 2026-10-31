namespace CardGame.Core;

public sealed record CardActionFactionOrigin(int ActualTurnNumber, int ActualTurnOwnerSeat, string? ActorFactionId, string? ProviderFactionId);
public sealed record ProgramFactionRewardOffer(int OwnerSeat, string SkillId);

internal sealed class RewardOutOfTurnFactionSlashDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.RewardOutOfTurnFactionSlash;
    public override ISkillProgramEffectHandler Handler { get; } = new RewardOutOfTurnFactionSlashHandler();
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards, static (e, c) => c.Draw(new(SkillProgramEffectOp.Draw, SkillProgramEffectTarget.Owner, 1, e.Condition)));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "providerFactionId", "condition");
        var effect = new SkillProgramEffect(Op, FilterBoundCardsProgramOperationDescriptor.Owner(r), 1, r.Condition(), providerFactionId: r.RequiredIdentifier("providerFactionId"));
        RequireAlways(effect, r.Path); return effect;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [new RequireTriggerWindows([SkillProgramTriggerWindow.CardUseBeforeTargetEffects, SkillProgramTriggerWindow.CardResponseAccepted])];
}

public sealed class RewardOutOfTurnFactionSlashHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.RewardOutOfTurnFactionSlash;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int seat, ISkillProgramEffectHost host)
    {
        host.Draw(frame.Id, frame.OwnerSeat, frame.OwnerSeat, 1, null, null, SkillProgramCardSetVisibility.Private, new($"skill-program.{frame.SkillId}.{effect.Op}"));
        return SkillProgramStepOutcome.Continue;
    }
}
