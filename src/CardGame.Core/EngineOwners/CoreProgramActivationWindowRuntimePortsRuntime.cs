#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreProgramActivationWindowRuntimePortsRuntime : global::CardGame.Core.IProgramActivationWindowFacts, global::CardGame.Core.IProgramActivationWindowReturns
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhangLiaoMobileGodEntries _characterZhangLiaoMobileGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreDamageCursorRuntimeEntries _coreDamageCursorRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineActualUseTargetProgramsEntries _coreActualUseTargetProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAfterTurnEndedProgramsEntries _coreAfterTurnEndedProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardEffectProgramsEntries _coreCardEffectProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDamagePreventionProgramsEntries _coreDamagePreventionProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDeathProgramsEntries _coreDeathProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDeferredHandAlignmentsEntries _coreDeferredHandAlignmentsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineFirstSlashDuelLegacyWindowsEntries _coreFirstSlashDuelLegacyWindowsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineForeignTurnContestsEntries _coreForeignTurnContestsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePindianRevealWindowsEntries _corePindianRevealWindowsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramTriggerWindowsEntries _coreProgramTriggerWindowsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTurnEndingOrderEntries _coreTurnEndingOrderOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDamageWindowsEntries _coreNativeDamageWindowsOperations;
    internal CoreProgramActivationWindowRuntimePortsRuntime(global::CardGame.Core.RuntimeEntries.CharacterZhangLiaoMobileGodEntries characterZhangLiaoMobileGodOperations, global::CardGame.Core.RuntimeEntries.CoreDamageCursorRuntimeEntries coreDamageCursorRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineActualUseTargetProgramsEntries coreActualUseTargetProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAfterTurnEndedProgramsEntries coreAfterTurnEndedProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardEffectProgramsEntries coreCardEffectProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDamagePreventionProgramsEntries coreDamagePreventionProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDeathProgramsEntries coreDeathProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDeferredHandAlignmentsEntries coreDeferredHandAlignmentsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineFirstSlashDuelLegacyWindowsEntries coreFirstSlashDuelLegacyWindowsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineForeignTurnContestsEntries coreForeignTurnContestsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePindianRevealWindowsEntries corePindianRevealWindowsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramTriggerWindowsEntries coreProgramTriggerWindowsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineTurnEndingOrderEntries coreTurnEndingOrderOperations, global::CardGame.Core.RuntimeEntries.CoreNativeDamageWindowsEntries coreNativeDamageWindowsOperations)
    {
        this._characterZhangLiaoMobileGodOperations = characterZhangLiaoMobileGodOperations;
        this._coreDamageCursorRuntimeOperations = coreDamageCursorRuntimeOperations;
        this._coreActualUseTargetProgramsOperations = coreActualUseTargetProgramsOperations;
        this._coreAfterTurnEndedProgramsOperations = coreAfterTurnEndedProgramsOperations;
        this._coreCardEffectProgramsOperations = coreCardEffectProgramsOperations;
        this._coreDamagePreventionProgramsOperations = coreDamagePreventionProgramsOperations;
        this._coreDeathProgramsOperations = coreDeathProgramsOperations;
        this._coreDeferredHandAlignmentsOperations = coreDeferredHandAlignmentsOperations;
        this._coreFirstSlashDuelLegacyWindowsOperations = coreFirstSlashDuelLegacyWindowsOperations;
        this._coreForeignTurnContestsOperations = coreForeignTurnContestsOperations;
        this._corePindianRevealWindowsOperations = corePindianRevealWindowsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreProgramTriggerWindowsOperations = coreProgramTriggerWindowsOperations;
        this._coreTurnEndingOrderOperations = coreTurnEndingOrderOperations;
        this._coreNativeDamageWindowsOperations = coreNativeDamageWindowsOperations;
    }
}
