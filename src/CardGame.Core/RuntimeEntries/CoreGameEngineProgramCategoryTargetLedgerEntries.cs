#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramCategoryTargetLedgerEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCategoryTargetLedgerRuntime> _resolve;
    private CoreGameEngineProgramCategoryTargetLedgerEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCategoryTargetLedgerRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramCategoryTargetLedgerEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCategoryTargetLedgerRuntime> resolve) => new(resolve);
}
