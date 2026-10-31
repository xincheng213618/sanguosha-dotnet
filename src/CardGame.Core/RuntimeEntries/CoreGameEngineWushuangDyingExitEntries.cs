#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangDyingExitEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangDyingExitRuntime> _resolve;
    private CoreGameEngineWushuangDyingExitEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangDyingExitRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWushuangDyingExitEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangDyingExitRuntime> resolve) => new(resolve);
}
