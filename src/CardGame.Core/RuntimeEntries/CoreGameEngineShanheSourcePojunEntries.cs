#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourcePojunEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourcePojunRuntime> _resolve;
    private CoreGameEngineShanheSourcePojunEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourcePojunRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSourcePojunEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourcePojunRuntime> resolve) => new(resolve);
}
