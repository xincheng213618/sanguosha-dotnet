#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreForeignTurnContestRuntimePortsRuntime : global::CardGame.Core.IForeignTurnContestContinuations, global::CardGame.Core.IForeignTurnContestFacts, global::CardGame.Core.IForeignTurnContestSlashProducer
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLiuPanOlEntries _characterLiuPanOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterMaChengOlEntries _characterMaChengOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterMaWanOlEntries _characterMaWanOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterMaXiuMaTieOlEntries _characterMaXiuMaTieOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterQianZhaoOlEntries _characterQianZhaoOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterYangFuOlEntries _characterYangFuOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhangYanOlEntries _characterZhangYanOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseModulesEntries _coreCardUseModulesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentSlashFireOffersEntries _coreCurrentSlashFireOffersOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineForeignTurnContestsEntries _coreForeignTurnContestsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineHpChangeProgramsEntries _coreHpChangeProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineOriginalTargetAdditionsEntries _coreOriginalTargetAdditionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSameTypeActualUseAidEntries _coreSameTypeActualUseAidOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineShortRangeSlashTargetsEntries _coreShortRangeSlashTargetsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries _coreSkillProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTreatedWoundedEntries _coreTreatedWoundedOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreRecoveryReplacementEntries _coreRecoveryReplacementOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreForeignTurnContestRuntimePortsRuntime(global::CardGame.Core.RuntimeEntries.CharacterLiuPanOlEntries characterLiuPanOlOperations, global::CardGame.Core.RuntimeEntries.CharacterMaChengOlEntries characterMaChengOlOperations, global::CardGame.Core.RuntimeEntries.CharacterMaWanOlEntries characterMaWanOlOperations, global::CardGame.Core.RuntimeEntries.CharacterMaXiuMaTieOlEntries characterMaXiuMaTieOlOperations, global::CardGame.Core.RuntimeEntries.CharacterQianZhaoOlEntries characterQianZhaoOlOperations, global::CardGame.Core.RuntimeEntries.CharacterYangFuOlEntries characterYangFuOlOperations, global::CardGame.Core.RuntimeEntries.CharacterZhangYanOlEntries characterZhangYanOlOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseModulesEntries coreCardUseModulesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentSlashFireOffersEntries coreCurrentSlashFireOffersOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineForeignTurnContestsEntries coreForeignTurnContestsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineHpChangeProgramsEntries coreHpChangeProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineOriginalTargetAdditionsEntries coreOriginalTargetAdditionsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineSameTypeActualUseAidEntries coreSameTypeActualUseAidOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineShortRangeSlashTargetsEntries coreShortRangeSlashTargetsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries coreSkillProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineTreatedWoundedEntries coreTreatedWoundedOperations, global::CardGame.Core.RuntimeEntries.CoreRecoveryReplacementEntries coreRecoveryReplacementOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterLiuPanOlOperations = characterLiuPanOlOperations;
        this._characterMaChengOlOperations = characterMaChengOlOperations;
        this._characterMaWanOlOperations = characterMaWanOlOperations;
        this._characterMaXiuMaTieOlOperations = characterMaXiuMaTieOlOperations;
        this._characterQianZhaoOlOperations = characterQianZhaoOlOperations;
        this._characterYangFuOlOperations = characterYangFuOlOperations;
        this._characterZhangYanOlOperations = characterZhangYanOlOperations;
        this._coreCardUseModulesOperations = coreCardUseModulesOperations;
        this._coreCurrentSlashFireOffersOperations = coreCurrentSlashFireOffersOperations;
        this._coreForeignTurnContestsOperations = coreForeignTurnContestsOperations;
        this._coreHpChangeProgramsOperations = coreHpChangeProgramsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreOriginalTargetAdditionsOperations = coreOriginalTargetAdditionsOperations;
        this._coreSameTypeActualUseAidOperations = coreSameTypeActualUseAidOperations;
        this._coreShortRangeSlashTargetsOperations = coreShortRangeSlashTargetsOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._coreTreatedWoundedOperations = coreTreatedWoundedOperations;
        this._coreRecoveryReplacementOperations = coreRecoveryReplacementOperations;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
