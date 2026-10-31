#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhenJiMouMobileRuntime))]
internal sealed partial class CharacterZhenJiMouMobileEntries
{
    private readonly Func<EngineOwners.CharacterZhenJiMouMobileRuntime> _resolve;
    private CharacterZhenJiMouMobileEntries(Func<EngineOwners.CharacterZhenJiMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhenJiMouMobileEntries Create(Func<EngineOwners.CharacterZhenJiMouMobileRuntime> resolve) => new(resolve);
}
