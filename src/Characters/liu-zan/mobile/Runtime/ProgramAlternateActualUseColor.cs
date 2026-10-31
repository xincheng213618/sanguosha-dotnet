namespace CardGame.Core;

// Scalar facts need no mutable collection projection or pending sidecar.
public sealed record AlternateActualUseColorRecordedEvent(
    int ActualTurn, int TurnOwnerSeat, int ActorSeat, long ActionId,
    long? PreviousActionId) : IGameEvent
{
    // This remains trusted rules state, like the accepted action's hidden cost.
    // Command replay reconstructs it; JSON and public event members do not expose it.
    [System.Text.Json.Serialization.JsonIgnore]
    internal bool? IsRed { get; init; }
    public override string ToString() => $"Actual use {ActionId}, owner {ActorSeat}, turn {ActualTurn}, previous {PreviousActionId}";
}
public sealed record AlternateActualUseColorDrawGrantedEvent(
    int ActualTurn, int OwnerSeat, long ActionId, long PreviousActionId,
    string SkillId, string SkillInstanceId) : IGameEvent;

internal interface IAlternateActualUseColorProgramHost
{
    void GrantAlternateActualUseColorDraw(ProgramSkillFrame frame);
}

internal sealed class GrantAlternateActualUseColorDrawDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.GrantAlternateActualUseColorDraw;
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override ISkillProgramEffectHandler Handler { get; } = new GrantAlternateActualUseColorDrawHandler();
    public override ProgramContextCapability RequiredCapabilities => ProgramContextCapability.CardAction;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(
        ProgramOperationAiSemantic.GainCards, static (_, _) => { });
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.CardUseCommitted), new RequireCardActionActor()];
}

internal sealed class GrantAlternateActualUseColorDrawHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.GrantAlternateActualUseColorDraw;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame,
        int targetSeat, ISkillProgramEffectHost host)
    {
        host.GetCapability<IAlternateActualUseColorProgramHost>().GrantAlternateActualUseColorDraw(frame);
        return SkillProgramStepOutcome.Continue;
    }
}

internal sealed class MobileFenyinCommittedUseContract : IProgramCommittedUseContract
{
    public string SkillId => "mobile:fenyin";
    public void ValidateTrigger(SkillProgramTrigger trigger)
    {
        if (trigger.Window != SkillProgramTriggerWindow.CardUseCommitted || trigger.Optional ||
            trigger.OwnerRelation != SkillProgramCardActionOwnerRelation.Actor || !trigger.IncludeResponseUses ||
            trigger.Subject is not null || trigger.SourceSkillId is not null || trigger.SourceViewAsId is not null ||
            trigger.Condition.Kind != SkillProgramTriggerConditionKind.OwnerIsTurnPlayer ||
            trigger.UsageScope is not null || trigger.UsageLimit is not null || trigger.DynamicUsageLimit is not null ||
            trigger.NamedUsageGroup is not null || trigger.MarkerCost is not null ||
            trigger.CardKinds.Count != 0 || trigger.CardCategories.Count != 0 || trigger.RequireNoCardConversion is not null ||
            trigger.Effects is not [
                { Op: SkillProgramEffectOp.GrantAlternateActualUseColorDraw, Target: SkillProgramEffectTarget.Owner,
                    Condition.Kind: SkillProgramConditionKind.Always },
                { Op: SkillProgramEffectOp.Draw, Target: SkillProgramEffectTarget.Owner, Amount: 1,
                    NumberExpression: null, SourceBind: null, ResultBind: null, TargetReference: null,
                    Condition.Kind: SkillProgramConditionKind.Always }])
            throw new InvalidOperationException("Mobile Fenyin requires its own-turn mandatory committed actual-use grant followed by exactly one ordinary owner draw.");
    }
}
