#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule, Core.ProgramCapability]
internal sealed partial class CoreProgramDrawDiscardWindowEntryRuntimePortsRuntime : IProgramDrawDiscardWindowEntryFacts, IProgramDrawDiscardWindowObservers
{
    private readonly RuntimeEntries.CharacterGuoYuanMobileShiEntries _characterGuoYuanMobileShiOperations;
    private readonly RuntimeEntries.CharacterHanSongAnniversaryEntries _characterHanSongAnniversaryOperations;
    private readonly RuntimeEntries.CharacterYanPuOlEntries _characterYanPuOlOperations;
    private readonly RuntimeEntries.CharacterZhangNiBoundaryMobileEntries _characterZhangNiBoundaryMobileOperations;
    private readonly RuntimeEntries.CharacterZhangShiPingOlEntries _characterZhangShiPingOlOperations;
    private readonly RuntimeEntries.CoreGameEngineAttributedMarkerEventsEntries _coreAttributedMarkerEventsOperations;
    private readonly RuntimeEntries.CoreGameEngineGuanduActivitiesEntries _coreGuanduActivitiesOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly RuntimeEntries.CoreGameEngineRecipientCategoryMarksEntries _coreRecipientCategoryMarksOperations;
    private readonly RuntimeEntries.CoreGameEngineTableOrderEntries _coreTableOrderOperations;
    internal CoreProgramDrawDiscardWindowEntryRuntimePortsRuntime(RuntimeEntries.CharacterGuoYuanMobileShiEntries characterGuoYuanMobileShiOperations, RuntimeEntries.CharacterHanSongAnniversaryEntries characterHanSongAnniversaryOperations, RuntimeEntries.CharacterYanPuOlEntries characterYanPuOlOperations, RuntimeEntries.CharacterZhangNiBoundaryMobileEntries characterZhangNiBoundaryMobileOperations, RuntimeEntries.CharacterZhangShiPingOlEntries characterZhangShiPingOlOperations, RuntimeEntries.CoreGameEngineAttributedMarkerEventsEntries coreAttributedMarkerEventsOperations, RuntimeEntries.CoreGameEngineGuanduActivitiesEntries coreGuanduActivitiesOperations, RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, RuntimeEntries.CoreGameEngineRecipientCategoryMarksEntries coreRecipientCategoryMarksOperations, RuntimeEntries.CoreGameEngineTableOrderEntries coreTableOrderOperations)
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
