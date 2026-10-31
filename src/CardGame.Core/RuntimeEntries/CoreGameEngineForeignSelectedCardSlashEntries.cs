#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineForeignSelectedCardSlashRuntime))]
internal sealed partial class CoreGameEngineForeignSelectedCardSlashEntries
{
    private readonly Func<EngineOwners.CoreGameEngineForeignSelectedCardSlashRuntime> _resolve;
    private CoreGameEngineForeignSelectedCardSlashEntries(Func<EngineOwners.CoreGameEngineForeignSelectedCardSlashRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineForeignSelectedCardSlashEntries Create(Func<EngineOwners.CoreGameEngineForeignSelectedCardSlashRuntime> resolve) => new(resolve);
}
