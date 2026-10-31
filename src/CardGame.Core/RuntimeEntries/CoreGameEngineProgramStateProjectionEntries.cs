#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramStateProjectionEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramStateProjectionRuntime> _resolve;
    private CoreGameEngineProgramStateProjectionEntries(Func<EngineOwners.CoreGameEngineProgramStateProjectionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramStateProjectionEntries Create(Func<EngineOwners.CoreGameEngineProgramStateProjectionRuntime> resolve) => new(resolve);
}
