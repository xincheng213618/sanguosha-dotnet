#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroResponsesRuntime))]
internal sealed partial class CoreGameEngineTieredRoundZeroResponsesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroResponsesRuntime> _resolve;
    private CoreGameEngineTieredRoundZeroResponsesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroResponsesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineTieredRoundZeroResponsesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroResponsesRuntime> resolve) => new(resolve);
}
