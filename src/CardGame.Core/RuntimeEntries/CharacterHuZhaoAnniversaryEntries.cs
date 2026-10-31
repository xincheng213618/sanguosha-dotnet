#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuZhaoAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuZhaoAnniversaryRuntime> _resolve;
    private CharacterHuZhaoAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuZhaoAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuZhaoAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuZhaoAnniversaryRuntime> resolve) => new(resolve);
}
