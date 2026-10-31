#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterTaishiCiMobileGodRuntime))]
internal sealed partial class CharacterTaishiCiMobileGodEntries
{
    private readonly Func<EngineOwners.CharacterTaishiCiMobileGodRuntime> _resolve;
    private CharacterTaishiCiMobileGodEntries(Func<EngineOwners.CharacterTaishiCiMobileGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterTaishiCiMobileGodEntries Create(Func<EngineOwners.CharacterTaishiCiMobileGodRuntime> resolve) => new(resolve);
}
