#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule, Core.ProgramCapability]
internal sealed partial class CoreGameEngineCurrentUsePhysicalClaimsRuntime : ICurrentUsePhysicalClaimProgramHost
{
    private readonly RuntimeEntries.CharacterChengJiChengCuiOlEntries _characterChengJiChengCuiOlOperations;
    private readonly RuntimeEntries.CharacterCuiLieAnniversaryEntries _characterCuiLieAnniversaryOperations;
    private readonly RuntimeEntries.CharacterDongChengMouEntries _characterDongChengMouOperations;
    private readonly RuntimeEntries.CharacterHuCheErAnniversaryEntries _characterHuCheErAnniversaryOperations;
    private readonly RuntimeEntries.CharacterHuaManOlEntries _characterHuaManOlOperations;
    private readonly RuntimeEntries.CharacterPangDeBoundaryMobileEntries _characterPangDeBoundaryMobileOperations;
    private readonly RuntimeEntries.CharacterXiangChongOlEntries _characterXiangChongOlOperations;
    private readonly RuntimeEntries.CharacterXinPiOlEntries _characterXinPiOlOperations;
    private readonly RuntimeEntries.CharacterXuRongOlEntries _characterXuRongOlOperations;
    private readonly RuntimeEntries.CharacterYanYanBoundaryMobileEntries _characterYanYanBoundaryMobileOperations;
    private readonly RuntimeEntries.CharacterZhouCangBoundaryMobileEntries _characterZhouCangBoundaryMobileOperations;
    private readonly IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly RuntimeEntries.CoreGameEngineCardMovementProgramsEntries _coreCardMovementProgramsOperations;
    private readonly RuntimeEntries.CoreGameEngineExtraEffectMaterialClaimsEntries _coreExtraEffectMaterialClaimsOperations;
    private readonly RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly RuntimeEntries.CoreGameEngineOverflowTargetCancellationEntries _coreOverflowTargetCancellationOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries _coreProgramAtomicCardMovesOperations;
    private readonly RuntimeEntries.CoreGameEngineResponseEntityExchangeEntries _coreResponseEntityExchangeOperations;
    private readonly RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly RuntimeEntries.CoreGameEngineShanheNullifiedTrickClaimsEntries _coreShanheNullifiedTrickClaimsOperations;
    private readonly RuntimeEntries.CoreGameEngineSourceDingxiNativeEntries _coreSourceDingxiNativeOperations;
    private readonly RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineCurrentUsePhysicalClaimsRuntime(RuntimeEntries.CharacterChengJiChengCuiOlEntries characterChengJiChengCuiOlOperations,RuntimeEntries.CharacterCuiLieAnniversaryEntries characterCuiLieAnniversaryOperations,RuntimeEntries.CharacterDongChengMouEntries characterDongChengMouOperations,RuntimeEntries.CharacterHuCheErAnniversaryEntries characterHuCheErAnniversaryOperations,RuntimeEntries.CharacterHuaManOlEntries characterHuaManOlOperations,RuntimeEntries.CharacterPangDeBoundaryMobileEntries characterPangDeBoundaryMobileOperations,RuntimeEntries.CharacterXiangChongOlEntries characterXiangChongOlOperations,RuntimeEntries.CharacterXinPiOlEntries characterXinPiOlOperations,RuntimeEntries.CharacterXuRongOlEntries characterXuRongOlOperations,RuntimeEntries.CharacterYanYanBoundaryMobileEntries characterYanYanBoundaryMobileOperations,RuntimeEntries.CharacterZhouCangBoundaryMobileEntries characterZhouCangBoundaryMobileOperations,IKernelCardZoneStoreAccess cardZonesOperations,IKernelFrameStoreAccess resolutionStackOperations,RuntimeEntries.CoreGameEngineCardMovementProgramsEntries coreCardMovementProgramsOperations,RuntimeEntries.CoreGameEngineExtraEffectMaterialClaimsEntries coreExtraEffectMaterialClaimsOperations,RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations,RuntimeEntries.CoreGameEngineOverflowTargetCancellationEntries coreOverflowTargetCancellationOperations,RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries coreProgramAtomicCardMovesOperations,RuntimeEntries.CoreGameEngineResponseEntityExchangeEntries coreResponseEntityExchangeOperations,RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations,RuntimeEntries.CoreGameEngineShanheNullifiedTrickClaimsEntries coreShanheNullifiedTrickClaimsOperations,RuntimeEntries.CoreGameEngineSourceDingxiNativeEntries coreSourceDingxiNativeOperations,RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations,IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations,EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess,EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterChengJiChengCuiOlOperations = characterChengJiChengCuiOlOperations;
        this._characterCuiLieAnniversaryOperations = characterCuiLieAnniversaryOperations;
        this._characterDongChengMouOperations = characterDongChengMouOperations;
        this._characterHuCheErAnniversaryOperations = characterHuCheErAnniversaryOperations;
        this._characterHuaManOlOperations = characterHuaManOlOperations;
        this._characterPangDeBoundaryMobileOperations = characterPangDeBoundaryMobileOperations;
        this._characterXiangChongOlOperations = characterXiangChongOlOperations;
        this._characterXinPiOlOperations = characterXinPiOlOperations;
        this._characterXuRongOlOperations = characterXuRongOlOperations;
        this._characterYanYanBoundaryMobileOperations = characterYanYanBoundaryMobileOperations;
        this._characterZhouCangBoundaryMobileOperations = characterZhouCangBoundaryMobileOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreExtraEffectMaterialClaimsOperations = coreExtraEffectMaterialClaimsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreOverflowTargetCancellationOperations = coreOverflowTargetCancellationOperations;
        this._coreProgramAtomicCardMovesOperations = coreProgramAtomicCardMovesOperations;
        this._coreResponseEntityExchangeOperations = coreResponseEntityExchangeOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreShanheNullifiedTrickClaimsOperations = coreShanheNullifiedTrickClaimsOperations;
        this._coreSourceDingxiNativeOperations = coreSourceDingxiNativeOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
