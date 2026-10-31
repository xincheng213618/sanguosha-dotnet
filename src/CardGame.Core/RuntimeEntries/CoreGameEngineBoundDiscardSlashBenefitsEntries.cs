#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineBoundDiscardSlashBenefitsRuntime))]
internal sealed partial class CoreGameEngineBoundDiscardSlashBenefitsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineBoundDiscardSlashBenefitsRuntime> _resolve;
    private CoreGameEngineBoundDiscardSlashBenefitsEntries(Func<EngineOwners.CoreGameEngineBoundDiscardSlashBenefitsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineBoundDiscardSlashBenefitsEntries Create(Func<EngineOwners.CoreGameEngineBoundDiscardSlashBenefitsRuntime> resolve) => new(resolve);
}
