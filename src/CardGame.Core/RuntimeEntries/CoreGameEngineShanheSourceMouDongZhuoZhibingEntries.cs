#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShanheSourceMouDongZhuoZhibingRuntime))]
internal sealed partial class CoreGameEngineShanheSourceMouDongZhuoZhibingEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheSourceMouDongZhuoZhibingRuntime> _resolve;
    private CoreGameEngineShanheSourceMouDongZhuoZhibingEntries(Func<EngineOwners.CoreGameEngineShanheSourceMouDongZhuoZhibingRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSourceMouDongZhuoZhibingEntries Create(Func<EngineOwners.CoreGameEngineShanheSourceMouDongZhuoZhibingRuntime> resolve) => new(resolve);
}
