#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterLuZhiMobileRuntime))]
internal sealed partial class CharacterLuZhiMobileEntries
{
    private readonly Func<EngineOwners.CharacterLuZhiMobileRuntime> _resolve;
    private CharacterLuZhiMobileEntries(Func<EngineOwners.CharacterLuZhiMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuZhiMobileEntries Create(Func<EngineOwners.CharacterLuZhiMobileRuntime> resolve) => new(resolve);
}
