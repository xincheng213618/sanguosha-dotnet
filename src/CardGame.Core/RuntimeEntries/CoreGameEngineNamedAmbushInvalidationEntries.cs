#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineNamedAmbushInvalidationRuntime))]
internal sealed partial class CoreGameEngineNamedAmbushInvalidationEntries
{
    private readonly Func<EngineOwners.CoreGameEngineNamedAmbushInvalidationRuntime> _resolve;
    private CoreGameEngineNamedAmbushInvalidationEntries(Func<EngineOwners.CoreGameEngineNamedAmbushInvalidationRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNamedAmbushInvalidationEntries Create(Func<EngineOwners.CoreGameEngineNamedAmbushInvalidationRuntime> resolve) => new(resolve);
}
