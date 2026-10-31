#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramDrawDiscardWindowEntryRuntimePortsRuntime
{
    bool IProgramDrawDiscardWindowEntryFacts.IsGuanduActivity => _coreGuanduActivitiesOperations.IsGuanduActivity;

    bool IProgramDrawDiscardWindowEntryFacts.HasLivingBindings(SkillProgramTriggerWindow window) => _coreProgramLifecycleOperations.HasLivingProgramTriggerBindings(window);
    bool IProgramDrawDiscardWindowEntryFacts.HasPendingBoundaryWurongDrawSkip(int ownerSeat) => _characterZhangNiBoundaryMobileOperations.HasPendingBoundaryWurongDrawSkip(ownerSeat);
    bool IProgramDrawDiscardWindowEntryFacts.HasOutstandingHongjiPhase(CharacterState owner, SkillProgramTriggerWindow window) => _characterZhangShiPingOlOperations.HasOutstandingHongjiPhase(owner, window);
    int IProgramDrawDiscardWindowEntryFacts.RelativeTableOrder(int from, int target) => _coreTableOrderOperations.RelativeTableOrder(from, target);
    IReadOnlyList<ProgramTriggerCandidate> IProgramDrawDiscardWindowObservers.IncludeAttributedDrawObservers(CharacterState subject, IReadOnlyList<ProgramTriggerCandidate> existing) => _coreAttributedMarkerEventsOperations.IncludeAttributedDrawObservers(subject, existing);
    IReadOnlyList<ProgramTriggerCandidate> IProgramDrawDiscardWindowObservers.IncludeYanPuDrawObservers(CharacterState subject, IReadOnlyList<ProgramTriggerCandidate> existing) => _characterYanPuOlOperations.IncludeYanPuDrawObservers(subject, existing);
    IReadOnlyList<ProgramTriggerCandidate> IProgramDrawDiscardWindowObservers.IncludeShiGyDrawObservers(CharacterState subject, IReadOnlyList<ProgramTriggerCandidate> existing) => _characterGuoYuanMobileShiOperations.IncludeShiGyDrawObservers(subject, existing);
    IReadOnlyList<ProgramTriggerCandidate> IProgramDrawDiscardWindowObservers.IncludeRecipientCategoryGiftObservers(CharacterState owner, IReadOnlyList<ProgramTriggerCandidate> existing) => _coreRecipientCategoryMarksOperations.IncludeRecipientCategoryGiftObservers(owner, existing);
    IReadOnlyList<ProgramTriggerCandidate> IProgramDrawDiscardWindowObservers.IncludeHongjiPhaseCandidates(CharacterState owner, SkillProgramTriggerWindow window, IReadOnlyList<ProgramTriggerCandidate> existing) => _characterZhangShiPingOlOperations.IncludeHongjiPhaseCandidates(owner, window, existing);
    IReadOnlyList<ProgramTriggerCandidate> IProgramDrawDiscardWindowObservers.AppendAnniversaryHanSongDrawEndedCandidates(CharacterState owner, IReadOnlyList<ProgramTriggerCandidate> existing) => _characterHanSongAnniversaryOperations.AppendAnniversaryHanSongDrawEndedCandidates(owner, existing);
}
