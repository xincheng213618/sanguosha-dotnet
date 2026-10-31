#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePhaseNamePredictionRuntime))]
internal sealed class CoreActualPlayPhaseCardUsesCorePhaseNamePredictionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseNamePredictionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualPlayPhaseCardUsesCorePhaseNamePredictionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseNamePredictionRuntime> _targetFactory = targetFactory;
    public void ObservePhaseNamePredictionUse(global::CardGame.Core.CardActionContext action, bool qualifiedActualUse = false) => this._targetFactory().ObservePhaseNamePredictionUse(action, qualifiedActualUse);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingSuitAndCompletedTopRuntime))]
internal sealed class CoreActualPlayPhaseCardUsesCoreAlternatingSuitAndCompletedTopAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingSuitAndCompletedTopRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualPlayPhaseCardUsesCoreAlternatingSuitAndCompletedTopOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingSuitAndCompletedTopRuntime> _targetFactory = targetFactory;
    public void RecordFirstTurnCategoryUse(global::CardGame.Core.CardActionContext action) => this._targetFactory().RecordFirstTurnCategoryUse(action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEndingHistoricalUsesRuntime))]
internal sealed class CoreActualPlayPhaseCardUsesCoreEndingHistoricalUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEndingHistoricalUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActualPlayPhaseCardUsesCoreEndingHistoricalUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEndingHistoricalUsesRuntime> _targetFactory = targetFactory;
    public void RecordOwnPlayEligibleUse(global::CardGame.Core.CardActionContext action) => this._targetFactory().RecordOwnPlayEligibleUse(action);
}
