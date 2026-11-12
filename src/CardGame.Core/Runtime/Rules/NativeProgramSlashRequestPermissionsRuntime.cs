namespace CardGame.Core;

internal interface INativeProgramSlashRequestPermissions : IProgramSelectionQuery
{
    ProgramSlashRequestIdentity? Declaration(int actorSeat);
}

// Prospective permissions belong to the actual declaration window. Once issued,
// the original native use owns its captured rights, independently of its source.
[ProgramRuntimeModule, ProgramCapability]
internal sealed class NativeProgramSlashRequestPermissionsRuntime(IProgramRuntimeFrames frames,
    INativeProgramSlashRequests requests) : INativeProgramSlashRequestPermissions
{
    public ProgramSlashRequestIdentity? Declaration(int actorSeat)
    {
        if (frames.Top is ProgramSkillFrame owner)
        {
            if (requests.GetOffer(owner, actorSeat) is { } offer) return offer;
            return owner.SlashRequest is { Closed: false, ExecutionFrameId: null, Acceptance: { } accepted } &&
                accepted.Identity.ActorSeat == actorSeat && requests.CanExecuteAcceptedUse(owner, accepted)
                    ? accepted.Identity : null;
        }
        if (frames.Top is CardDeclarationFrame { Return.ProgramSlashRequest: { } declaration } &&
            frames.Find(declaration.Identity.FrameId) is ProgramSkillFrame original &&
            declaration.Identity.ActorSeat == actorSeat && requests.CanExecuteAcceptedUse(original, declaration))
            return declaration.Identity;
        return null;
    }

    [ProgramDistanceRule(ProgramDistanceStage.PhysicalSlash, 900009)]
    internal bool RequestedPhysicalSlashDistance(ProgramSlashDistanceQuery query) =>
        Distance(query.Actor.Seat, query.Target.Seat, query.Kind, query.ExistingUseFrameId, true);

    [ProgramDistanceRule(ProgramDistanceStage.VirtualSlash, 900009)]
    [ProgramDistanceRule(ProgramDistanceStage.SpecificSlashExemption, 900009)]
    internal bool RequestedSlashDistance(ProgramSlashDistanceQuery query) =>
        Distance(query.Actor.Seat, query.Target.Seat, query.Kind, query.ExistingUseFrameId, false);

    [ProgramDistanceRule(ProgramDistanceStage.CardUseExemption, 900009)]
    internal bool RequestedCardDistance(ProgramCardDistanceQuery query) =>
        Distance(query.Actor.Seat, query.Target.Seat, query.Kind, query.CardUseFrameId, false);

    private bool Distance(int actor, int target, CardKind kind, long? useId, bool physical)
    {
        if (!EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind)) return false;
        if (useId is { } id)
            return physical && frames.Find(id) is CardUseFrame { ProgramSlashRequestExecution: { } execution } &&
                execution.Acceptance.Identity.ActorSeat == actor && execution.Acceptance.Identity.Spec.IgnoreDistance &&
                execution.Acceptance.Identity.Spec.IssuedPhysicalSlashDistanceTargets is { } scope &&
                Matches(scope, actor, target, execution.Acceptance.TargetSeat);
        return Declaration(actor) is { Spec.IgnoreDistance: true } declaration &&
            frames.Find(declaration.FrameId) is ProgramSkillFrame owner && requests.GetTargets(owner, actor).Contains(target);
    }

    [ProgramTargetRule(ProgramTargetRulePoint.SlashQuotaBeforeLimit, 900009)]
    internal bool RequestedSlashQuota(ProgramSlashQuotaQuery query)
    {
        if (!EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(query.Kind) ||
            Declaration(query.Actor.Seat) is not { Spec.CountsTowardSlashLimit: false } declaration)
            return false;
        return declaration.Spec.QuotaTargets == ProgramSlashRequestPermissionTargets.AnyOtherTarget
            ? query.Actor.Seat != query.Target.Seat
            : frames.Find(declaration.FrameId) is ProgramSkillFrame owner &&
                requests.GetTargets(owner, query.Actor.Seat).Contains(query.Target.Seat);
    }

    [ProgramCardUsePayment(ProgramCardUsePaymentStage.BeforeSlashDebit, 900009)]
    internal bool IssuedSlashDebitExemption(long useId, int actorSeat) =>
        frames.Find(useId) is CardUseFrame { ProgramSlashRequestExecution: { } execution } &&
        execution.Acceptance.Identity.ActorSeat == actorSeat && execution.Acceptance.Identity.Spec.ExemptIssuedSlashDebit;

    private static bool Matches(ProgramSlashRequestPermissionTargets scope, int actor, int target, int requestedTarget) =>
        scope == ProgramSlashRequestPermissionTargets.AnyOtherTarget ? actor != target : target == requestedTarget;
}
