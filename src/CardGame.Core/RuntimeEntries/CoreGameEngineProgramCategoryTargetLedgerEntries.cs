#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramCategoryTargetLedgerRuntime))]
internal sealed partial class CoreGameEngineProgramCategoryTargetLedgerEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramCategoryTargetLedgerRuntime> _resolve;
    private CoreGameEngineProgramCategoryTargetLedgerEntries(Func<EngineOwners.CoreGameEngineProgramCategoryTargetLedgerRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramCategoryTargetLedgerEntries Create(Func<EngineOwners.CoreGameEngineProgramCategoryTargetLedgerRuntime> resolve) => new(resolve);
}
