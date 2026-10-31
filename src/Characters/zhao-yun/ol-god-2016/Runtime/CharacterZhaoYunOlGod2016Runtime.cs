#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CharacterZhaoYunOlGod2016Runtime
{
    private readonly ICharacterZhaoYunOlGod2016ContentRegistryOperations _contentRegistryOperations;
    private readonly ICharacterZhaoYunOlGod2016CorePlayCardHistoryOperations _corePlayCardHistoryOperations;
    private readonly ICharacterZhaoYunOlGod2016CoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICharacterZhaoYunOlGod2016KernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterZhaoYunOlGod2016Runtime(ICharacterZhaoYunOlGod2016ContentRegistryOperations contentRegistryOperations, ICharacterZhaoYunOlGod2016CorePlayCardHistoryOperations corePlayCardHistoryOperations, ICharacterZhaoYunOlGod2016CoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICharacterZhaoYunOlGod2016KernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._corePlayCardHistoryOperations = corePlayCardHistoryOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
