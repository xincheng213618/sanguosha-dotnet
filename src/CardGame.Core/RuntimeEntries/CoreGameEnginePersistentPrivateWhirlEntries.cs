#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePersistentPrivateWhirlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePersistentPrivateWhirlRuntime> _resolve;
    private CoreGameEnginePersistentPrivateWhirlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePersistentPrivateWhirlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePersistentPrivateWhirlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePersistentPrivateWhirlRuntime> resolve) => new(resolve);
}
