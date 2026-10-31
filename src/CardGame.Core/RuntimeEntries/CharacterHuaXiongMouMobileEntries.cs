#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterHuaXiongMouMobileRuntime))]
internal sealed partial class CharacterHuaXiongMouMobileEntries
{
    private readonly Func<EngineOwners.CharacterHuaXiongMouMobileRuntime> _resolve;
    private CharacterHuaXiongMouMobileEntries(Func<EngineOwners.CharacterHuaXiongMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuaXiongMouMobileEntries Create(Func<EngineOwners.CharacterHuaXiongMouMobileRuntime> resolve) => new(resolve);
}
