#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuaTuoMobileGodEntries
{
    private readonly Func<EngineOwners.CharacterHuaTuoMobileGodRuntime> _resolve;
    private CharacterHuaTuoMobileGodEntries(Func<EngineOwners.CharacterHuaTuoMobileGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuaTuoMobileGodEntries Create(Func<EngineOwners.CharacterHuaTuoMobileGodRuntime> resolve) => new(resolve);
}
