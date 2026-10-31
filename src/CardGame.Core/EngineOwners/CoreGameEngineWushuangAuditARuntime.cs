#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineWushuangAuditARuntime : global::CardGame.Core.IWushuangTiaoxinExpansionHost
{
    private readonly ICoreWushuangAuditACoreGuanduGeneralsOperations _coreGuanduGeneralsOperations;
    private readonly ICoreWushuangAuditACoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICoreWushuangAuditACorePlayCardHistoryOperations _corePlayCardHistoryOperations;
    private readonly ICoreWushuangAuditACorePrivateOfferReturnsOperations _corePrivateOfferReturnsOperations;
    private readonly ICoreWushuangAuditACoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICoreWushuangAuditACoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreWushuangAuditAKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineSkillProgramsProgramPhaseUsesAccess _coreGameEngineSkillProgramsProgramPhaseUsesAccess;
    internal CoreGameEngineWushuangAuditARuntime(ICoreWushuangAuditACoreGuanduGeneralsOperations coreGuanduGeneralsOperations, ICoreWushuangAuditACoreNationalSkillsOperations coreNationalSkillsOperations, ICoreWushuangAuditACorePlayCardHistoryOperations corePlayCardHistoryOperations, ICoreWushuangAuditACorePrivateOfferReturnsOperations corePrivateOfferReturnsOperations, ICoreWushuangAuditACoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICoreWushuangAuditACoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreWushuangAuditAKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineSkillProgramsProgramPhaseUsesAccess coreGameEngineSkillProgramsProgramPhaseUsesAccess)
    {
        this._coreGuanduGeneralsOperations = coreGuanduGeneralsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._corePlayCardHistoryOperations = corePlayCardHistoryOperations;
        this._corePrivateOfferReturnsOperations = corePrivateOfferReturnsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess = coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
        this._coreGameEngineSkillProgramsProgramPhaseUsesAccess = coreGameEngineSkillProgramsProgramPhaseUsesAccess;
    }
}
