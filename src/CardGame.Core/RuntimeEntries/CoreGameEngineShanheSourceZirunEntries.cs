#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceZirunEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceZirunRuntime> _resolve;
    private CoreGameEngineShanheSourceZirunEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceZirunRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSourceZirunEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceZirunRuntime> resolve) => new(resolve);
}
