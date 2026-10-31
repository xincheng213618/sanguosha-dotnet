#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreNativeHealthRecoveryServicesRuntime : global::CardGame.Core.INativeHealthRecoveryFacts, global::CardGame.Core.INativeHealthRecoveryProtocols
{
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreDamageObserverCursorQueriesEntries _coreDamageObserverCursorQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries _coreAlternatingBenefitsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPaymentsEntries _coreProgramCardPaymentsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramRecoveryReceiptsEntries _coreProgramRecoveryReceiptsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRecoveryReplacementDamageRidesEntries _coreRecoveryReplacementDamageRidesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTypedRecoveryDamageCursorEntries _coreTypedRecoveryDamageCursorOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreHulaoActivityProgramsEntries _coreHulaoActivityProgramsOperations;
    internal CoreNativeHealthRecoveryServicesRuntime(global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreDamageObserverCursorQueriesEntries coreDamageObserverCursorQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries coreAlternatingBenefitsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPaymentsEntries coreProgramCardPaymentsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramRecoveryReceiptsEntries coreProgramRecoveryReceiptsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRecoveryReplacementDamageRidesEntries coreRecoveryReplacementDamageRidesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineTypedRecoveryDamageCursorEntries coreTypedRecoveryDamageCursorOperations, global::CardGame.Core.RuntimeEntries.CoreHulaoActivityProgramsEntries coreHulaoActivityProgramsOperations)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDamageObserverCursorQueriesOperations = coreDamageObserverCursorQueriesOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreProgramCardPaymentsOperations = coreProgramCardPaymentsOperations;
        this._coreProgramRecoveryReceiptsOperations = coreProgramRecoveryReceiptsOperations;
        this._coreRecoveryReplacementDamageRidesOperations = coreRecoveryReplacementDamageRidesOperations;
        this._coreTypedRecoveryDamageCursorOperations = coreTypedRecoveryDamageCursorOperations;
        this._coreHulaoActivityProgramsOperations = coreHulaoActivityProgramsOperations;
    }
}
