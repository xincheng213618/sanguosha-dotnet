#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFactionRequestPaymentsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFactionRequestPaymentsRuntime> _resolve;
    private CoreGameEngineFactionRequestPaymentsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFactionRequestPaymentsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFactionRequestPaymentsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFactionRequestPaymentsRuntime> resolve) => new(resolve);
}
