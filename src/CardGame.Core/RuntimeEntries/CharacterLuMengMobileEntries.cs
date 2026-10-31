#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterLuMengMobileRuntime))]
internal sealed partial class CharacterLuMengMobileEntries
{
    private readonly Func<EngineOwners.CharacterLuMengMobileRuntime> _resolve;
    private CharacterLuMengMobileEntries(Func<EngineOwners.CharacterLuMengMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuMengMobileEntries Create(Func<EngineOwners.CharacterLuMengMobileRuntime> resolve) => new(resolve);
}
