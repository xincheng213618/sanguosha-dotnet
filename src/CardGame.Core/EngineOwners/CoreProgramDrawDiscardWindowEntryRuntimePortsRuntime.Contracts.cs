#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramDrawDiscardWindowEntryRuntimePortsCoreTableOrderOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int RelativeTableOrder(int from, int target);
}

internal interface ICoreProgramDrawDiscardWindowEntryRuntimePortsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasLivingProgramTriggerBindings(global::CardGame.Core.SkillProgramTriggerWindow window);
}

internal interface ICoreProgramDrawDiscardWindowEntryRuntimePortsCharacterZhangShiPingOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasOutstandingHongjiPhase(global::CardGame.Core.CharacterState recipient, global::CardGame.Core.SkillProgramTriggerWindow window);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> IncludeHongjiPhaseCandidates(global::CardGame.Core.CharacterState turnOwner, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> candidates);
}

internal interface ICoreProgramDrawDiscardWindowEntryRuntimePortsCharacterGuoYuanMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> IncludeShiGyDrawObservers(global::CardGame.Core.CharacterState subject, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> existing);
}

internal interface ICoreProgramDrawDiscardWindowEntryRuntimePortsCharacterHanSongAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> AppendAnniversaryHanSongDrawEndedCandidates(global::CardGame.Core.CharacterState phaseOwner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> existing);
}

internal interface ICoreProgramDrawDiscardWindowEntryRuntimePortsCoreRecipientCategoryMarksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> IncludeRecipientCategoryGiftObservers(global::CardGame.Core.CharacterState drawOwner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> existing);
}

internal interface ICoreProgramDrawDiscardWindowEntryRuntimePortsCoreGuanduActivitiesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsGuanduActivity { get; }
}

internal interface ICoreProgramDrawDiscardWindowEntryRuntimePortsCharacterYanPuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> IncludeYanPuDrawObservers(global::CardGame.Core.CharacterState subject, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> existing);
}

internal interface ICoreProgramDrawDiscardWindowEntryRuntimePortsCoreAttributedMarkerEventsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> IncludeAttributedDrawObservers(global::CardGame.Core.CharacterState subject, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> existing);
}

internal interface ICoreProgramDrawDiscardWindowEntryRuntimePortsCharacterZhangNiBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasPendingBoundaryWurongDrawSkip(int target);
}
