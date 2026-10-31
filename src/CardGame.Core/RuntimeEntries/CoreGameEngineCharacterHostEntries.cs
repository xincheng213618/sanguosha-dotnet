#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineCharacterHostRuntime))]
internal sealed partial class CoreGameEngineCharacterHostEntries
{
    private readonly Func<EngineOwners.CoreGameEngineCharacterHostRuntime> _resolve;
    private CoreGameEngineCharacterHostEntries(Func<EngineOwners.CoreGameEngineCharacterHostRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCharacterHostEntries Create(Func<EngineOwners.CoreGameEngineCharacterHostRuntime> resolve) => new(resolve);
}
