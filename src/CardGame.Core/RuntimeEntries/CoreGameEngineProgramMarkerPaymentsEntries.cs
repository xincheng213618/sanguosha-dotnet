#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramMarkerPaymentsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramMarkerPaymentsRuntime> _resolve;
    private CoreGameEngineProgramMarkerPaymentsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramMarkerPaymentsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramMarkerPaymentsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramMarkerPaymentsRuntime> resolve) => new(resolve);
}
