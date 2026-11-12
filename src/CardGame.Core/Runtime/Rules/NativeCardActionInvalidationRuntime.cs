namespace CardGame.Core;

// Qualification, payment and status consumption stay at their actual producers.
// These writes retain only the effect on the original native owner; reads never requalify its source.
[ProgramRuntimeModule, ProgramCapability]
internal sealed class NativeCardActionInvalidationRuntime(IProgramRuntimeFrames frames,
    INativeCardUseAdmission admission) : INativeCardActionInvalidations
{
    public void IssueUse(long useId, CardActionContext? acceptedAction, NativeCardUseInvalidationSpec spec)
    {
        ArgumentNullException.ThrowIfNull(spec);
        var use = Use(useId);
        if (acceptedAction is null
                ? use.Action is not null || !admission.IsLegacyProducerUse(use)
                : acceptedAction.Type != CardActionType.Use || acceptedAction.ActionId <= 0 ||
                    use.Action is not { Type: CardActionType.Use } action || action.ActionId != acceptedAction.ActionId)
            throw new InvalidOperationException("An ineffective use requires its actual accepted action or admitted actionless native producer.");
        var previous = Read(use);
        var policy = spec.SimpleCancellation;
        var addPolicy = policy is not null && previous?.SimpleCancellations.Any(existing => SamePolicy(existing, policy)) != true;
        if (previous is not null && (!spec.SuppressRemainingCardDamage || previous.SuppressRemainingCardDamage) && !addPolicy)
            return;
        var state = previous ?? new NativeCardUseInvalidationState(acceptedAction?.ActionId);
        frames.Replace(useId, use with { NativeUseInvalidation = state with
        {
            SuppressRemainingCardDamage = state.SuppressRemainingCardDamage || spec.SuppressRemainingCardDamage,
            SimpleCancellations = addPolicy ? [.. state.SimpleCancellations, policy!] : state.SimpleCancellations
        } });
    }

    public bool IsUseIneffective(long useId) => frames.Find(useId) is CardUseFrame use && IsUseIneffective(use);
    public bool IsUseIneffective(CardUseFrame use) => Read(use) is not null;
    public bool SuppressesRemainingCardDamage(long useId) =>
        frames.Find(useId) is CardUseFrame use && Read(use)?.SuppressRemainingCardDamage == true;

    public void IssueDodge(long useId, CardActionContext acceptedAction)
    {
        var use = Use(useId);
        if (!OwnsDodge(use, acceptedAction))
            throw new InvalidOperationException("An ineffective Dodge requires its actual incoming use, response action and current native defender.");
        if (use.IneffectiveDodgeActionIds?.Contains(acceptedAction.ActionId) == true) return;
        frames.Replace(useId, use with { IneffectiveDodgeActionIds = [.. use.IneffectiveDodgeActionIds ?? [], acceptedAction.ActionId] });
    }

    public bool IsDodgeIneffective(long useId, CardActionContext acceptedAction) =>
        frames.Find(useId) is CardUseFrame use && OwnsDodge(use, acceptedAction) &&
        use.IneffectiveDodgeActionIds?.Contains(acceptedAction.ActionId) == true;

    public void IssueCounterspell(long windowId, CardActionContext acceptedAction)
    {
        if (frames.Find(windowId) is not NullificationWindowFrame window || !OwnsCounterspell(window, acceptedAction))
            throw new InvalidOperationException("An ineffective counterspell requires its actual accepted native response node and depth.");
        if (window.CounterspellInvalidation is { } previous && previous.ActionId == acceptedAction.ActionId &&
            previous.ChainDepth == window.ChainDepth) return;
        frames.Replace(windowId, window with { CounterspellInvalidation = new(acceptedAction.ActionId, window.ChainDepth) });
    }

    public bool IsCounterspellIneffective(NullificationWindowFrame window, CardActionContext acceptedAction) =>
        OwnsCounterspell(window, acceptedAction) && window.CounterspellInvalidation is { } receipt &&
        receipt.ActionId == acceptedAction.ActionId && receipt.ChainDepth == window.ChainDepth;

    public NativeSimpleCardCancellationPolicy? TryReadSimpleCancellation(long useId, SimpleCardUseEffect effect,
        NativeSimpleCardCancellationBoundary boundary)
    {
        if (frames.Find(useId) is not CardUseFrame use || Read(use) is not { } state) return null;
        NativeSimpleCardCancellationPolicy? selected = null;
        foreach (var policy in state.SimpleCancellations)
            if (policy.Boundary == boundary && policy.Effects.Contains(effect) &&
                (selected is null || policy.Priority < selected.Priority))
                selected = policy;
        return selected;
    }

    private CardUseFrame Use(long id) => frames.Find(id) as CardUseFrame ??
        throw new InvalidOperationException("Card-action invalidation lost its original native use.");

    private static NativeCardUseInvalidationState? Read(CardUseFrame use)
    {
        var state = use.NativeUseInvalidation;
        if (state is not null && state.ActionId != use.Action?.ActionId)
            throw new InvalidOperationException("An ineffective use cannot change its original native action identity.");
        return state;
    }

    private static bool SamePolicy(NativeSimpleCardCancellationPolicy a, NativeSimpleCardCancellationPolicy b) =>
        a.Boundary == b.Boundary && a.Priority == b.Priority && a.CleanupReason == b.CleanupReason && a.Effects.SequenceEqual(b.Effects);

    private static bool OwnsDodge(CardUseFrame use, CardActionContext action) =>
        action.ActionId > 0 && action.Type == CardActionType.Response && action.EffectiveKind == CardKind.Dodge &&
        // Supplied native responses may retain the provider as ResponderSeat;
        // the actual defender is the accepted actor on this incoming attack.
        action.ParentActionId == use.Action?.ActionId &&
        use.CardAttack is { } attack && attack.TargetSeat == action.ActorSeat;

    private static bool OwnsCounterspell(NullificationWindowFrame window, CardActionContext action) =>
        action.ActionId > 0 && action.Type == CardActionType.Response && action.EffectiveKind == CardKind.Nullification &&
        action.ResponderSeat == action.ActorSeat && window.ChainDepth > 0 &&
        window.ResponseRuleNode is { } node && node.ChainDepth == window.ChainDepth && node.Action.ActionId == action.ActionId &&
        window.NamedAmbushResponse is { } response && response.ActionId == action.ActionId && response.ResponderSeat == action.ActorSeat &&
        response.PreviousChainDepth + 1 == window.ChainDepth;
}
