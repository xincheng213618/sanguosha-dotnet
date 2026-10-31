#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineWushuangDyingExitRuntime))]
internal sealed partial class CoreGameEngineWushuangDyingExitEntries
{
    private readonly Func<EngineOwners.CoreGameEngineWushuangDyingExitRuntime> _resolve;
    private CoreGameEngineWushuangDyingExitEntries(Func<EngineOwners.CoreGameEngineWushuangDyingExitRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWushuangDyingExitEntries Create(Func<EngineOwners.CoreGameEngineWushuangDyingExitRuntime> resolve) => new(resolve);
}
