#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(MatchCardTargetQueries))]
internal sealed class CoreRuleQueriesMatchCardTargetQueriesAdapter(Func<MatchCardTargetQueries> targetFactory) : ICoreRuleQueriesMatchCardTargetQueriesOperations
{
    private readonly Func<MatchCardTargetQueries> _targetFactory = targetFactory;
    public MatchCardTargetQueries Get() => this._targetFactory();
}

[Core.ProgramCapability(typeof(MatchHandLimitQueries))]
internal sealed class CoreRuleQueriesMatchHandLimitQueriesAdapter(Func<MatchHandLimitQueries> targetFactory) : ICoreRuleQueriesMatchHandLimitQueriesOperations
{
    private readonly Func<MatchHandLimitQueries> _targetFactory = targetFactory;
    public MatchHandLimitQueries Get() => this._targetFactory();
}

[Core.ProgramCapability(typeof(ProgramNumericContributionRuntime))]
internal sealed class CoreRuleQueriesProgramNumericContributionAdapter(Func<ProgramNumericContributionRuntime> targetFactory) : ICoreRuleQueriesProgramNumericContributionOperations
{
    private readonly Func<ProgramNumericContributionRuntime> _targetFactory = targetFactory;
    public ProgramNumericContributionRuntime Get() => this._targetFactory();
}

[Core.ProgramCapability(typeof(MatchTurnNumericQueries))]
internal sealed class CoreRuleQueriesMatchTurnNumericQueriesAdapter(Func<MatchTurnNumericQueries> targetFactory) : ICoreRuleQueriesMatchTurnNumericQueriesOperations
{
    private readonly Func<MatchTurnNumericQueries> _targetFactory = targetFactory;
    public MatchTurnNumericQueries Get() => this._targetFactory();
}

[Core.ProgramCapability(typeof(CoreProgramTargetRulesRuntime))]
internal sealed class CoreRuleQueriesCoreProgramTargetRulesAdapter(Func<CoreProgramTargetRulesRuntime> targetFactory) : ICoreRuleQueriesCoreProgramTargetRulesOperations
{
    private readonly Func<CoreProgramTargetRulesRuntime> _targetFactory = targetFactory;
    public ProgramTargetRules TargetRules { get => this._targetFactory().TargetRules; }
}

[Core.ProgramCapability(typeof(MatchDistanceQueries))]
internal sealed class CoreRuleQueriesMatchDistanceQueriesAdapter(Func<MatchDistanceQueries> targetFactory) : ICoreRuleQueriesMatchDistanceQueriesOperations
{
    private readonly Func<MatchDistanceQueries> _targetFactory = targetFactory;
    public MatchDistanceQueries Get() => this._targetFactory();
}
