#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXuChuAnniversaryGodEntries
{
    private readonly Func<EngineOwners.CharacterXuChuAnniversaryGodRuntime> _resolve;
    private CharacterXuChuAnniversaryGodEntries(Func<EngineOwners.CharacterXuChuAnniversaryGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXuChuAnniversaryGodEntries Create(Func<EngineOwners.CharacterXuChuAnniversaryGodRuntime> resolve) => new(resolve);
}
