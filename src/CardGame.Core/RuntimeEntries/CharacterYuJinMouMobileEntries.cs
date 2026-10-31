#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterYuJinMouMobileRuntime))]
internal sealed partial class CharacterYuJinMouMobileEntries
{
    private readonly Func<EngineOwners.CharacterYuJinMouMobileRuntime> _resolve;
    private CharacterYuJinMouMobileEntries(Func<EngineOwners.CharacterYuJinMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterYuJinMouMobileEntries Create(Func<EngineOwners.CharacterYuJinMouMobileRuntime> resolve) => new(resolve);
}
