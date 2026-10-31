#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceMouDongZhuoRuntime))]
internal sealed partial class CoreGameEngineShanheSourceMouDongZhuoEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceMouDongZhuoRuntime> _resolve;
    private CoreGameEngineShanheSourceMouDongZhuoEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceMouDongZhuoRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSourceMouDongZhuoEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceMouDongZhuoRuntime> resolve) => new(resolve);
}
