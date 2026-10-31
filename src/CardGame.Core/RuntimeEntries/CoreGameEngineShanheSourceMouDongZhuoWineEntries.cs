#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShanheSourceMouDongZhuoWineRuntime))]
internal sealed partial class CoreGameEngineShanheSourceMouDongZhuoWineEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheSourceMouDongZhuoWineRuntime> _resolve;
    private CoreGameEngineShanheSourceMouDongZhuoWineEntries(Func<EngineOwners.CoreGameEngineShanheSourceMouDongZhuoWineRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSourceMouDongZhuoWineEntries Create(Func<EngineOwners.CoreGameEngineShanheSourceMouDongZhuoWineRuntime> resolve) => new(resolve);
}
