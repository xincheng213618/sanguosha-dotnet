#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXuChuHappyGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXuChuHappyGodRuntime> _resolve;
    private CharacterXuChuHappyGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXuChuHappyGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXuChuHappyGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXuChuHappyGodRuntime> resolve) => new(resolve);
}
