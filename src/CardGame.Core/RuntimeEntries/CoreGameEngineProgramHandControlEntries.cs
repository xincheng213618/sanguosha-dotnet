#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramHandControlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramHandControlRuntime> _resolve;
    private CoreGameEngineProgramHandControlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramHandControlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramHandControlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramHandControlRuntime> resolve) => new(resolve);
}
