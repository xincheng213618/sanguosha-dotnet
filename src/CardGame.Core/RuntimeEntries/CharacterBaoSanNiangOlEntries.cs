#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterBaoSanNiangOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterBaoSanNiangOlRuntime> _resolve;
    private CharacterBaoSanNiangOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterBaoSanNiangOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterBaoSanNiangOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterBaoSanNiangOlRuntime> resolve) => new(resolve);
}
