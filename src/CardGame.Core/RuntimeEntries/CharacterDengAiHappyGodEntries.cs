#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDengAiHappyGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDengAiHappyGodRuntime> _resolve;
    private CharacterDengAiHappyGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDengAiHappyGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDengAiHappyGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDengAiHappyGodRuntime> resolve) => new(resolve);
}
