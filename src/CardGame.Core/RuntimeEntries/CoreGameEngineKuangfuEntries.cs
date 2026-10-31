#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineKuangfuRuntime))]
internal sealed partial class CoreGameEngineKuangfuEntries
{
    private readonly Func<EngineOwners.CoreGameEngineKuangfuRuntime> _resolve;
    private CoreGameEngineKuangfuEntries(Func<EngineOwners.CoreGameEngineKuangfuRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineKuangfuEntries Create(Func<EngineOwners.CoreGameEngineKuangfuRuntime> resolve) => new(resolve);
}
