#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShanheSourceLiuYanTrioRuntime))]
internal sealed partial class CoreGameEngineShanheSourceLiuYanTrioEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheSourceLiuYanTrioRuntime> _resolve;
    private CoreGameEngineShanheSourceLiuYanTrioEntries(Func<EngineOwners.CoreGameEngineShanheSourceLiuYanTrioRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSourceLiuYanTrioEntries Create(Func<EngineOwners.CoreGameEngineShanheSourceLiuYanTrioRuntime> resolve) => new(resolve);
}
