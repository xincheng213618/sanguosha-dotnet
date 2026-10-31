#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramActivationLedgerEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramActivationLedgerRuntime> _resolve;
    private CoreGameEngineProgramActivationLedgerEntries(Func<EngineOwners.CoreGameEngineProgramActivationLedgerRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramActivationLedgerEntries Create(Func<EngineOwners.CoreGameEngineProgramActivationLedgerRuntime> resolve) => new(resolve);
}
