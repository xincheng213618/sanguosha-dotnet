#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineMantianGuohaiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMantianGuohaiRuntime> _resolve;
    private CoreGameEngineMantianGuohaiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMantianGuohaiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineMantianGuohaiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMantianGuohaiRuntime> resolve) => new(resolve);
}
