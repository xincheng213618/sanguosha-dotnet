#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterLuMengMouMobileRuntime))]
internal sealed partial class CharacterLuMengMouMobileEntries
{
    private readonly Func<EngineOwners.CharacterLuMengMouMobileRuntime> _resolve;
    private CharacterLuMengMouMobileEntries(Func<EngineOwners.CharacterLuMengMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuMengMouMobileEntries Create(Func<EngineOwners.CharacterLuMengMouMobileRuntime> resolve) => new(resolve);
}
