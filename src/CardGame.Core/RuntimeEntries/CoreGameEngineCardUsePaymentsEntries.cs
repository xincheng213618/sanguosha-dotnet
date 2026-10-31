#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUsePaymentsRuntime))]
internal sealed partial class CoreGameEngineCardUsePaymentsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUsePaymentsRuntime> _resolve;
    private CoreGameEngineCardUsePaymentsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUsePaymentsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCardUsePaymentsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUsePaymentsRuntime> resolve) => new(resolve);
}
