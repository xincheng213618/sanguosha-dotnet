#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime))]
internal sealed class CoreProgramDrawDiscardWindowEntryRuntimePortsCoreTableOrderAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDrawDiscardWindowEntryRuntimePortsCoreTableOrderOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime> _targetFactory = targetFactory;
    public int RelativeTableOrder(int from, int target) => this._targetFactory().RelativeTableOrder(from, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreProgramDrawDiscardWindowEntryRuntimePortsCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDrawDiscardWindowEntryRuntimePortsCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public bool HasLivingProgramTriggerBindings(global::CardGame.Core.SkillProgramTriggerWindow window) => this._targetFactory().HasLivingProgramTriggerBindings(window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangShiPingOlRuntime))]
internal sealed class CoreProgramDrawDiscardWindowEntryRuntimePortsCharacterZhangShiPingOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangShiPingOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDrawDiscardWindowEntryRuntimePortsCharacterZhangShiPingOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangShiPingOlRuntime> _targetFactory = targetFactory;
    public bool HasOutstandingHongjiPhase(global::CardGame.Core.CharacterState recipient, global::CardGame.Core.SkillProgramTriggerWindow window) => this._targetFactory().HasOutstandingHongjiPhase(recipient, window);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> IncludeHongjiPhaseCandidates(global::CardGame.Core.CharacterState turnOwner, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> candidates) => this._targetFactory().IncludeHongjiPhaseCandidates(turnOwner, window, candidates);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuoYuanMobileShiRuntime))]
internal sealed class CoreProgramDrawDiscardWindowEntryRuntimePortsCharacterGuoYuanMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoYuanMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDrawDiscardWindowEntryRuntimePortsCharacterGuoYuanMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoYuanMobileShiRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> IncludeShiGyDrawObservers(global::CardGame.Core.CharacterState subject, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> existing) => this._targetFactory().IncludeShiGyDrawObservers(subject, existing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHanSongAnniversaryRuntime))]
internal sealed class CoreProgramDrawDiscardWindowEntryRuntimePortsCharacterHanSongAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHanSongAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDrawDiscardWindowEntryRuntimePortsCharacterHanSongAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHanSongAnniversaryRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> AppendAnniversaryHanSongDrawEndedCandidates(global::CardGame.Core.CharacterState phaseOwner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> existing) => this._targetFactory().AppendAnniversaryHanSongDrawEndedCandidates(phaseOwner, existing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRecipientCategoryMarksRuntime))]
internal sealed class CoreProgramDrawDiscardWindowEntryRuntimePortsCoreRecipientCategoryMarksAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientCategoryMarksRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDrawDiscardWindowEntryRuntimePortsCoreRecipientCategoryMarksOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientCategoryMarksRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> IncludeRecipientCategoryGiftObservers(global::CardGame.Core.CharacterState drawOwner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> existing) => this._targetFactory().IncludeRecipientCategoryGiftObservers(drawOwner, existing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGuanduActivitiesRuntime))]
internal sealed class CoreProgramDrawDiscardWindowEntryRuntimePortsCoreGuanduActivitiesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduActivitiesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDrawDiscardWindowEntryRuntimePortsCoreGuanduActivitiesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduActivitiesRuntime> _targetFactory = targetFactory;
    public bool IsGuanduActivity { get => this._targetFactory().IsGuanduActivity; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterYanPuOlRuntime))]
internal sealed class CoreProgramDrawDiscardWindowEntryRuntimePortsCharacterYanPuOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYanPuOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDrawDiscardWindowEntryRuntimePortsCharacterYanPuOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYanPuOlRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> IncludeYanPuDrawObservers(global::CardGame.Core.CharacterState subject, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> existing) => this._targetFactory().IncludeYanPuDrawObservers(subject, existing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAttributedMarkerEventsRuntime))]
internal sealed class CoreProgramDrawDiscardWindowEntryRuntimePortsCoreAttributedMarkerEventsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAttributedMarkerEventsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDrawDiscardWindowEntryRuntimePortsCoreAttributedMarkerEventsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAttributedMarkerEventsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> IncludeAttributedDrawObservers(global::CardGame.Core.CharacterState subject, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> existing) => this._targetFactory().IncludeAttributedDrawObservers(subject, existing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangNiBoundaryMobileRuntime))]
internal sealed class CoreProgramDrawDiscardWindowEntryRuntimePortsCharacterZhangNiBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangNiBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDrawDiscardWindowEntryRuntimePortsCharacterZhangNiBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangNiBoundaryMobileRuntime> _targetFactory = targetFactory;
    public bool HasPendingBoundaryWurongDrawSkip(int target) => this._targetFactory().HasPendingBoundaryWurongDrawSkip(target);
}
