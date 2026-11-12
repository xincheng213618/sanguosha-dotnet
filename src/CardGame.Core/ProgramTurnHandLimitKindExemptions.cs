namespace CardGame.Core;

public sealed record TurnHandLimitCardKindExemption(long GrantSequence, int TurnNumber, int TurnSeat,
    long ParentFrameId, int EffectIndex, CardUseEffectSource Source, IReadOnlyList<CardKind> CardKinds)
{
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
    public SkillProgramDamageModifierExpiration Expiration { get; init; }
}
public sealed record TurnHandLimitCardKindExemptionGrantedEvent(TurnHandLimitCardKindExemption Policy) : IGameEvent;

public sealed partial record PlayerSnapshot
{
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<TurnHandLimitCardKindExemption>? TurnHandLimitCardKindExemptions { get; init; }
}

internal interface ITurnHandLimitKindExemptionProgramHost
{
    void GrantTurnHandLimitCardKindExemption(ProgramSkillFrame frame, IReadOnlyList<CardKind> cardKinds);
}
internal sealed class GrantTurnHandLimitCardKindExemptionDescriptor : TurnEffectProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.GrantTurnHandLimitCardKindExemption;
    public override ISkillProgramEffectHandler Handler { get; } = new GrantTurnHandLimitCardKindExemptionHandler();
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GrantTurnRuleModifier,
        static (_, context) => context.PublicControlValue(4d));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader)
    {
        reader.AllowOnly("op", "target", "cardKinds", "condition");
        var owner = FilterBoundCardsProgramOperationDescriptor.Owner(reader);
        var kinds = reader.RequiredEnumArray<CardKind>("cardKinds");
        if (kinds.Count == 0)
            throw new InvalidOperationException($"Invalid skill program at {reader.Path}.cardKinds: requires at least one card kind.");
        return new(Op, owner, 0, reader.Condition(), cardKinds: kinds);
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [];
}
public sealed class GrantTurnHandLimitCardKindExemptionHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.GrantTurnHandLimitCardKindExemption;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host)
    {
        host.GetCapability<ITurnHandLimitKindExemptionProgramHost>().GrantTurnHandLimitCardKindExemption(frame, effect.CardKinds);
        return SkillProgramStepOutcome.Continue;
    }
}
