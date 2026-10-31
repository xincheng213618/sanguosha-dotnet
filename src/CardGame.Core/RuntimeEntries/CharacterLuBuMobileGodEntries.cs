#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterLuBuMobileGodRuntime))]
internal sealed partial class CharacterLuBuMobileGodEntries
{
    private readonly Func<EngineOwners.CharacterLuBuMobileGodRuntime> _resolve;
    private CharacterLuBuMobileGodEntries(Func<EngineOwners.CharacterLuBuMobileGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuBuMobileGodEntries Create(Func<EngineOwners.CharacterLuBuMobileGodRuntime> resolve) => new(resolve);
}
