#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDeferredHandAlignmentsRuntime))]
internal sealed partial class CoreGameEngineDeferredHandAlignmentsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDeferredHandAlignmentsRuntime> _resolve;
    private CoreGameEngineDeferredHandAlignmentsEntries(Func<EngineOwners.CoreGameEngineDeferredHandAlignmentsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDeferredHandAlignmentsEntries Create(Func<EngineOwners.CoreGameEngineDeferredHandAlignmentsRuntime> resolve) => new(resolve);
}
