#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGuanYuAnniversaryGodEntries
{
    private readonly Func<EngineOwners.CharacterGuanYuAnniversaryGodRuntime> _resolve;
    private CharacterGuanYuAnniversaryGodEntries(Func<EngineOwners.CharacterGuanYuAnniversaryGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGuanYuAnniversaryGodEntries Create(Func<EngineOwners.CharacterGuanYuAnniversaryGodRuntime> resolve) => new(resolve);
}
