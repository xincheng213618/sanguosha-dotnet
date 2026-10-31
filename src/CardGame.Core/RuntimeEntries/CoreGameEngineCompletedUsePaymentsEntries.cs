#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCompletedUsePaymentsRuntime))]
internal sealed partial class CoreGameEngineCompletedUsePaymentsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCompletedUsePaymentsRuntime> _resolve;
    private CoreGameEngineCompletedUsePaymentsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCompletedUsePaymentsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCompletedUsePaymentsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCompletedUsePaymentsRuntime> resolve) => new(resolve);
}
