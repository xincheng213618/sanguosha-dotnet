#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterTaishiCiMobileEntries
{
    private readonly Func<EngineOwners.CharacterTaishiCiMobileRuntime> _resolve;
    private CharacterTaishiCiMobileEntries(Func<EngineOwners.CharacterTaishiCiMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterTaishiCiMobileEntries Create(Func<EngineOwners.CharacterTaishiCiMobileRuntime> resolve) => new(resolve);
}
