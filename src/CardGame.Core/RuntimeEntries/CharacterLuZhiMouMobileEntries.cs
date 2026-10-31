#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterLuZhiMouMobileRuntime))]
internal sealed partial class CharacterLuZhiMouMobileEntries
{
    private readonly Func<EngineOwners.CharacterLuZhiMouMobileRuntime> _resolve;
    private CharacterLuZhiMouMobileEntries(Func<EngineOwners.CharacterLuZhiMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuZhiMouMobileEntries Create(Func<EngineOwners.CharacterLuZhiMouMobileRuntime> resolve) => new(resolve);
}
