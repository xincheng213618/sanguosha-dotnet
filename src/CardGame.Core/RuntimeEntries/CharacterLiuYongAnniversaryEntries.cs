#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiuYongAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuYongAnniversaryRuntime> _resolve;
    private CharacterLiuYongAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuYongAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLiuYongAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuYongAnniversaryRuntime> resolve) => new(resolve);
}
