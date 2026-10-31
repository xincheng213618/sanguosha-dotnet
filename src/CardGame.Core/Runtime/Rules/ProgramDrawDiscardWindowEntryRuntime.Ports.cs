namespace CardGame.Core;

internal interface IProgramDrawDiscardWindowEntryFacts : IProgramSelectionQuery
{
    bool IsGuanduActivity { get; }
    bool HasLivingBindings(SkillProgramTriggerWindow window);
    bool HasPendingBoundaryWurongDrawSkip(int ownerSeat);
    bool HasOutstandingHongjiPhase(CharacterState owner, SkillProgramTriggerWindow window);
    int RelativeTableOrder(int from, int target);
}

// These producers retain their issued receipts and rule-specific qualification.
// The entry runtime owns their order, candidate preparation and frame issuance.
internal interface IProgramDrawDiscardWindowObservers : IProgramSelectionQuery
{
    IReadOnlyList<ProgramTriggerCandidate> IncludeAttributedDrawObservers(CharacterState subject, IReadOnlyList<ProgramTriggerCandidate> existing);
    IReadOnlyList<ProgramTriggerCandidate> IncludeYanPuDrawObservers(CharacterState subject, IReadOnlyList<ProgramTriggerCandidate> existing);
    IReadOnlyList<ProgramTriggerCandidate> IncludeShiGyDrawObservers(CharacterState subject, IReadOnlyList<ProgramTriggerCandidate> existing);
    IReadOnlyList<ProgramTriggerCandidate> IncludeRecipientCategoryGiftObservers(CharacterState owner, IReadOnlyList<ProgramTriggerCandidate> existing);
    IReadOnlyList<ProgramTriggerCandidate> IncludeHongjiPhaseCandidates(CharacterState owner, SkillProgramTriggerWindow window, IReadOnlyList<ProgramTriggerCandidate> existing);
    IReadOnlyList<ProgramTriggerCandidate> AppendAnniversaryHanSongDrawEndedCandidates(CharacterState owner, IReadOnlyList<ProgramTriggerCandidate> existing);
}

public sealed partial class GameEngine
{
}
