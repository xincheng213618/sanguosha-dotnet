#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramContinuationPermissionsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramContinuationPermissionsRuntime> _resolve;
    private CoreGameEngineProgramContinuationPermissionsEntries(Func<EngineOwners.CoreGameEngineProgramContinuationPermissionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramContinuationPermissionsEntries Create(Func<EngineOwners.CoreGameEngineProgramContinuationPermissionsRuntime> resolve) => new(resolve);
}
