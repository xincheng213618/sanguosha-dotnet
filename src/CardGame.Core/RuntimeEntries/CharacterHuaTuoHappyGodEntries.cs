#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterHuaTuoHappyGodRuntime))]
internal sealed partial class CharacterHuaTuoHappyGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaTuoHappyGodRuntime> _resolve;
    private CharacterHuaTuoHappyGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaTuoHappyGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuaTuoHappyGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaTuoHappyGodRuntime> resolve) => new(resolve);
}
