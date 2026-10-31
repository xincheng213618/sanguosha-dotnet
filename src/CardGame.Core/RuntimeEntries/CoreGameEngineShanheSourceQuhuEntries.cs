#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceQuhuEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceQuhuRuntime> _resolve;
    private CoreGameEngineShanheSourceQuhuEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceQuhuRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSourceQuhuEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceQuhuRuntime> resolve) => new(resolve);
}
