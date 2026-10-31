#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoCaoOlGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoCaoOlGodRuntime> _resolve;
    private CharacterCaoCaoOlGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoCaoOlGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoCaoOlGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoCaoOlGodRuntime> resolve) => new(resolve);
}
