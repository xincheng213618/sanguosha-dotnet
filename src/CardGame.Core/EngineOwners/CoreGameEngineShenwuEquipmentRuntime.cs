#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineShenwuEquipmentRuntime
{
    private readonly ICoreShenwuEquipmentCharacterHuCheErMouOperations _characterHuCheErMouOperations;
    private readonly ICoreShenwuEquipmentCoreAdvancedWeaponsOperations _coreAdvancedWeaponsOperations;
    private readonly ICoreShenwuEquipmentCoreCardUseLifecycleOperations _coreCardUseLifecycleOperations;
    private readonly ICoreShenwuEquipmentCoreDirectedCardPolicyQueriesOperations _coreDirectedCardPolicyQueriesOperations;
    private readonly ICoreShenwuEquipmentCoreEquipmentContentOperations _coreEquipmentContentOperations;
    private readonly ICoreShenwuEquipmentCoreEquipmentSkillsOperations _coreEquipmentSkillsOperations;
    private readonly ICoreShenwuEquipmentCoreExtendedViewAsOperations _coreExtendedViewAsOperations;
    private readonly ICoreShenwuEquipmentCoreProgramAttackAttemptOperations _coreProgramAttackAttemptOperations;
    private readonly ICoreShenwuEquipmentCoreNativeAttackCompletionOperations _coreNativeAttackCompletionOperations;
    private readonly ICoreShenwuEquipmentCoreNativeCardUseFramesOperations _coreNativeCardUseFramesOperations;
    private readonly ICoreShenwuEquipmentCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICoreShenwuEquipmentCoreShenwuNpcProgramsOperations _coreShenwuNpcProgramsOperations;
    private readonly ICoreShenwuEquipmentKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineShenwuEquipmentRuntime(ICoreShenwuEquipmentCharacterHuCheErMouOperations characterHuCheErMouOperations, ICoreShenwuEquipmentCoreAdvancedWeaponsOperations coreAdvancedWeaponsOperations, ICoreShenwuEquipmentCoreCardUseLifecycleOperations coreCardUseLifecycleOperations, ICoreShenwuEquipmentCoreDirectedCardPolicyQueriesOperations coreDirectedCardPolicyQueriesOperations, ICoreShenwuEquipmentCoreEquipmentContentOperations coreEquipmentContentOperations, ICoreShenwuEquipmentCoreEquipmentSkillsOperations coreEquipmentSkillsOperations, ICoreShenwuEquipmentCoreExtendedViewAsOperations coreExtendedViewAsOperations, ICoreShenwuEquipmentCoreProgramAttackAttemptOperations coreProgramAttackAttemptOperations, ICoreShenwuEquipmentCoreNativeAttackCompletionOperations coreNativeAttackCompletionOperations, ICoreShenwuEquipmentCoreNativeCardUseFramesOperations coreNativeCardUseFramesOperations, ICoreShenwuEquipmentCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICoreShenwuEquipmentCoreShenwuNpcProgramsOperations coreShenwuNpcProgramsOperations, ICoreShenwuEquipmentKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterHuCheErMouOperations = characterHuCheErMouOperations;
        this._coreAdvancedWeaponsOperations = coreAdvancedWeaponsOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreDirectedCardPolicyQueriesOperations = coreDirectedCardPolicyQueriesOperations;
        this._coreEquipmentContentOperations = coreEquipmentContentOperations;
        this._coreEquipmentSkillsOperations = coreEquipmentSkillsOperations;
        this._coreExtendedViewAsOperations = coreExtendedViewAsOperations;
        this._coreProgramAttackAttemptOperations = coreProgramAttackAttemptOperations;
        this._coreNativeAttackCompletionOperations = coreNativeAttackCompletionOperations;
        this._coreNativeCardUseFramesOperations = coreNativeCardUseFramesOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreShenwuNpcProgramsOperations = coreShenwuNpcProgramsOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
