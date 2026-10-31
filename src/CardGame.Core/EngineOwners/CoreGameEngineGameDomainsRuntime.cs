#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineGameDomainsRuntime
{
    private readonly ICoreGameDomainsContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreGameDomainsCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICoreGameDomainsCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreGameDomainsCoreTableOrderOperations _coreTableOrderOperations;
    private readonly ICoreGameDomainsKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatAccess _coreActualTurnProgressCurrentSeatAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineGameDomainsRuntime(ICoreGameDomainsContentRegistryOperations contentRegistryOperations, ICoreGameDomainsCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICoreGameDomainsCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreGameDomainsCoreTableOrderOperations coreTableOrderOperations, ICoreGameDomainsKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatAccess coreActualTurnProgressCurrentSeatAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreTableOrderOperations = coreTableOrderOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatAccess = coreActualTurnProgressCurrentSeatAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
