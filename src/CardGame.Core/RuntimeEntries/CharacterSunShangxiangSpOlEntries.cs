#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterSunShangxiangSpOlRuntime))]
internal sealed partial class CharacterSunShangxiangSpOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunShangxiangSpOlRuntime> _resolve;
    private CharacterSunShangxiangSpOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunShangxiangSpOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSunShangxiangSpOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunShangxiangSpOlRuntime> resolve) => new(resolve);
}
