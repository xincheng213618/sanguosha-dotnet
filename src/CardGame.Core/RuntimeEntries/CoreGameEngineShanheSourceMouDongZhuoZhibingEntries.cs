#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceMouDongZhuoZhibingRuntime))]
internal sealed partial class CoreGameEngineShanheSourceMouDongZhuoZhibingEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceMouDongZhuoZhibingRuntime> _resolve;
    private CoreGameEngineShanheSourceMouDongZhuoZhibingEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceMouDongZhuoZhibingRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSourceMouDongZhuoZhibingEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceMouDongZhuoZhibingRuntime> resolve) => new(resolve);
}
