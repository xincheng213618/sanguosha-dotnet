#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineTieredRoundZeroUsesRuntime))]
internal sealed partial class CoreGameEngineTieredRoundZeroUsesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineTieredRoundZeroUsesRuntime> _resolve;
    private CoreGameEngineTieredRoundZeroUsesEntries(Func<EngineOwners.CoreGameEngineTieredRoundZeroUsesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineTieredRoundZeroUsesEntries Create(Func<EngineOwners.CoreGameEngineTieredRoundZeroUsesRuntime> resolve) => new(resolve);
}
