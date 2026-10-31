#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineAdvancedPoliciesRuntime
{
    private readonly ICoreAdvancedPoliciesContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreAdvancedPoliciesSkillRuntimeStateOperations _skillRuntimeStateOperations;
    private readonly ICoreAdvancedPoliciesCoreAdvancedLifecycleOperations _coreAdvancedLifecycleOperations;
    private readonly ICoreAdvancedPoliciesCoreAdvancedWeaponsOperations _coreAdvancedWeaponsOperations;
    private readonly ICoreAdvancedPoliciesCoreCardUseLifecycleOperations _coreCardUseLifecycleOperations;
    private readonly ICoreAdvancedPoliciesCoreFriendTrioOperations _coreFriendTrioOperations;
    private readonly ICoreAdvancedPoliciesCoreProgramCardPoliciesOperations _coreProgramCardPoliciesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineAdvancedPoliciesNextUnlimitedCardOwnersAccess _coreGameEngineAdvancedPoliciesNextUnlimitedCardOwnersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineFriendTrioFriendQihuiUnlimitedAccess _coreGameEngineFriendTrioFriendQihuiUnlimitedAccess;
    internal CoreGameEngineAdvancedPoliciesRuntime(ICoreAdvancedPoliciesContentRegistryOperations contentRegistryOperations, ICoreAdvancedPoliciesSkillRuntimeStateOperations skillRuntimeStateOperations, ICoreAdvancedPoliciesCoreAdvancedLifecycleOperations coreAdvancedLifecycleOperations, ICoreAdvancedPoliciesCoreAdvancedWeaponsOperations coreAdvancedWeaponsOperations, ICoreAdvancedPoliciesCoreCardUseLifecycleOperations coreCardUseLifecycleOperations, ICoreAdvancedPoliciesCoreFriendTrioOperations coreFriendTrioOperations, ICoreAdvancedPoliciesCoreProgramCardPoliciesOperations coreProgramCardPoliciesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineAdvancedPoliciesNextUnlimitedCardOwnersAccess coreGameEngineAdvancedPoliciesNextUnlimitedCardOwnersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineFriendTrioFriendQihuiUnlimitedAccess coreGameEngineFriendTrioFriendQihuiUnlimitedAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._skillRuntimeStateOperations = skillRuntimeStateOperations;
        this._coreAdvancedLifecycleOperations = coreAdvancedLifecycleOperations;
        this._coreAdvancedWeaponsOperations = coreAdvancedWeaponsOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreFriendTrioOperations = coreFriendTrioOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreGameEngineAdvancedPoliciesNextUnlimitedCardOwnersAccess = coreGameEngineAdvancedPoliciesNextUnlimitedCardOwnersAccess;
        this._coreGameEngineFriendTrioFriendQihuiUnlimitedAccess = coreGameEngineFriendTrioFriendQihuiUnlimitedAccess;
    }
}
