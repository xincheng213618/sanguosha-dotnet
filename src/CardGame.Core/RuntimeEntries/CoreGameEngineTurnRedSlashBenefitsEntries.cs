#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineTurnRedSlashBenefitsRuntime))]
internal sealed partial class CoreGameEngineTurnRedSlashBenefitsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineTurnRedSlashBenefitsRuntime> _resolve;
    private CoreGameEngineTurnRedSlashBenefitsEntries(Func<EngineOwners.CoreGameEngineTurnRedSlashBenefitsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineTurnRedSlashBenefitsEntries Create(Func<EngineOwners.CoreGameEngineTurnRedSlashBenefitsRuntime> resolve) => new(resolve);
}
