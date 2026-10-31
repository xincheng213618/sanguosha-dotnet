#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWushuangDyingExitRuntime))]
internal sealed partial class CoreGameEngineWushuangDyingExitEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangDyingExitRuntime> _resolve;
    private CoreGameEngineWushuangDyingExitEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangDyingExitRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWushuangDyingExitEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangDyingExitRuntime> resolve) => new(resolve);
}
