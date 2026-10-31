#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineLiejieSourceDiscardsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLiejieSourceDiscardsRuntime> _resolve;
    private CoreGameEngineLiejieSourceDiscardsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLiejieSourceDiscardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineLiejieSourceDiscardsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLiejieSourceDiscardsRuntime> resolve) => new(resolve);
}
