#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterSunQuanMobileRuntime))]
internal sealed partial class CharacterSunQuanMobileEntries
{
    private readonly Func<EngineOwners.CharacterSunQuanMobileRuntime> _resolve;
    private CharacterSunQuanMobileEntries(Func<EngineOwners.CharacterSunQuanMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSunQuanMobileEntries Create(Func<EngineOwners.CharacterSunQuanMobileRuntime> resolve) => new(resolve);
}
