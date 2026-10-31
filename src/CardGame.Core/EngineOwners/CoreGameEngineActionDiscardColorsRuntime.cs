#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineActionDiscardColorsRuntime
{
    private readonly ICoreActionDiscardColorsContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreActionDiscardColorsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreActionDiscardColorsCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICoreActionDiscardColorsCoreCardDeclarationsOperations _coreCardDeclarationsOperations;
    private readonly ICoreActionDiscardColorsCoreTurnRedSlashBenefitsOperations _coreTurnRedSlashBenefitsOperations;
    private readonly ICoreActionDiscardColorsCoreNativeCardAppearanceOperations _coreNativeCardAppearanceOperations;
    private readonly ICoreActionDiscardColorsKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineActionDiscardColorsRuntime(ICoreActionDiscardColorsContentRegistryOperations contentRegistryOperations, ICoreActionDiscardColorsResolutionStackOperations resolutionStackOperations, ICoreActionDiscardColorsCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICoreActionDiscardColorsCoreCardDeclarationsOperations coreCardDeclarationsOperations, ICoreActionDiscardColorsCoreTurnRedSlashBenefitsOperations coreTurnRedSlashBenefitsOperations, ICoreActionDiscardColorsCoreNativeCardAppearanceOperations coreNativeCardAppearanceOperations, ICoreActionDiscardColorsKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreCardDeclarationsOperations = coreCardDeclarationsOperations;
        this._coreTurnRedSlashBenefitsOperations = coreTurnRedSlashBenefitsOperations;
        this._coreNativeCardAppearanceOperations = coreNativeCardAppearanceOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
