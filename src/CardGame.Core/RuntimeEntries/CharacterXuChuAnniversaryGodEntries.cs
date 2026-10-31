#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXuChuAnniversaryGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXuChuAnniversaryGodRuntime> _resolve;
    private CharacterXuChuAnniversaryGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXuChuAnniversaryGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXuChuAnniversaryGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXuChuAnniversaryGodRuntime> resolve) => new(resolve);
}
