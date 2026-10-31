#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceXiyingEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceXiyingRuntime> _resolve;
    private CoreGameEngineShanheSourceXiyingEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceXiyingRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSourceXiyingEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceXiyingRuntime> resolve) => new(resolve);
}
