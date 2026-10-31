#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterZhuZhiAnniversaryRuntime : global::CardGame.Core.IAnniversaryZhuZhiHost
{
    private readonly ICharacterZhuZhiAnniversaryCharacterDaQiaoXiaoQiaoHappyGodOperations _characterDaQiaoXiaoQiaoHappyGodOperations;
    private readonly ICharacterZhuZhiAnniversaryContentRegistryOperations _contentRegistryOperations;
    private readonly ICharacterZhuZhiAnniversaryCorePrivateOfferReturnsOperations _corePrivateOfferReturnsOperations;
    private readonly ICharacterZhuZhiAnniversaryCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICharacterZhuZhiAnniversaryCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICharacterZhuZhiAnniversaryCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICharacterZhuZhiAnniversaryCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICharacterZhuZhiAnniversaryKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterZhuZhiAnniversaryRuntime(ICharacterZhuZhiAnniversaryCharacterDaQiaoXiaoQiaoHappyGodOperations characterDaQiaoXiaoQiaoHappyGodOperations, ICharacterZhuZhiAnniversaryContentRegistryOperations contentRegistryOperations, ICharacterZhuZhiAnniversaryCorePrivateOfferReturnsOperations corePrivateOfferReturnsOperations, ICharacterZhuZhiAnniversaryCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICharacterZhuZhiAnniversaryCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICharacterZhuZhiAnniversaryCoreRuntimeOperations coreRuntimeOperations, ICharacterZhuZhiAnniversaryCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICharacterZhuZhiAnniversaryKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterDaQiaoXiaoQiaoHappyGodOperations = characterDaQiaoXiaoQiaoHappyGodOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._corePrivateOfferReturnsOperations = corePrivateOfferReturnsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
