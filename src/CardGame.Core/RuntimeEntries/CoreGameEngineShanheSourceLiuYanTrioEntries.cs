#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceLiuYanTrioRuntime))]
internal sealed partial class CoreGameEngineShanheSourceLiuYanTrioEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceLiuYanTrioRuntime> _resolve;
    private CoreGameEngineShanheSourceLiuYanTrioEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceLiuYanTrioRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSourceLiuYanTrioEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceLiuYanTrioRuntime> resolve) => new(resolve);
}
