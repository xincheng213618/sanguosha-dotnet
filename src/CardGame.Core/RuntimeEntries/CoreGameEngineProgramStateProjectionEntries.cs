#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramStateProjectionEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramStateProjectionRuntime> _resolve;
    private CoreGameEngineProgramStateProjectionEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramStateProjectionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramStateProjectionEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramStateProjectionRuntime> resolve) => new(resolve);
}
