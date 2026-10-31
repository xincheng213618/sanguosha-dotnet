#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSteadyHandEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSteadyHandRuntime> _resolve;
    private CoreGameEngineShanheSteadyHandEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSteadyHandRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSteadyHandEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSteadyHandRuntime> resolve) => new(resolve);
}
