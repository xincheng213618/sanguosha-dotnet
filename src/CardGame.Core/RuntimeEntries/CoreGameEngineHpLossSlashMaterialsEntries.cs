#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineHpLossSlashMaterialsRuntime))]
internal sealed partial class CoreGameEngineHpLossSlashMaterialsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineHpLossSlashMaterialsRuntime> _resolve;
    private CoreGameEngineHpLossSlashMaterialsEntries(Func<EngineOwners.CoreGameEngineHpLossSlashMaterialsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineHpLossSlashMaterialsEntries Create(Func<EngineOwners.CoreGameEngineHpLossSlashMaterialsRuntime> resolve) => new(resolve);
}
