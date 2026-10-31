#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterFengXiAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterFengXiAnniversaryRuntime> _resolve;
    private CharacterFengXiAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterFengXiAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterFengXiAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterFengXiAnniversaryRuntime> resolve) => new(resolve);
}
