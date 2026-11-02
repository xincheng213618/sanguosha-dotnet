namespace CardGame.Core;

internal interface INativeFactDemand : IProgramSelectionQuery
{
    bool RequiresFact(ProgramTrackedFact fact);
}

// Immutable demand for this match. Catalogs may be shared by different modes;
// the selected mode's requirements must never mutate the catalog's facts.
internal sealed class MatchNativeFactDemand : INativeFactDemand
{
    private readonly ProgramTrackedFact _facts;

    internal MatchNativeFactDemand(ProgramTrackedFact facts)
    {
        if (!new ProgramFactRequirements(Triggered: facts).IsValid)
            throw new ArgumentOutOfRangeException(nameof(facts), "Unknown native fact requirement.");
        _facts = ProgramFactRequirements.Close(facts);
    }

    public bool RequiresFact(ProgramTrackedFact fact) => (_facts & fact) != ProgramTrackedFact.None;
}
