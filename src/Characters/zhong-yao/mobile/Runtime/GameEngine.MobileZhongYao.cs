namespace CardGame.Core;

internal interface IMobileZhongYaoSelectionQueries : IProgramSelectionQuery
{
    IReadOnlyList<int> EligibleTargets(int ownerSeat);
}

[ProgramTargetResolver(SkillProgramTargetKind.MobileZhongYaoZuodingTarget)]
internal sealed class MobileZhongYaoZuodingTargetResolver : IProgramTargetResolver
{
    public IReadOnlyList<int> Resolve(ProgramTargetResolverContext context, IProgramSelectionQueries queries) =>
        queries.Get<IMobileZhongYaoSelectionQueries>().EligibleTargets(context.OwnerSeat);
}

public sealed partial class GameEngine
{
    private IReadOnlyList<int> MobileZhongYaoZuodingTargets(int ownerSeat, ProgramSkillWindowContext? context)
    {
        if (context is not { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, CardUse: { } }) return [];
        var damaged = EventsSinceLastBoundary(item => item is TurnStartedEvent or PhaseChangedEvent { Phase: TurnPhase.Play })
            .OfType<DamageAppliedEvent>().Where(item => item.Amount > 0).Select(item => item.TargetSeat).ToHashSet();
        return Array.AsReadOnly(GetProgramTargetSeats(ownerSeat, SkillProgramTargetKind.CurrentCardUseTargets, context)
            .Where(seat => !damaged.Contains(ownerSeat) || !damaged.Contains(seat)).ToArray());
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 77700, SkillProgramEffectOp.SelectTarget)]
    private bool CanOfferMobileZhongYaoZuoding(ProgramTriggerLegalityContext gate) =>
        gate.SkillId != "mobile:zuoding" || MobileZhongYaoZuodingTargets(gate.OwnerSeat, gate.Context).Count > 0;

    [ProgramCapability]
    private sealed class MobileZhongYaoSelectionQueries(GameEngine engine) : IMobileZhongYaoSelectionQueries
    {
        public IReadOnlyList<int> EligibleTargets(int ownerSeat) => engine.MobileZhongYaoZuodingTargets(ownerSeat,
            engine._resolutionStack.LastOrDefault() switch
            {
                ProgramSkillFrame frame => frame.WindowContext,
                ProgramCardTriggerWindowFrame window when window.CandidateIndex < window.Candidates.Count =>
                    engine.CreateCardActionProgramContext(window, window.Candidates[window.CandidateIndex]),
                _ => null
            });
    }
}
