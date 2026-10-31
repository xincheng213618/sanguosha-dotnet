#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePindianEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianRuntime> _resolve;
    private CoreGameEnginePindianEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePindianEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianRuntime> resolve) => new(resolve);
}
