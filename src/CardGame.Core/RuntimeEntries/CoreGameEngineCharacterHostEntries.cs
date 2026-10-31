#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCharacterHostRuntime))]
internal sealed partial class CoreGameEngineCharacterHostEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCharacterHostRuntime> _resolve;
    private CoreGameEngineCharacterHostEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCharacterHostRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCharacterHostEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCharacterHostRuntime> resolve) => new(resolve);
}
