#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

internal interface ICoreRuleQueriesMatchCardTargetQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.MatchCardTargetQueries Get();
}

internal interface ICoreRuleQueriesMatchHandLimitQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.MatchHandLimitQueries Get();
}

internal interface ICoreRuleQueriesProgramNumericContributionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramNumericContributionRuntime Get();
}

internal interface ICoreRuleQueriesMatchTurnNumericQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.MatchTurnNumericQueries Get();
}

internal interface ICoreRuleQueriesCoreProgramTargetRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramTargetRules TargetRules { get; }
}

internal interface ICoreRuleQueriesMatchDistanceQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.MatchDistanceQueries Get();
}
