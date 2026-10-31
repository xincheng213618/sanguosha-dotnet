#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterSunHaoMobileRuntime))]
internal sealed partial class CharacterSunHaoMobileEntries
{
    private readonly Func<EngineOwners.CharacterSunHaoMobileRuntime> _resolve;
    private CharacterSunHaoMobileEntries(Func<EngineOwners.CharacterSunHaoMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSunHaoMobileEntries Create(Func<EngineOwners.CharacterSunHaoMobileRuntime> resolve) => new(resolve);
}
