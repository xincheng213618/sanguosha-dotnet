#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramMarkerPaymentsRuntime))]
internal sealed partial class CoreGameEngineProgramMarkerPaymentsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramMarkerPaymentsRuntime> _resolve;
    private CoreGameEngineProgramMarkerPaymentsEntries(Func<EngineOwners.CoreGameEngineProgramMarkerPaymentsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramMarkerPaymentsEntries Create(Func<EngineOwners.CoreGameEngineProgramMarkerPaymentsRuntime> resolve) => new(resolve);
}
