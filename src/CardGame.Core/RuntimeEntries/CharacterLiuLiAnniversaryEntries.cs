#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiuLiAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuLiAnniversaryRuntime> _resolve;
    private CharacterLiuLiAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuLiAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLiuLiAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuLiAnniversaryRuntime> resolve) => new(resolve);
}
