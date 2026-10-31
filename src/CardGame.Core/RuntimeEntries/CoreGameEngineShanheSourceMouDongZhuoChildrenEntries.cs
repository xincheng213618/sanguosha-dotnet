#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceMouDongZhuoChildrenRuntime))]
internal sealed partial class CoreGameEngineShanheSourceMouDongZhuoChildrenEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceMouDongZhuoChildrenRuntime> _resolve;
    private CoreGameEngineShanheSourceMouDongZhuoChildrenEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceMouDongZhuoChildrenRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSourceMouDongZhuoChildrenEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceMouDongZhuoChildrenRuntime> resolve) => new(resolve);
}
