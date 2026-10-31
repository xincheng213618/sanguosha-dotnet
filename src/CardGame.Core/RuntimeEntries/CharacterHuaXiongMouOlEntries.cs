#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterHuaXiongMouOlRuntime))]
internal sealed partial class CharacterHuaXiongMouOlEntries
{
    private readonly Func<EngineOwners.CharacterHuaXiongMouOlRuntime> _resolve;
    private CharacterHuaXiongMouOlEntries(Func<EngineOwners.CharacterHuaXiongMouOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuaXiongMouOlEntries Create(Func<EngineOwners.CharacterHuaXiongMouOlRuntime> resolve) => new(resolve);
}
