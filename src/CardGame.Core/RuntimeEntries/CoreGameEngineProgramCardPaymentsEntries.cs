#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramCardPaymentsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime> _resolve;
    private CoreGameEngineProgramCardPaymentsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramCardPaymentsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime> resolve) => new(resolve);
}
