#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShanheSourceChunlao3161Runtime))]
internal sealed partial class CoreGameEngineShanheSourceChunlao3161Entries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheSourceChunlao3161Runtime> _resolve;
    private CoreGameEngineShanheSourceChunlao3161Entries(Func<EngineOwners.CoreGameEngineShanheSourceChunlao3161Runtime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSourceChunlao3161Entries Create(Func<EngineOwners.CoreGameEngineShanheSourceChunlao3161Runtime> resolve) => new(resolve);
}
