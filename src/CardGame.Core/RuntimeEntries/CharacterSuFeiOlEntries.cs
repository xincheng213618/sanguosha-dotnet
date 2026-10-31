#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSuFeiOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSuFeiOlRuntime> _resolve;
    private CharacterSuFeiOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSuFeiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSuFeiOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSuFeiOlRuntime> resolve) => new(resolve);
}
