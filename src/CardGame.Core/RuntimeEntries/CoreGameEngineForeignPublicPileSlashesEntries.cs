#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineForeignPublicPileSlashesRuntime))]
internal sealed partial class CoreGameEngineForeignPublicPileSlashesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForeignPublicPileSlashesRuntime> _resolve;
    private CoreGameEngineForeignPublicPileSlashesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForeignPublicPileSlashesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineForeignPublicPileSlashesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForeignPublicPileSlashesRuntime> resolve) => new(resolve);
}
