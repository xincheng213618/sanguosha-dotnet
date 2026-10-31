#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineModePresentationRuntime))]
internal sealed partial class CoreGameEngineModePresentationEntries
{
    private readonly Func<EngineOwners.CoreGameEngineModePresentationRuntime> _resolve;
    private CoreGameEngineModePresentationEntries(Func<EngineOwners.CoreGameEngineModePresentationRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineModePresentationEntries Create(Func<EngineOwners.CoreGameEngineModePresentationRuntime> resolve) => new(resolve);
}
