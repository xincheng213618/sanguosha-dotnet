#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineKenshangPaymentsRuntime))]
internal sealed partial class CoreGameEngineKenshangPaymentsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineKenshangPaymentsRuntime> _resolve;
    private CoreGameEngineKenshangPaymentsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineKenshangPaymentsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineKenshangPaymentsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineKenshangPaymentsRuntime> resolve) => new(resolve);
}
