#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineCurrentUsePhysicalClaimsRuntime : global::CardGame.Core.ICurrentUsePhysicalClaimProgramHost
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterChengJiChengCuiOlEntries _characterChengJiChengCuiOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterCuiLieAnniversaryEntries _characterCuiLieAnniversaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDongChengMouEntries _characterDongChengMouOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuCheErAnniversaryEntries _characterHuCheErAnniversaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuaManOlEntries _characterHuaManOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterPangDeBoundaryMobileEntries _characterPangDeBoundaryMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterXiangChongOlEntries _characterXiangChongOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterXinPiOlEntries _characterXinPiOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterXuRongOlEntries _characterXuRongOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterYanYanBoundaryMobileEntries _characterYanYanBoundaryMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhouCangBoundaryMobileEntries _characterZhouCangBoundaryMobileOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardMovementProgramsEntries _coreCardMovementProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineExtraEffectMaterialClaimsEntries _coreExtraEffectMaterialClaimsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineOverflowTargetCancellationEntries _coreOverflowTargetCancellationOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries _coreProgramAtomicCardMovesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineResponseEntityExchangeEntries _coreResponseEntityExchangeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheNullifiedTrickClaimsEntries _coreShanheNullifiedTrickClaimsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSourceDingxiNativeEntries _coreSourceDingxiNativeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineCurrentUsePhysicalClaimsRuntime(global::CardGame.Core.RuntimeEntries.CharacterChengJiChengCuiOlEntries characterChengJiChengCuiOlOperations,global::CardGame.Core.RuntimeEntries.CharacterCuiLieAnniversaryEntries characterCuiLieAnniversaryOperations,global::CardGame.Core.RuntimeEntries.CharacterDongChengMouEntries characterDongChengMouOperations,global::CardGame.Core.RuntimeEntries.CharacterHuCheErAnniversaryEntries characterHuCheErAnniversaryOperations,global::CardGame.Core.RuntimeEntries.CharacterHuaManOlEntries characterHuaManOlOperations,global::CardGame.Core.RuntimeEntries.CharacterPangDeBoundaryMobileEntries characterPangDeBoundaryMobileOperations,global::CardGame.Core.RuntimeEntries.CharacterXiangChongOlEntries characterXiangChongOlOperations,global::CardGame.Core.RuntimeEntries.CharacterXinPiOlEntries characterXinPiOlOperations,global::CardGame.Core.RuntimeEntries.CharacterXuRongOlEntries characterXuRongOlOperations,global::CardGame.Core.RuntimeEntries.CharacterYanYanBoundaryMobileEntries characterYanYanBoundaryMobileOperations,global::CardGame.Core.RuntimeEntries.CharacterZhouCangBoundaryMobileEntries characterZhouCangBoundaryMobileOperations,global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess cardZonesOperations,global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCardMovementProgramsEntries coreCardMovementProgramsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineExtraEffectMaterialClaimsEntries coreExtraEffectMaterialClaimsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineOverflowTargetCancellationEntries coreOverflowTargetCancellationOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries coreProgramAtomicCardMovesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineResponseEntityExchangeEntries coreResponseEntityExchangeOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheNullifiedTrickClaimsEntries coreShanheNullifiedTrickClaimsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineSourceDingxiNativeEntries coreSourceDingxiNativeOperations,global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations,global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations,global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess,global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
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
