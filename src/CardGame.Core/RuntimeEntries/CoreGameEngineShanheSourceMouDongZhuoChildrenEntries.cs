#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShanheSourceMouDongZhuoChildrenRuntime))]
internal sealed partial class CoreGameEngineShanheSourceMouDongZhuoChildrenEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheSourceMouDongZhuoChildrenRuntime> _resolve;
    private CoreGameEngineShanheSourceMouDongZhuoChildrenEntries(Func<EngineOwners.CoreGameEngineShanheSourceMouDongZhuoChildrenRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSourceMouDongZhuoChildrenEntries Create(Func<EngineOwners.CoreGameEngineShanheSourceMouDongZhuoChildrenRuntime> resolve) => new(resolve);
}
