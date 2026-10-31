#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterSunShangxiangSpOlRuntime))]
internal sealed partial class CharacterSunShangxiangSpOlEntries
{
    private readonly Func<EngineOwners.CharacterSunShangxiangSpOlRuntime> _resolve;
    private CharacterSunShangxiangSpOlEntries(Func<EngineOwners.CharacterSunShangxiangSpOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSunShangxiangSpOlEntries Create(Func<EngineOwners.CharacterSunShangxiangSpOlRuntime> resolve) => new(resolve);
}
