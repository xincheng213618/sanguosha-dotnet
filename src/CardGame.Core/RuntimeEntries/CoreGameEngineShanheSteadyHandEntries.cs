#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSteadyHandEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheSteadyHandRuntime> _resolve;
    private CoreGameEngineShanheSteadyHandEntries(Func<EngineOwners.CoreGameEngineShanheSteadyHandRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSteadyHandEntries Create(Func<EngineOwners.CoreGameEngineShanheSteadyHandRuntime> resolve) => new(resolve);
}
