#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramExpressionsRuntime))]
internal sealed partial class CoreGameEngineProgramExpressionsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramExpressionsRuntime> _resolve;
    private CoreGameEngineProgramExpressionsEntries(Func<EngineOwners.CoreGameEngineProgramExpressionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramExpressionsEntries Create(Func<EngineOwners.CoreGameEngineProgramExpressionsRuntime> resolve) => new(resolve);
}
