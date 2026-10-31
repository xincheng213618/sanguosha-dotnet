#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineZhenyiJudgmentWindowsRuntime
{
    private readonly ICoreZhenyiJudgmentWindowsCharacterXiaoQiaoBoundaryMobileOperations _characterXiaoQiaoBoundaryMobileOperations;
    private readonly ICoreZhenyiJudgmentWindowsCharacterZhangQiYingOlOperations _characterZhangQiYingOlOperations;
    private readonly ICoreZhenyiJudgmentWindowsContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreZhenyiJudgmentWindowsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreZhenyiJudgmentWindowsCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreZhenyiJudgmentWindowsCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreZhenyiJudgmentWindowsCoreSkillProgramsOperations _coreSkillProgramsOperations;
    private readonly ICoreZhenyiJudgmentWindowsCoreHellDizangProgramsOperations _coreHellDizangProgramsOperations;
    private readonly ICoreZhenyiJudgmentWindowsCoreNativeCardAppearanceOperations _coreNativeCardAppearanceOperations;
    private readonly ICoreZhenyiJudgmentWindowsCoreNativeDamageHandlesOperations _coreNativeDamageHandlesOperations;
    private readonly ICoreZhenyiJudgmentWindowsCoreNativeJudgmentOperations _coreNativeJudgmentOperations;
    private readonly ICoreZhenyiJudgmentWindowsCoreResolutionIdentityBoundaryOperations _coreResolutionIdentityBoundaryOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineZhenyiJudgmentWindowsRuntime(ICoreZhenyiJudgmentWindowsCharacterXiaoQiaoBoundaryMobileOperations characterXiaoQiaoBoundaryMobileOperations, ICoreZhenyiJudgmentWindowsCharacterZhangQiYingOlOperations characterZhangQiYingOlOperations, ICoreZhenyiJudgmentWindowsContentRegistryOperations contentRegistryOperations, ICoreZhenyiJudgmentWindowsResolutionStackOperations resolutionStackOperations, ICoreZhenyiJudgmentWindowsCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreZhenyiJudgmentWindowsCoreRuntimeOperations coreRuntimeOperations, ICoreZhenyiJudgmentWindowsCoreSkillProgramsOperations coreSkillProgramsOperations, ICoreZhenyiJudgmentWindowsCoreHellDizangProgramsOperations coreHellDizangProgramsOperations, ICoreZhenyiJudgmentWindowsCoreNativeCardAppearanceOperations coreNativeCardAppearanceOperations, ICoreZhenyiJudgmentWindowsCoreNativeDamageHandlesOperations coreNativeDamageHandlesOperations, ICoreZhenyiJudgmentWindowsCoreNativeJudgmentOperations coreNativeJudgmentOperations, ICoreZhenyiJudgmentWindowsCoreResolutionIdentityBoundaryOperations coreResolutionIdentityBoundaryOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterXiaoQiaoBoundaryMobileOperations = characterXiaoQiaoBoundaryMobileOperations;
        this._characterZhangQiYingOlOperations = characterZhangQiYingOlOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._coreHellDizangProgramsOperations = coreHellDizangProgramsOperations;
        this._coreNativeCardAppearanceOperations = coreNativeCardAppearanceOperations;
        this._coreNativeDamageHandlesOperations = coreNativeDamageHandlesOperations;
        this._coreNativeJudgmentOperations = coreNativeJudgmentOperations;
        this._coreResolutionIdentityBoundaryOperations = coreResolutionIdentityBoundaryOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
