#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterGaoLanOlRuntime : global::CardGame.Core.IGaoLanProgramHost
{
    private readonly ICharacterGaoLanOlCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICharacterGaoLanOlCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICharacterGaoLanOlCoreTreatedWoundedOperations _coreTreatedWoundedOperations;
    private readonly ICharacterGaoLanOlCoreNativeRecoveryFramesOperations _coreNativeRecoveryFramesOperations;
    private readonly ICharacterGaoLanOlCoreRecoveryReplacementOperations _coreRecoveryReplacementOperations;
    private readonly ICharacterGaoLanOlKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterGaoLanOlRuntime(ICharacterGaoLanOlCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICharacterGaoLanOlCoreRuntimeOperations coreRuntimeOperations, ICharacterGaoLanOlCoreTreatedWoundedOperations coreTreatedWoundedOperations, ICharacterGaoLanOlCoreNativeRecoveryFramesOperations coreNativeRecoveryFramesOperations, ICharacterGaoLanOlCoreRecoveryReplacementOperations coreRecoveryReplacementOperations, ICharacterGaoLanOlKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreTreatedWoundedOperations = coreTreatedWoundedOperations;
        this._coreNativeRecoveryFramesOperations = coreNativeRecoveryFramesOperations;
        this._coreRecoveryReplacementOperations = coreRecoveryReplacementOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
