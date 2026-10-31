#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSoleIncomingRangeDrawEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleIncomingRangeDrawRuntime> _resolve;
    private CoreGameEngineSoleIncomingRangeDrawEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleIncomingRangeDrawRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSoleIncomingRangeDrawEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleIncomingRangeDrawRuntime> resolve) => new(resolve);
}
