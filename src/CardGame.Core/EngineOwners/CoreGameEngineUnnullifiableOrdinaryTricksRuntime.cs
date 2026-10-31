#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineUnnullifiableOrdinaryTricksRuntime
{
    private readonly ICoreUnnullifiableOrdinaryTricksContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreUnnullifiableOrdinaryTricksCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICoreUnnullifiableOrdinaryTricksCoreCardUseLifecycleOperations _coreCardUseLifecycleOperations;
    private readonly ICoreUnnullifiableOrdinaryTricksCoreProgramCardPoliciesOperations _coreProgramCardPoliciesOperations;
    private readonly ICoreUnnullifiableOrdinaryTricksCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreUnnullifiableOrdinaryTricksKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineUnnullifiableOrdinaryTricksRuntime(ICoreUnnullifiableOrdinaryTricksContentRegistryOperations contentRegistryOperations, ICoreUnnullifiableOrdinaryTricksCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICoreUnnullifiableOrdinaryTricksCoreCardUseLifecycleOperations coreCardUseLifecycleOperations, ICoreUnnullifiableOrdinaryTricksCoreProgramCardPoliciesOperations coreProgramCardPoliciesOperations, ICoreUnnullifiableOrdinaryTricksCoreRuntimeOperations coreRuntimeOperations, ICoreUnnullifiableOrdinaryTricksKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
