#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroUsesRuntime))]
internal sealed partial class CoreGameEngineTieredRoundZeroUsesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroUsesRuntime> _resolve;
    private CoreGameEngineTieredRoundZeroUsesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroUsesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineTieredRoundZeroUsesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroUsesRuntime> resolve) => new(resolve);
}
