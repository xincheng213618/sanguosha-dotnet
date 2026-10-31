#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShanheSourceMouDongZhuoRuntime))]
internal sealed partial class CoreGameEngineShanheSourceMouDongZhuoEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheSourceMouDongZhuoRuntime> _resolve;
    private CoreGameEngineShanheSourceMouDongZhuoEntries(Func<EngineOwners.CoreGameEngineShanheSourceMouDongZhuoRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSourceMouDongZhuoEntries Create(Func<EngineOwners.CoreGameEngineShanheSourceMouDongZhuoRuntime> resolve) => new(resolve);
}
