#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoCaoHappyGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoCaoHappyGodRuntime> _resolve;
    private CharacterCaoCaoHappyGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoCaoHappyGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoCaoHappyGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoCaoHappyGodRuntime> resolve) => new(resolve);
}
