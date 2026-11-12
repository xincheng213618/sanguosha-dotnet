using System.Text.Json.Serialization;

namespace CardGame.Core;

public enum NativeSimpleCardCancellationBoundary { EarlySimple, BeforeTargetEffects }

/// <summary>The native continuation boundary and physical cleanup of an already ineffective simple use.</summary>
public sealed record NativeSimpleCardCancellationPolicy
{
    public NativeSimpleCardCancellationPolicy(NativeSimpleCardCancellationBoundary Boundary,
        IReadOnlyList<SimpleCardUseEffect> Effects, CardMoveReason CleanupReason, int Priority = 0)
    {
        ArgumentNullException.ThrowIfNull(Effects);
        if (!Enum.IsDefined(Boundary) || Effects.Count == 0 || Effects.Any(effect => !Enum.IsDefined(effect)) ||
            Effects.Distinct().Count() != Effects.Count || string.IsNullOrWhiteSpace(CleanupReason.Value))
            throw new InvalidOperationException("A simple-use cancellation requires its declared native boundary, effects and cleanup reason.");
        this.Boundary = Boundary;
        this.Effects = Array.AsReadOnly(Effects.ToArray());
        this.CleanupReason = CleanupReason;
        this.Priority = Priority;
    }

    public NativeSimpleCardCancellationBoundary Boundary { get; }
    public IReadOnlyList<SimpleCardUseEffect> Effects { get; }
    public CardMoveReason CleanupReason { get; }
    public int Priority { get; }
}

public sealed record NativeCardUseInvalidationSpec(bool SuppressRemainingCardDamage = false,
    NativeSimpleCardCancellationPolicy? SimpleCancellation = null);

/// <summary>One monotonic effect fact on the actual use, without a second payment or source-qualification ledger.</summary>
public sealed record NativeCardUseInvalidationState(long? ActionId, bool SuppressRemainingCardDamage = false)
{
    private readonly IReadOnlyList<NativeSimpleCardCancellationPolicy> _simpleCancellations = [];
    public IReadOnlyList<NativeSimpleCardCancellationPolicy> SimpleCancellations
    {
        get => _simpleCancellations;
        init => _simpleCancellations = Array.AsReadOnly(value.ToArray());
    }
}

public sealed record NativeCounterspellInvalidation(long ActionId, int ChainDepth);

public sealed partial record CardUseFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public NativeCardUseInvalidationState? NativeUseInvalidation { get; init; }
    private readonly IReadOnlyList<long>? _ineffectiveDodgeActionIds;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<long>? IneffectiveDodgeActionIds
    {
        get => _ineffectiveDodgeActionIds;
        init => _ineffectiveDodgeActionIds = value is null ? null : Array.AsReadOnly(value.ToArray());
    }
}

public sealed partial record NullificationWindowFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public NativeCounterspellInvalidation? CounterspellInvalidation { get; init; }
}

internal interface INativeCardActionInvalidations : IProgramRuntimeOperations
{
    void IssueUse(long useId, CardActionContext? acceptedAction, NativeCardUseInvalidationSpec spec);
    bool IsUseIneffective(long useId);
    bool IsUseIneffective(CardUseFrame use);
    // This permission does not establish whether a particular damage attempt belongs to the use.
    bool SuppressesRemainingCardDamage(long useId);
    void IssueDodge(long useId, CardActionContext acceptedAction);
    bool IsDodgeIneffective(long useId, CardActionContext acceptedAction);
    void IssueCounterspell(long windowId, CardActionContext acceptedAction);
    bool IsCounterspellIneffective(NullificationWindowFrame window, CardActionContext acceptedAction);
    NativeSimpleCardCancellationPolicy? TryReadSimpleCancellation(long useId, SimpleCardUseEffect effect,
        NativeSimpleCardCancellationBoundary boundary);
}
