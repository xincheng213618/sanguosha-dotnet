#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceWeizhuEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceWeizhuRuntime> _resolve;
    private CoreGameEngineShanheSourceWeizhuEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceWeizhuRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSourceWeizhuEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceWeizhuRuntime> resolve) => new(resolve);
}
