#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePindianRuntime))]
internal sealed partial class CoreGameEnginePindianEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePindianRuntime> _resolve;
    private CoreGameEnginePindianEntries(Func<EngineOwners.CoreGameEnginePindianRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePindianEntries Create(Func<EngineOwners.CoreGameEnginePindianRuntime> resolve) => new(resolve);
}
