#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePindianRandomSelectionsRuntime))]
internal sealed partial class CoreGameEnginePindianRandomSelectionsEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePindianRandomSelectionsRuntime> _resolve;
    private CoreGameEnginePindianRandomSelectionsEntries(Func<EngineOwners.CoreGameEnginePindianRandomSelectionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePindianRandomSelectionsEntries Create(Func<EngineOwners.CoreGameEnginePindianRandomSelectionsRuntime> resolve) => new(resolve);
}
