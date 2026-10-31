#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterSunYuanOlRuntime))]
internal sealed partial class CharacterSunYuanOlEntries
{
    private readonly Func<EngineOwners.CharacterSunYuanOlRuntime> _resolve;
    private CharacterSunYuanOlEntries(Func<EngineOwners.CharacterSunYuanOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSunYuanOlEntries Create(Func<EngineOwners.CharacterSunYuanOlRuntime> resolve) => new(resolve);
}
