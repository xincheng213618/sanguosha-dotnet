namespace CardGame.Core;

/// <summary>Immutable facts for one actual damage attempt; frame ownership and paid state remain in Core.</summary>
public sealed record ModeDamageFacts(long ResolutionId, int SourceSeat, int TargetSeat, int CardUserSeat,
    CardKind? EffectiveCardKind, DamageNature Nature, bool IsChainPropagation, bool IsSourceLess);

public interface IMatchModeDamageRuleProvider : IGameModeFeature
{
    IReadOnlyList<ModeDamageRuleRegistration> DamageRules { get; }
}

public sealed class ModeDamageRuleRegistration
{
    private ModeDamageRuleRegistration(int order, Func<ModeDamageFacts, int> amount)
    {
        if (order <= 0) throw new ArgumentOutOfRangeException(nameof(order), "A mode damage rule requires a positive explicit order.");
        ArgumentNullException.ThrowIfNull(amount);
        Order = order;
        Amount = amount;
    }
    public int Order { get; }
    internal Func<ModeDamageFacts, int> Amount { get; }
    public static ModeDamageRuleRegistration Additive(int order, Func<ModeDamageFacts, int> amount) => new(order, amount);
}

/// <summary>Nature transformations run at the original native nature boundary, including public Slash traits.</summary>
public interface IMatchModeDamageNaturePolicy : IGameModeFeature
{
    int Order { get; }
    DamageNature TransformDamageNature(DamageNature nature);
}
