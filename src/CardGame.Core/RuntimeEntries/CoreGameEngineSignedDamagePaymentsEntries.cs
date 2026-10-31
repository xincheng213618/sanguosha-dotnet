#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSignedDamagePaymentsRuntime))]
internal sealed partial class CoreGameEngineSignedDamagePaymentsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSignedDamagePaymentsRuntime> _resolve;
    private CoreGameEngineSignedDamagePaymentsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSignedDamagePaymentsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSignedDamagePaymentsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSignedDamagePaymentsRuntime> resolve) => new(resolve);
}
