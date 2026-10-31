namespace CardGame.Core;

internal interface IProgramBoundCardCleanup : IProgramRuntimeOperations
{
    void Cleanup(ProgramSkillFrame frame, bool completed);
}

// Writes immutable card-set bindings on the original active program frame.
// Consumers share ProgramBoundCardRuntime's validation and selection provenance.
internal interface IProgramCardSetBindings : IProgramRuntimeOperations
{
    void SetProgramCardSet(long frameId, string bind, IReadOnlyList<int> cardIds,
        SkillProgramCardSetVisibility visibility, IReadOnlyList<CardLocation>? sourceLocations = null,
        Suit? frozenRevealedSuit = null, int? selectionActorSeat = null);
}
