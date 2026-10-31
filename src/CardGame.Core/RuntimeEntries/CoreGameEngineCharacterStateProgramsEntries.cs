#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCharacterStateProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCharacterStateProgramsRuntime> _resolve;
    private CoreGameEngineCharacterStateProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCharacterStateProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCharacterStateProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCharacterStateProgramsRuntime> resolve) => new(resolve);
}
