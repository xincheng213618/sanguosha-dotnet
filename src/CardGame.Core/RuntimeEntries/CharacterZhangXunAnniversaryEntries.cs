#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangXunAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangXunAnniversaryRuntime> _resolve;
    private CharacterZhangXunAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangXunAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangXunAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangXunAnniversaryRuntime> resolve) => new(resolve);
}
