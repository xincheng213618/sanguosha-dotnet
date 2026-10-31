#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheGuanpingMoyanEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheGuanpingMoyanRuntime> _resolve;
    private CoreGameEngineShanheGuanpingMoyanEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheGuanpingMoyanRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheGuanpingMoyanEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheGuanpingMoyanRuntime> resolve) => new(resolve);
}
