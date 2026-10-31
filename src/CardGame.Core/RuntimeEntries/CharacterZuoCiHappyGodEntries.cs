#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterZuoCiHappyGodRuntime))]
internal sealed partial class CharacterZuoCiHappyGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZuoCiHappyGodRuntime> _resolve;
    private CharacterZuoCiHappyGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZuoCiHappyGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZuoCiHappyGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZuoCiHappyGodRuntime> resolve) => new(resolve);
}
