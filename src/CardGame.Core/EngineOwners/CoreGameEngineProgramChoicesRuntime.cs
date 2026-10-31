#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramChoicesRuntime
{
    private readonly ICoreProgramChoicesCharacterPanZhangMaZhongBoundaryMobileOperations _characterPanZhangMaZhongBoundaryMobileOperations;
    private readonly ICoreProgramChoicesCharacterXuChuBoundaryMobileOperations _characterXuChuBoundaryMobileOperations;
    private readonly ICoreProgramChoicesContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreProgramChoicesCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICoreProgramChoicesCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreProgramChoicesCoreProgramPersistentStateOperations _coreProgramPersistentStateOperations;
    private readonly ICoreProgramChoicesCoreShanheS5XuchuAiOperations _coreShanheS5XuchuAiOperations;
    private readonly ICoreProgramChoicesCoreNativeSkillContextQueriesOperations _coreNativeSkillContextQueriesOperations;
    private readonly ICoreProgramChoicesCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICoreProgramChoicesProgramPaidCardOperations _programPaidCardOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineProgramChoicesRuntime(ICoreProgramChoicesCharacterPanZhangMaZhongBoundaryMobileOperations characterPanZhangMaZhongBoundaryMobileOperations, ICoreProgramChoicesCharacterXuChuBoundaryMobileOperations characterXuChuBoundaryMobileOperations, ICoreProgramChoicesContentRegistryOperations contentRegistryOperations, ICoreProgramChoicesCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICoreProgramChoicesCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreProgramChoicesCoreProgramPersistentStateOperations coreProgramPersistentStateOperations, ICoreProgramChoicesCoreShanheS5XuchuAiOperations coreShanheS5XuchuAiOperations, ICoreProgramChoicesCoreNativeSkillContextQueriesOperations coreNativeSkillContextQueriesOperations, ICoreProgramChoicesCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICoreProgramChoicesProgramPaidCardOperations programPaidCardOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
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
