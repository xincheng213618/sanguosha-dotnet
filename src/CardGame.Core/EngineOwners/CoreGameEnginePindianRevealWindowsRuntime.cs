#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEnginePindianRevealWindowsRuntime
{
    private readonly ICorePindianRevealWindowsCharacterCuiLieAnniversaryOperations _characterCuiLieAnniversaryOperations;
    private readonly ICorePindianRevealWindowsCharacterLuKangWumiaoOperations _characterLuKangWumiaoOperations;
    private readonly ICorePindianRevealWindowsCharacterPangJiOlOperations _characterPangJiOlOperations;
    private readonly ICorePindianRevealWindowsCharacterZhangLiaoMobileGodOperations _characterZhangLiaoMobileGodOperations;
    private readonly ICorePindianRevealWindowsCardZonesOperations _cardZonesOperations;
    private readonly ICorePindianRevealWindowsContentRegistryOperations _contentRegistryOperations;
    private readonly ICorePindianRevealWindowsResolutionStackOperations _resolutionStackOperations;
    private readonly ICorePindianRevealWindowsCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICorePindianRevealWindowsCorePindianOperations _corePindianOperations;
    private readonly ICorePindianRevealWindowsCorePindianClaimsOperations _corePindianClaimsOperations;
    private readonly ICorePindianRevealWindowsCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICorePindianRevealWindowsCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICorePindianRevealWindowsCoreSimultaneousPindianOperations _coreSimultaneousPindianOperations;
    private readonly ICorePindianRevealWindowsCoreTableOrderOperations _coreTableOrderOperations;
    private readonly ICorePindianRevealWindowsCoreMatchLogOperations _coreMatchLogOperations;
    private readonly ICorePindianRevealWindowsCoreResolutionIdentityBoundaryOperations _coreResolutionIdentityBoundaryOperations;
    private readonly ICorePindianRevealWindowsKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineSkillProgramsProgramPhaseUsesAccess _coreGameEngineSkillProgramsProgramPhaseUsesAccess;
    internal CoreGameEnginePindianRevealWindowsRuntime(ICorePindianRevealWindowsCharacterCuiLieAnniversaryOperations characterCuiLieAnniversaryOperations, ICorePindianRevealWindowsCharacterLuKangWumiaoOperations characterLuKangWumiaoOperations, ICorePindianRevealWindowsCharacterPangJiOlOperations characterPangJiOlOperations, ICorePindianRevealWindowsCharacterZhangLiaoMobileGodOperations characterZhangLiaoMobileGodOperations, ICorePindianRevealWindowsCardZonesOperations cardZonesOperations, ICorePindianRevealWindowsContentRegistryOperations contentRegistryOperations, ICorePindianRevealWindowsResolutionStackOperations resolutionStackOperations, ICorePindianRevealWindowsCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICorePindianRevealWindowsCorePindianOperations corePindianOperations, ICorePindianRevealWindowsCorePindianClaimsOperations corePindianClaimsOperations, ICorePindianRevealWindowsCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICorePindianRevealWindowsCoreRuntimeOperations coreRuntimeOperations, ICorePindianRevealWindowsCoreSimultaneousPindianOperations coreSimultaneousPindianOperations, ICorePindianRevealWindowsCoreTableOrderOperations coreTableOrderOperations, ICorePindianRevealWindowsCoreMatchLogOperations coreMatchLogOperations, ICorePindianRevealWindowsCoreResolutionIdentityBoundaryOperations coreResolutionIdentityBoundaryOperations, ICorePindianRevealWindowsKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineSkillProgramsProgramPhaseUsesAccess coreGameEngineSkillProgramsProgramPhaseUsesAccess)
    {
        this._characterCuiLieAnniversaryOperations = characterCuiLieAnniversaryOperations;
        this._characterLuKangWumiaoOperations = characterLuKangWumiaoOperations;
        this._characterPangJiOlOperations = characterPangJiOlOperations;
        this._characterZhangLiaoMobileGodOperations = characterZhangLiaoMobileGodOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._corePindianOperations = corePindianOperations;
        this._corePindianClaimsOperations = corePindianClaimsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSimultaneousPindianOperations = coreSimultaneousPindianOperations;
        this._coreTableOrderOperations = coreTableOrderOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreResolutionIdentityBoundaryOperations = coreResolutionIdentityBoundaryOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreGameEngineSkillProgramsProgramPhaseUsesAccess = coreGameEngineSkillProgramsProgramPhaseUsesAccess;
    }
}
