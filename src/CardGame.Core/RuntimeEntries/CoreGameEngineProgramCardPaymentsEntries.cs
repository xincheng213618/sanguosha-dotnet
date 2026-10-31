#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramCardPaymentsRuntime))]
internal sealed partial class CoreGameEngineProgramCardPaymentsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramCardPaymentsRuntime> _resolve;
    private CoreGameEngineProgramCardPaymentsEntries(Func<EngineOwners.CoreGameEngineProgramCardPaymentsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramCardPaymentsEntries Create(Func<EngineOwners.CoreGameEngineProgramCardPaymentsRuntime> resolve) => new(resolve);
}
