#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterDiaoChanHappyGodRuntime))]
internal sealed partial class CharacterDiaoChanHappyGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDiaoChanHappyGodRuntime> _resolve;
    private CharacterDiaoChanHappyGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDiaoChanHappyGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDiaoChanHappyGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDiaoChanHappyGodRuntime> resolve) => new(resolve);
}
