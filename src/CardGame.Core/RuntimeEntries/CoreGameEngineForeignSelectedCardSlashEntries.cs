#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineForeignSelectedCardSlashEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForeignSelectedCardSlashRuntime> _resolve;
    private CoreGameEngineForeignSelectedCardSlashEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForeignSelectedCardSlashRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineForeignSelectedCardSlashEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForeignSelectedCardSlashRuntime> resolve) => new(resolve);
}
