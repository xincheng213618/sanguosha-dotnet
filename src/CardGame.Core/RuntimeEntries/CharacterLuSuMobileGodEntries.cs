#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterLuSuMobileGodRuntime))]
internal sealed partial class CharacterLuSuMobileGodEntries
{
    private readonly Func<EngineOwners.CharacterLuSuMobileGodRuntime> _resolve;
    private CharacterLuSuMobileGodEntries(Func<EngineOwners.CharacterLuSuMobileGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuSuMobileGodEntries Create(Func<EngineOwners.CharacterLuSuMobileGodRuntime> resolve) => new(resolve);
}
