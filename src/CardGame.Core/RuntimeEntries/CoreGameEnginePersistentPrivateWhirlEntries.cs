#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePersistentPrivateWhirlRuntime))]
internal sealed partial class CoreGameEnginePersistentPrivateWhirlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePersistentPrivateWhirlRuntime> _resolve;
    private CoreGameEnginePersistentPrivateWhirlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePersistentPrivateWhirlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePersistentPrivateWhirlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePersistentPrivateWhirlRuntime> resolve) => new(resolve);
}
