#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineFirstSlashDuelBenefitsRuntime))]
internal sealed partial class CoreGameEngineFirstSlashDuelBenefitsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineFirstSlashDuelBenefitsRuntime> _resolve;
    private CoreGameEngineFirstSlashDuelBenefitsEntries(Func<EngineOwners.CoreGameEngineFirstSlashDuelBenefitsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFirstSlashDuelBenefitsEntries Create(Func<EngineOwners.CoreGameEngineFirstSlashDuelBenefitsRuntime> resolve) => new(resolve);
}
