#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineKuangfuEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineKuangfuRuntime> _resolve;
    private CoreGameEngineKuangfuEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineKuangfuRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineKuangfuEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineKuangfuRuntime> resolve) => new(resolve);
}
