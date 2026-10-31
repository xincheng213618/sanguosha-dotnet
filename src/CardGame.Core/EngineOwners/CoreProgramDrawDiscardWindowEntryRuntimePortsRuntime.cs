#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreProgramDrawDiscardWindowEntryRuntimePortsRuntime : global::CardGame.Core.IProgramDrawDiscardWindowEntryFacts, global::CardGame.Core.IProgramDrawDiscardWindowObservers
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterGuoYuanMobileShiEntries _characterGuoYuanMobileShiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHanSongAnniversaryEntries _characterHanSongAnniversaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterYanPuOlEntries _characterYanPuOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhangNiBoundaryMobileEntries _characterZhangNiBoundaryMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhangShiPingOlEntries _characterZhangShiPingOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAttributedMarkerEventsEntries _coreAttributedMarkerEventsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineGuanduActivitiesEntries _coreGuanduActivitiesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRecipientCategoryMarksEntries _coreRecipientCategoryMarksOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTableOrderEntries _coreTableOrderOperations;
    internal CoreProgramDrawDiscardWindowEntryRuntimePortsRuntime(global::CardGame.Core.RuntimeEntries.CharacterGuoYuanMobileShiEntries characterGuoYuanMobileShiOperations, global::CardGame.Core.RuntimeEntries.CharacterHanSongAnniversaryEntries characterHanSongAnniversaryOperations, global::CardGame.Core.RuntimeEntries.CharacterYanPuOlEntries characterYanPuOlOperations, global::CardGame.Core.RuntimeEntries.CharacterZhangNiBoundaryMobileEntries characterZhangNiBoundaryMobileOperations, global::CardGame.Core.RuntimeEntries.CharacterZhangShiPingOlEntries characterZhangShiPingOlOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAttributedMarkerEventsEntries coreAttributedMarkerEventsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineGuanduActivitiesEntries coreGuanduActivitiesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRecipientCategoryMarksEntries coreRecipientCategoryMarksOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineTableOrderEntries coreTableOrderOperations)
    {
        this._characterGuoYuanMobileShiOperations = characterGuoYuanMobileShiOperations;
        this._characterHanSongAnniversaryOperations = characterHanSongAnniversaryOperations;
        this._characterYanPuOlOperations = characterYanPuOlOperations;
        this._characterZhangNiBoundaryMobileOperations = characterZhangNiBoundaryMobileOperations;
        this._characterZhangShiPingOlOperations = characterZhangShiPingOlOperations;
        this._coreAttributedMarkerEventsOperations = coreAttributedMarkerEventsOperations;
        this._coreGuanduActivitiesOperations = coreGuanduActivitiesOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRecipientCategoryMarksOperations = coreRecipientCategoryMarksOperations;
        this._coreTableOrderOperations = coreTableOrderOperations;
    }
}
