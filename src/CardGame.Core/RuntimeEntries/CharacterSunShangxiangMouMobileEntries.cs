#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterSunShangxiangMouMobileRuntime))]
internal sealed partial class CharacterSunShangxiangMouMobileEntries
{
    private readonly Func<EngineOwners.CharacterSunShangxiangMouMobileRuntime> _resolve;
    private CharacterSunShangxiangMouMobileEntries(Func<EngineOwners.CharacterSunShangxiangMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSunShangxiangMouMobileEntries Create(Func<EngineOwners.CharacterSunShangxiangMouMobileRuntime> resolve) => new(resolve);
}
