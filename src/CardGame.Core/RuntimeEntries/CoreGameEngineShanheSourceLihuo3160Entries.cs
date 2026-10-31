#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShanheSourceLihuo3160Runtime))]
internal sealed partial class CoreGameEngineShanheSourceLihuo3160Entries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheSourceLihuo3160Runtime> _resolve;
    private CoreGameEngineShanheSourceLihuo3160Entries(Func<EngineOwners.CoreGameEngineShanheSourceLihuo3160Runtime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSourceLihuo3160Entries Create(Func<EngineOwners.CoreGameEngineShanheSourceLihuo3160Runtime> resolve) => new(resolve);
}
