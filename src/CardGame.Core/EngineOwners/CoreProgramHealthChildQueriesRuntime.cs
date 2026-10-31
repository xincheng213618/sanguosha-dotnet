#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreProgramHealthChildQueriesRuntime : global::CardGame.Core.IProgramHealthChildOperations, global::CardGame.Core.IProgramHealthChildQueries
{
    private readonly ICoreProgramHealthChildQueriesResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreProgramHealthChildQueriesCoreDyingCursorRuntimeOperations _coreDyingCursorRuntimeOperations;
    private readonly ICoreProgramHealthChildQueriesCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICoreProgramHealthChildQueriesCoreObserverSubtreesOperations _coreObserverSubtreesOperations;
    private readonly ICoreProgramHealthChildQueriesCoreProgramRecoveryReceiptsOperations _coreProgramRecoveryReceiptsOperations;
    private readonly ICoreProgramHealthChildQueriesCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineHpChangeProgramsPendingHpChangesAccess _coreGameEngineHpChangeProgramsPendingHpChangesAccess;
    internal CoreProgramHealthChildQueriesRuntime(ICoreProgramHealthChildQueriesResolutionStackOperations resolutionStackOperations, ICoreProgramHealthChildQueriesCoreDyingCursorRuntimeOperations coreDyingCursorRuntimeOperations, ICoreProgramHealthChildQueriesCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICoreProgramHealthChildQueriesCoreObserverSubtreesOperations coreObserverSubtreesOperations, ICoreProgramHealthChildQueriesCoreProgramRecoveryReceiptsOperations coreProgramRecoveryReceiptsOperations, ICoreProgramHealthChildQueriesCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineHpChangeProgramsPendingHpChangesAccess coreGameEngineHpChangeProgramsPendingHpChangesAccess)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreObserverSubtreesOperations = coreObserverSubtreesOperations;
        this._coreProgramRecoveryReceiptsOperations = coreProgramRecoveryReceiptsOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._coreGameEngineHpChangeProgramsPendingHpChangesAccess = coreGameEngineHpChangeProgramsPendingHpChangesAccess;
    }
}
