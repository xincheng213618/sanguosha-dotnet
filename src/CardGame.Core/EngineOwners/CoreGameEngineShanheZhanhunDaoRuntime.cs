#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineShanheZhanhunDaoRuntime : global::CardGame.Core.IShanheZhanhunDaoProgramHost
{
    private readonly ICoreShanheZhanhunDaoCharacterDaQiaoXiaoQiaoHappyGodOperations _characterDaQiaoXiaoQiaoHappyGodOperations;
    private readonly ICoreShanheZhanhunDaoResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreShanheZhanhunDaoCoreDamagePreventionProgramsOperations _coreDamagePreventionProgramsOperations;
    private readonly ICoreShanheZhanhunDaoCoreEquipmentSkillsOperations _coreEquipmentSkillsOperations;
    private readonly ICoreShanheZhanhunDaoCoreProgramAttackAttemptOperations _coreProgramAttackAttemptOperations;
    private readonly ICoreShanheZhanhunDaoCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreShanheZhanhunDaoCoreTreatedWoundedOperations _coreTreatedWoundedOperations;
    private readonly ICoreShanheZhanhunDaoCoreWushuangTacticRulesOperations _coreWushuangTacticRulesOperations;
    private readonly ICoreShanheZhanhunDaoCoreNativePlayerDeathOperations _coreNativePlayerDeathOperations;
    private readonly ICoreShanheZhanhunDaoCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICoreShanheZhanhunDaoKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineShanheZhanhunDaoRuntime(ICoreShanheZhanhunDaoCharacterDaQiaoXiaoQiaoHappyGodOperations characterDaQiaoXiaoQiaoHappyGodOperations, ICoreShanheZhanhunDaoResolutionStackOperations resolutionStackOperations, ICoreShanheZhanhunDaoCoreDamagePreventionProgramsOperations coreDamagePreventionProgramsOperations, ICoreShanheZhanhunDaoCoreEquipmentSkillsOperations coreEquipmentSkillsOperations, ICoreShanheZhanhunDaoCoreProgramAttackAttemptOperations coreProgramAttackAttemptOperations, ICoreShanheZhanhunDaoCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreShanheZhanhunDaoCoreTreatedWoundedOperations coreTreatedWoundedOperations, ICoreShanheZhanhunDaoCoreWushuangTacticRulesOperations coreWushuangTacticRulesOperations, ICoreShanheZhanhunDaoCoreNativePlayerDeathOperations coreNativePlayerDeathOperations, ICoreShanheZhanhunDaoCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICoreShanheZhanhunDaoKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterDaQiaoXiaoQiaoHappyGodOperations = characterDaQiaoXiaoQiaoHappyGodOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDamagePreventionProgramsOperations = coreDamagePreventionProgramsOperations;
        this._coreEquipmentSkillsOperations = coreEquipmentSkillsOperations;
        this._coreProgramAttackAttemptOperations = coreProgramAttackAttemptOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreTreatedWoundedOperations = coreTreatedWoundedOperations;
        this._coreWushuangTacticRulesOperations = coreWushuangTacticRulesOperations;
        this._coreNativePlayerDeathOperations = coreNativePlayerDeathOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
