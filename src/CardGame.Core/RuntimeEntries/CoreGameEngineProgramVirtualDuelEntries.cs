#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramVirtualDuelEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramVirtualDuelRuntime> _resolve;
    private CoreGameEngineProgramVirtualDuelEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramVirtualDuelRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramVirtualDuelEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramVirtualDuelRuntime> resolve) => new(resolve);
}
