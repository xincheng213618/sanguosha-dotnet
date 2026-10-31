#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramDrawDiscardWindowEntryRuntimePortsRuntime
{
    bool global::CardGame.Core.IProgramDrawDiscardWindowEntryFacts.IsGuanduActivity => _coreGuanduActivitiesOperations.IsGuanduActivity;

    bool global::CardGame.Core.IProgramDrawDiscardWindowEntryFacts.HasLivingBindings(SkillProgramTriggerWindow window) => _coreProgramLifecycleOperations.HasLivingProgramTriggerBindings(window);
    bool global::CardGame.Core.IProgramDrawDiscardWindowEntryFacts.HasPendingBoundaryWurongDrawSkip(int ownerSeat) => _characterZhangNiBoundaryMobileOperations.HasPendingBoundaryWurongDrawSkip(ownerSeat);
    bool global::CardGame.Core.IProgramDrawDiscardWindowEntryFacts.HasOutstandingHongjiPhase(CharacterState owner, SkillProgramTriggerWindow window) => _characterZhangShiPingOlOperations.HasOutstandingHongjiPhase(owner, window);
    int global::CardGame.Core.IProgramDrawDiscardWindowEntryFacts.RelativeTableOrder(int from, int target) => _coreTableOrderOperations.RelativeTableOrder(from, target);
    IReadOnlyList<ProgramTriggerCandidate> global::CardGame.Core.IProgramDrawDiscardWindowObservers.IncludeAttributedDrawObservers(CharacterState subject, IReadOnlyList<ProgramTriggerCandidate> existing) => _coreAttributedMarkerEventsOperations.IncludeAttributedDrawObservers(subject, existing);
    IReadOnlyList<ProgramTriggerCandidate> global::CardGame.Core.IProgramDrawDiscardWindowObservers.IncludeYanPuDrawObservers(CharacterState subject, IReadOnlyList<ProgramTriggerCandidate> existing) => _characterYanPuOlOperations.IncludeYanPuDrawObservers(subject, existing);
    IReadOnlyList<ProgramTriggerCandidate> global::CardGame.Core.IProgramDrawDiscardWindowObservers.IncludeShiGyDrawObservers(CharacterState subject, IReadOnlyList<ProgramTriggerCandidate> existing) => _characterGuoYuanMobileShiOperations.IncludeShiGyDrawObservers(subject, existing);
    IReadOnlyList<ProgramTriggerCandidate> global::CardGame.Core.IProgramDrawDiscardWindowObservers.IncludeRecipientCategoryGiftObservers(CharacterState owner, IReadOnlyList<ProgramTriggerCandidate> existing) => _coreRecipientCategoryMarksOperations.IncludeRecipientCategoryGiftObservers(owner, existing);
    IReadOnlyList<ProgramTriggerCandidate> global::CardGame.Core.IProgramDrawDiscardWindowObservers.IncludeHongjiPhaseCandidates(CharacterState owner, SkillProgramTriggerWindow window, IReadOnlyList<ProgramTriggerCandidate> existing) => _characterZhangShiPingOlOperations.IncludeHongjiPhaseCandidates(owner, window, existing);
    IReadOnlyList<ProgramTriggerCandidate> global::CardGame.Core.IProgramDrawDiscardWindowObservers.AppendAnniversaryHanSongDrawEndedCandidates(CharacterState owner, IReadOnlyList<ProgramTriggerCandidate> existing) => _characterHanSongAnniversaryOperations.AppendAnniversaryHanSongDrawEndedCandidates(owner, existing);
}
