namespace CardGame.Core;

// Candidate qualification and trigger snapshots still use the shared legacy
// trigger owner. Character runtimes depend on this narrow read contract rather
// than receiving a second character-specific engine adapter.
internal interface IProgramTriggerQueries : IProgramSelectionQuery
{
    SkillProgramTriggerFacts CaptureProgramTriggerFacts(int ownerSeat);
    IReadOnlyList<ProgramTriggerCandidate> CollectProgramTriggerCandidates(int ownerSeat, SkillProgramTriggerWindow window);
}
