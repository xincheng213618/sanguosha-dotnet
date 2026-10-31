#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePindianHandReturnsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianHandReturnsRuntime> _resolve;
    private CoreGameEnginePindianHandReturnsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianHandReturnsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePindianHandReturnsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianHandReturnsRuntime> resolve) => new(resolve);
}
