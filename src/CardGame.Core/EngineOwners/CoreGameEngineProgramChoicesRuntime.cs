#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramChoicesRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterPanZhangMaZhongBoundaryMobileEntries _characterPanZhangMaZhongBoundaryMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterXuChuBoundaryMobileEntries _characterXuChuBoundaryMobileOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries _coreAlternatingBenefitsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramPersistentStateEntries _coreProgramPersistentStateOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheS5XuchuAiEntries _coreShanheS5XuchuAiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeSkillContextQueriesEntries _coreNativeSkillContextQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    private readonly ICoreProgramChoicesProgramPaidCardOperations _programPaidCardOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineProgramChoicesRuntime(global::CardGame.Core.RuntimeEntries.CharacterPanZhangMaZhongBoundaryMobileEntries characterPanZhangMaZhongBoundaryMobileOperations, global::CardGame.Core.RuntimeEntries.CharacterXuChuBoundaryMobileEntries characterXuChuBoundaryMobileOperations, global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries coreAlternatingBenefitsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramPersistentStateEntries coreProgramPersistentStateOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheS5XuchuAiEntries coreShanheS5XuchuAiOperations, global::CardGame.Core.RuntimeEntries.CoreNativeSkillContextQueriesEntries coreNativeSkillContextQueriesOperations, global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations, ICoreProgramChoicesProgramPaidCardOperations programPaidCardOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterPanZhangMaZhongBoundaryMobileOperations = characterPanZhangMaZhongBoundaryMobileOperations;
        this._characterXuChuBoundaryMobileOperations = characterXuChuBoundaryMobileOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreProgramPersistentStateOperations = coreProgramPersistentStateOperations;
        this._coreShanheS5XuchuAiOperations = coreShanheS5XuchuAiOperations;
        this._coreNativeSkillContextQueriesOperations = coreNativeSkillContextQueriesOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._programPaidCardOperations = programPaidCardOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
