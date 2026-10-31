#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceWeizhuEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheSourceWeizhuRuntime> _resolve;
    private CoreGameEngineShanheSourceWeizhuEntries(Func<EngineOwners.CoreGameEngineShanheSourceWeizhuRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSourceWeizhuEntries Create(Func<EngineOwners.CoreGameEngineShanheSourceWeizhuRuntime> resolve) => new(resolve);
}
