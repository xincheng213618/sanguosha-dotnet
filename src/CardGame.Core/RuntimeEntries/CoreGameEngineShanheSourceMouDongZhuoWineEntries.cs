#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceMouDongZhuoWineRuntime))]
internal sealed partial class CoreGameEngineShanheSourceMouDongZhuoWineEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceMouDongZhuoWineRuntime> _resolve;
    private CoreGameEngineShanheSourceMouDongZhuoWineEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceMouDongZhuoWineRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSourceMouDongZhuoWineEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceMouDongZhuoWineRuntime> resolve) => new(resolve);
}
