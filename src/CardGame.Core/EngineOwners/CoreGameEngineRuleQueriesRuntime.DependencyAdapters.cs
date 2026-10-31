#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.MatchCardTargetQueries))]
internal sealed class CoreRuleQueriesMatchCardTargetQueriesAdapter(global::System.Func<global::CardGame.Core.MatchCardTargetQueries> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuleQueriesMatchCardTargetQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.MatchCardTargetQueries> _targetFactory = targetFactory;
    public global::CardGame.Core.MatchCardTargetQueries Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.MatchHandLimitQueries))]
internal sealed class CoreRuleQueriesMatchHandLimitQueriesAdapter(global::System.Func<global::CardGame.Core.MatchHandLimitQueries> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuleQueriesMatchHandLimitQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.MatchHandLimitQueries> _targetFactory = targetFactory;
    public global::CardGame.Core.MatchHandLimitQueries Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramNumericContributionRuntime))]
internal sealed class CoreRuleQueriesProgramNumericContributionAdapter(global::System.Func<global::CardGame.Core.ProgramNumericContributionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuleQueriesProgramNumericContributionOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramNumericContributionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramNumericContributionRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.MatchTurnNumericQueries))]
internal sealed class CoreRuleQueriesMatchTurnNumericQueriesAdapter(global::System.Func<global::CardGame.Core.MatchTurnNumericQueries> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuleQueriesMatchTurnNumericQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.MatchTurnNumericQueries> _targetFactory = targetFactory;
    public global::CardGame.Core.MatchTurnNumericQueries Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramTargetRulesRuntime))]
internal sealed class CoreRuleQueriesCoreProgramTargetRulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramTargetRulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuleQueriesCoreProgramTargetRulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramTargetRulesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramTargetRules TargetRules { get => this._targetFactory().TargetRules; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.MatchDistanceQueries))]
internal sealed class CoreRuleQueriesMatchDistanceQueriesAdapter(global::System.Func<global::CardGame.Core.MatchDistanceQueries> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuleQueriesMatchDistanceQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.MatchDistanceQueries> _targetFactory = targetFactory;
    public global::CardGame.Core.MatchDistanceQueries Get() => this._targetFactory();
}
