#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineCardRecoveryModifiersRuntime
{
    private readonly ICoreCardRecoveryModifiersCharacterDengAiAnniversaryGodOperations _characterDengAiAnniversaryGodOperations;
    private readonly ICoreCardRecoveryModifiersTurnCardUseEffectsOperations _turnCardUseEffectsOperations;
    private readonly ICoreCardRecoveryModifiersCoreCardUseLifecycleOperations _coreCardUseLifecycleOperations;
    private readonly ICoreCardRecoveryModifiersCoreCardUseModulesOperations _coreCardUseModulesOperations;
    private readonly ICoreCardRecoveryModifiersKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    internal CoreGameEngineCardRecoveryModifiersRuntime(ICoreCardRecoveryModifiersCharacterDengAiAnniversaryGodOperations characterDengAiAnniversaryGodOperations, ICoreCardRecoveryModifiersTurnCardUseEffectsOperations turnCardUseEffectsOperations, ICoreCardRecoveryModifiersCoreCardUseLifecycleOperations coreCardUseLifecycleOperations, ICoreCardRecoveryModifiersCoreCardUseModulesOperations coreCardUseModulesOperations, ICoreCardRecoveryModifiersKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess)
    {
        this._characterDengAiAnniversaryGodOperations = characterDengAiAnniversaryGodOperations;
        this._turnCardUseEffectsOperations = turnCardUseEffectsOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreCardUseModulesOperations = coreCardUseModulesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
    }
}
