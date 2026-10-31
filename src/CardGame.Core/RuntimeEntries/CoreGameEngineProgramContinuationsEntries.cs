#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramContinuationsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramContinuationsRuntime> _resolve;
    private CoreGameEngineProgramContinuationsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramContinuationsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramContinuationsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramContinuationsRuntime> resolve) => new(resolve);
}
