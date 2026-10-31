#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterRuanJiAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterRuanJiAnniversaryRuntime> _resolve;
    private CharacterRuanJiAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterRuanJiAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterRuanJiAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterRuanJiAnniversaryRuntime> resolve) => new(resolve);
}
