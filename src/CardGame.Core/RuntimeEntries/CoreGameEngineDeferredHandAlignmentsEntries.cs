#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDeferredHandAlignmentsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeferredHandAlignmentsRuntime> _resolve;
    private CoreGameEngineDeferredHandAlignmentsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeferredHandAlignmentsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDeferredHandAlignmentsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeferredHandAlignmentsRuntime> resolve) => new(resolve);
}
