#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineAdvancedPoliciesRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelSkillRuntimeStateAccess _skillRuntimeStateOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries _coreAdvancedLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedWeaponsEntries _coreAdvancedWeaponsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries _coreCardUseLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineFriendTrioEntries _coreFriendTrioOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries _coreProgramCardPoliciesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineAdvancedPoliciesNextUnlimitedCardOwnersAccess _coreGameEngineAdvancedPoliciesNextUnlimitedCardOwnersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineFriendTrioFriendQihuiUnlimitedAccess _coreGameEngineFriendTrioFriendQihuiUnlimitedAccess;
    internal CoreGameEngineAdvancedPoliciesRuntime(global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.EngineOwners.IKernelSkillRuntimeStateAccess skillRuntimeStateOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries coreAdvancedLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedWeaponsEntries coreAdvancedWeaponsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries coreCardUseLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineFriendTrioEntries coreFriendTrioOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries coreProgramCardPoliciesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineAdvancedPoliciesNextUnlimitedCardOwnersAccess coreGameEngineAdvancedPoliciesNextUnlimitedCardOwnersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineFriendTrioFriendQihuiUnlimitedAccess coreGameEngineFriendTrioFriendQihuiUnlimitedAccess)
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
