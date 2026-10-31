#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXuJingAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXuJingAnniversaryRuntime> _resolve;
    private CharacterXuJingAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXuJingAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXuJingAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXuJingAnniversaryRuntime> resolve) => new(resolve);
}
