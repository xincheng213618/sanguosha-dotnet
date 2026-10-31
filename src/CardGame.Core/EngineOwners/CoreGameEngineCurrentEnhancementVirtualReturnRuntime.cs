#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineCurrentEnhancementVirtualReturnRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLuBuHappyGodEntries _characterLuBuHappyGodOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries _coreAlternatingBenefitsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentEnhancementSlashRedirectsEntries _coreCurrentEnhancementSlashRedirectsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    internal CoreGameEngineCurrentEnhancementVirtualReturnRuntime(global::CardGame.Core.RuntimeEntries.CharacterLuBuHappyGodEntries characterLuBuHappyGodOperations, global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries coreAlternatingBenefitsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentEnhancementSlashRedirectsEntries coreCurrentEnhancementSlashRedirectsOperations, global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations)
    {
        this._characterLuBuHappyGodOperations = characterLuBuHappyGodOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreCurrentEnhancementSlashRedirectsOperations = coreCurrentEnhancementSlashRedirectsOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
    }
}
