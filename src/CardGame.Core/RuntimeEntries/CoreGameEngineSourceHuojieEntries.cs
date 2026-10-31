#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineSourceHuojieRuntime))]
internal sealed partial class CoreGameEngineSourceHuojieEntries
{
    private readonly Func<EngineOwners.CoreGameEngineSourceHuojieRuntime> _resolve;
    private CoreGameEngineSourceHuojieEntries(Func<EngineOwners.CoreGameEngineSourceHuojieRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSourceHuojieEntries Create(Func<EngineOwners.CoreGameEngineSourceHuojieRuntime> resolve) => new(resolve);
}
