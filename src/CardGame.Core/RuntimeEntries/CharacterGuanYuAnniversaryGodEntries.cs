#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGuanYuAnniversaryGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuAnniversaryGodRuntime> _resolve;
    private CharacterGuanYuAnniversaryGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuAnniversaryGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGuanYuAnniversaryGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuAnniversaryGodRuntime> resolve) => new(resolve);
}
