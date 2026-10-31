#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineForeignPublicPileSlashesRuntime))]
internal sealed partial class CoreGameEngineForeignPublicPileSlashesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineForeignPublicPileSlashesRuntime> _resolve;
    private CoreGameEngineForeignPublicPileSlashesEntries(Func<EngineOwners.CoreGameEngineForeignPublicPileSlashesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineForeignPublicPileSlashesEntries Create(Func<EngineOwners.CoreGameEngineForeignPublicPileSlashesRuntime> resolve) => new(resolve);
}
