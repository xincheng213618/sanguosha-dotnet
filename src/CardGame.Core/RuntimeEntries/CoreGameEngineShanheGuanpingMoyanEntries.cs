#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheGuanpingMoyanEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheGuanpingMoyanRuntime> _resolve;
    private CoreGameEngineShanheGuanpingMoyanEntries(Func<EngineOwners.CoreGameEngineShanheGuanpingMoyanRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheGuanpingMoyanEntries Create(Func<EngineOwners.CoreGameEngineShanheGuanpingMoyanRuntime> resolve) => new(resolve);
}
