#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineForeignSelectedCardSlashReturnsRuntime))]
internal sealed partial class CoreGameEngineForeignSelectedCardSlashReturnsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineForeignSelectedCardSlashReturnsRuntime> _resolve;
    private CoreGameEngineForeignSelectedCardSlashReturnsEntries(Func<EngineOwners.CoreGameEngineForeignSelectedCardSlashReturnsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineForeignSelectedCardSlashReturnsEntries Create(Func<EngineOwners.CoreGameEngineForeignSelectedCardSlashReturnsRuntime> resolve) => new(resolve);
}
