#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterHuaManOlRuntime))]
internal sealed partial class CharacterHuaManOlEntries
{
    private readonly Func<EngineOwners.CharacterHuaManOlRuntime> _resolve;
    private CharacterHuaManOlEntries(Func<EngineOwners.CharacterHuaManOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuaManOlEntries Create(Func<EngineOwners.CharacterHuaManOlRuntime> resolve) => new(resolve);
}
