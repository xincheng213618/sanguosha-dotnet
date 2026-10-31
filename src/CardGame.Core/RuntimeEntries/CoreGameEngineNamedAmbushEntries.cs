#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineNamedAmbushRuntime))]
internal sealed partial class CoreGameEngineNamedAmbushEntries
{
    private readonly Func<EngineOwners.CoreGameEngineNamedAmbushRuntime> _resolve;
    private CoreGameEngineNamedAmbushEntries(Func<EngineOwners.CoreGameEngineNamedAmbushRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNamedAmbushEntries Create(Func<EngineOwners.CoreGameEngineNamedAmbushRuntime> resolve) => new(resolve);
}
