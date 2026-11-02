namespace CardGame.Core.EngineOwners;

internal sealed partial class CoreDirectedTurnCardPoliciesRuntime
{
    private readonly IReadOnlyList<DirectedTurnCardPolicy> EmptyDirectedSnapshot =
        Array.AsReadOnly(Array.Empty<DirectedTurnCardPolicy>());

    internal static bool DeclarationIsTrueUse(CardDeclarationParentReturn parent) => parent.Purpose is
        CardDeclarationPurpose.Use or CardDeclarationPurpose.Dodge or CardDeclarationPurpose.Counterspell or
        CardDeclarationPurpose.Recovery or CardDeclarationPurpose.BorrowedSword or CardDeclarationPurpose.ProvidedSlash or
        CardDeclarationPurpose.BorrowedSwordProvidedSlash or CardDeclarationPurpose.QinglongProvidedSlash;

    internal DeclaredCardPayment CaptureDirectedDeclarationPayment(DeclaredCardPayment paid, CardDeclarationParentReturn parent)
    {
        if (paid.DirectedPolicies is not null || paid.Claimed || paid.ActorSeat != parent.ActorSeat ||
            !DeclarationIsTrueUse(parent) || _cardZones.GetLocation(paid.Cost.CardId) != paid.Cost.From)
            return paid;
        return paid with { DirectedPolicies = CaptureDirectedPolicies(parent.ActorSeat) };
    }

    internal void IssueDirectedUse(CardUseFrame use)
    {
        if (_cardUses.LifecycleCardUse(use.Id) is not { DirectedPolicies: null, Action: { Type: CardActionType.Use } action } current)
            return;
        var paid = current.AcceptedDeclarationPayment;
        if (paid is null && action.PhysicalCards is [var cost])
            paid = _coreCardDeclarationsOperations.UnclaimedDeclarationPayment(action.ProviderSeat, cost.CardId);
        var snapshot = paid is not null && paid.ActorSeat == action.ActorSeat
            ? paid.DirectedPolicies ?? EmptyDirectedSnapshot
            : CaptureDirectedPolicies(action.ActorSeat);
        _runtime.ReplaceRuntimeFrame(current.Id, current with { DirectedPolicies = snapshot });
    }

    internal IReadOnlyList<DirectedTurnCardPolicy>? GetDirectedPolicySnapshot(int actor, long? useId = null)
    {
        if (useId is { } id)
            return _cardUses.LifecycleCardUse(id) is { Action: { Type: CardActionType.Use } action } use &&
                use.SourceSeat == actor && action.ActorSeat == actor ? use.DirectedPolicies ?? EmptyDirectedSnapshot : EmptyDirectedSnapshot;

        var current = _resolutionStackOperations._resolutionStack.LastOrDefault();
        if (current is CardDeclarationFrame { Stage: CardDeclarationStage.Applying } declaration &&
            declaration.Return.ActorSeat == actor && DeclarationIsTrueUse(declaration.Return))
            return declaration.Payment.DirectedPolicies ?? EmptyDirectedSnapshot;
        if (current is ResponseWindowFrame response)
            current = _resolutionStackOperations._resolutionStack.SingleOrDefault(frame => frame.Id == response.ParentFrameId);
        if (current?.AcceptedDeclarationPayment is { Claimed: false } paid && paid.ActorSeat == actor &&
            _cardZones.GetLocation(paid.Cost.CardId) == CardLocation.Processing)
            return paid.DirectedPolicies ?? EmptyDirectedSnapshot;
        return null;
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 28620)]
    internal void CaptureDirectedZeroMaterialUse(IGameEvent payload)
    {
        if (payload is CardUseDeclaredEvent { CardId: 0 } declared &&
            _cardUses.LifecycleCardUse(declared.ResolutionId) is
                { DirectedPolicies: null, CardId: 0, Action: { Type: CardActionType.Use, PhysicalCards.Count: 0 } } use &&
            use.PhysicalCardIds is null or { Count: 0 } && use.SourceSeat == declared.SourceSeat && use.CardKind == declared.CardKind)
            IssueDirectedUse(use);
    }
}
