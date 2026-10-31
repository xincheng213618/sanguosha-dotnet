#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineTurnDrawDebtsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnDrawDebtsRuntime> _resolve;
    private CoreGameEngineTurnDrawDebtsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnDrawDebtsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineTurnDrawDebtsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnDrawDebtsRuntime> resolve) => new(resolve);
}
