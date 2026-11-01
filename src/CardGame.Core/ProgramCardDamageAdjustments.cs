using System.Text.Json.Serialization;

namespace CardGame.Core;

public enum ProgramCardDamageTargetScope { Use, CurrentTarget }

public sealed record CardDamageAdjustment
{
    public int Amount { get; }
    public int? TargetSeat { get; }
    public IReadOnlyList<CardKind> CardKinds { get; }
    public SkillProgramDamageLayer DamageLayer { get; }
    public CardDamageAdjustment(int amount, int? targetSeat = null, IReadOnlyList<CardKind>? cardKinds = null,
        SkillProgramDamageLayer damageLayer = SkillProgramDamageLayer.AdditionalDamage) =>
        (Amount, TargetSeat, CardKinds, DamageLayer) = (amount, targetSeat,
            Array.AsReadOnly((cardKinds ?? []).ToArray()), damageLayer);
}

public sealed partial record CardUseFrame
{
    private readonly IReadOnlyList<CardDamageAdjustment>? _cardDamageAdjustments;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<CardDamageAdjustment>? CardDamageAdjustments
    {
        get => _cardDamageAdjustments;
        init => _cardDamageAdjustments = value is null ? null : Array.AsReadOnly(value.ToArray());
    }
}

internal interface ICurrentCardDamageProgramHost
{
    void AddCurrentCardDamage(ProgramSkillFrame frame, SkillProgramEffect effect, int amount);
}

internal sealed class AddCurrentCardDamageDescriptor : ProgramOperationDescriptorBase
{
    internal static bool HasTargetEffects(IEnumerable<SkillProgramEffect> effects) => effects.Any(effect =>
        effect.Op == SkillProgramEffectOp.AddCurrentCardDamage && effect.CardDamageTargetScope == ProgramCardDamageTargetScope.CurrentTarget);
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.AddCurrentCardDamage;
    public override ISkillProgramEffectHandler Handler { get; } = new AddCurrentCardDamageHandler();
    public override ProgramContextCapability RequiredCapabilities => ProgramContextCapability.CardAction;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GrantTurnCardDamageModifier,
        static (effect, context) => context.CurrentCardDamageValue(effect));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "targetScope", "amount", "amountExpr", "onUnknownAmount", "cardKinds", "damageLayer", "condition");
        var expression = r.AmountExpression();
        return new(Op, FilterBoundCardsProgramOperationDescriptor.Owner(r),
            expression is null ? DrawProgramOperationDescriptor.Amount(r, 20) : 0, r.Condition(),
            cardKinds: r.OptionalEnumArray<CardKind>("cardKinds"))
        {
            CardDamageTargetScope = r.Has("targetScope") ? r.RequiredEnum<ProgramCardDamageTargetScope>("targetScope") : ProgramCardDamageTargetScope.Use,
            CardDamageLayer = r.Has("damageLayer") ? r.RequiredEnum<SkillProgramDamageLayer>("damageLayer") : SkillProgramDamageLayer.AdditionalDamage,
            AmountExpression = expression, OnUnknownAmount = r.UnknownAmountBehavior()
        };
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireContext(ProgramContextCapability.CardAction), new RequireTriggerWindows(
            effect.CardDamageTargetScope == ProgramCardDamageTargetScope.CurrentTarget
                ? [SkillProgramTriggerWindow.CardUseTargetsFinalized, SkillProgramTriggerWindow.SlashBeforeResponse]
                : [SkillProgramTriggerWindow.CardUseCommitted, SkillProgramTriggerWindow.CardUseTargetsFinalized, SkillProgramTriggerWindow.SlashBeforeResponse])];
}

internal sealed class AddCurrentCardDamageHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.AddCurrentCardDamage;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host)
    {
        var amount = effect.AmountExpression is { } expression
            ? host.EvaluateAmountExpression(frame, expression, effect.OnUnknownAmount) : effect.Amount;
        if (amount is { } value) host.GetCapability<ICurrentCardDamageProgramHost>().AddCurrentCardDamage(frame, effect, value);
        return SkillProgramStepOutcome.Continue;
    }
}
