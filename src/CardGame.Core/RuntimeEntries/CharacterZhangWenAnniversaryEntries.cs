#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangWenAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangWenAnniversaryRuntime> _resolve;
    private CharacterZhangWenAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangWenAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangWenAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangWenAnniversaryRuntime> resolve) => new(resolve);
}
