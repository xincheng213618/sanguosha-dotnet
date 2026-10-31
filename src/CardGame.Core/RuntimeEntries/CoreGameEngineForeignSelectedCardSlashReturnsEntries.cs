#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineForeignSelectedCardSlashReturnsRuntime))]
internal sealed partial class CoreGameEngineForeignSelectedCardSlashReturnsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForeignSelectedCardSlashReturnsRuntime> _resolve;
    private CoreGameEngineForeignSelectedCardSlashReturnsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForeignSelectedCardSlashReturnsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineForeignSelectedCardSlashReturnsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForeignSelectedCardSlashReturnsRuntime> resolve) => new(resolve);
}
