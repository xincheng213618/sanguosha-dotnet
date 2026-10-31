#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineKunlunMirrorRuntime : global::CardGame.Core.IKunlunMirrorHost
{
    private readonly ICoreKunlunMirrorCharacterHuCheErMouOperations _characterHuCheErMouOperations;
    private readonly ICoreKunlunMirrorResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreKunlunMirrorCoreCardUseLifecycleOperations _coreCardUseLifecycleOperations;
    private readonly ICoreKunlunMirrorCoreEquipmentSkillsOperations _coreEquipmentSkillsOperations;
    private readonly ICoreKunlunMirrorCoreLastDamageSourceReciprocityObserversOperations _coreLastDamageSourceReciprocityObserversOperations;
    private readonly ICoreKunlunMirrorCoreProgramAttackAttemptOperations _coreProgramAttackAttemptOperations;
    private readonly ICoreKunlunMirrorCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICoreKunlunMirrorCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreKunlunMirrorCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreKunlunMirrorCoreNativeDamageHandlesOperations _coreNativeDamageHandlesOperations;
    private readonly ICoreKunlunMirrorCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICoreKunlunMirrorKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineKunlunMirrorRuntime(ICoreKunlunMirrorCharacterHuCheErMouOperations characterHuCheErMouOperations, ICoreKunlunMirrorResolutionStackOperations resolutionStackOperations, ICoreKunlunMirrorCoreCardUseLifecycleOperations coreCardUseLifecycleOperations, ICoreKunlunMirrorCoreEquipmentSkillsOperations coreEquipmentSkillsOperations, ICoreKunlunMirrorCoreLastDamageSourceReciprocityObserversOperations coreLastDamageSourceReciprocityObserversOperations, ICoreKunlunMirrorCoreProgramAttackAttemptOperations coreProgramAttackAttemptOperations, ICoreKunlunMirrorCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICoreKunlunMirrorCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreKunlunMirrorCoreRuntimeOperations coreRuntimeOperations, ICoreKunlunMirrorCoreNativeDamageHandlesOperations coreNativeDamageHandlesOperations, ICoreKunlunMirrorCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICoreKunlunMirrorKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterHuCheErMouOperations = characterHuCheErMouOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreEquipmentSkillsOperations = coreEquipmentSkillsOperations;
        this._coreLastDamageSourceReciprocityObserversOperations = coreLastDamageSourceReciprocityObserversOperations;
        this._coreProgramAttackAttemptOperations = coreProgramAttackAttemptOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreNativeDamageHandlesOperations = coreNativeDamageHandlesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
