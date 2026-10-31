#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramContinuationPermissionsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramContinuationPermissionsRuntime> _resolve;
    private CoreGameEngineProgramContinuationPermissionsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramContinuationPermissionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramContinuationPermissionsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramContinuationPermissionsRuntime> resolve) => new(resolve);
}
