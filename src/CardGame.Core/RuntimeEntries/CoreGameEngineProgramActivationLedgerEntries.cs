#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramActivationLedgerEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramActivationLedgerRuntime> _resolve;
    private CoreGameEngineProgramActivationLedgerEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramActivationLedgerRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramActivationLedgerEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramActivationLedgerRuntime> resolve) => new(resolve);
}
