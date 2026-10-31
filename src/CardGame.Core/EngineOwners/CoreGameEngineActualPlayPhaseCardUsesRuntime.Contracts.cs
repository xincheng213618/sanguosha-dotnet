#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreActualPlayPhaseCardUsesCorePhaseNamePredictionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ObservePhaseNamePredictionUse(global::CardGame.Core.CardActionContext action, bool qualifiedActualUse = false);
}

internal interface ICoreActualPlayPhaseCardUsesContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreActualPlayPhaseCardUsesCoreAlternatingSuitAndCompletedTopOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordFirstTurnCategoryUse(global::CardGame.Core.CardActionContext action);
}

internal interface ICoreActualPlayPhaseCardUsesKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreActualPlayPhaseCardUsesCoreEndingHistoricalUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordOwnPlayEligibleUse(global::CardGame.Core.CardActionContext action);
}
