#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterTianYuMouMobileRuntime))]
internal sealed partial class CharacterTianYuMouMobileEntries
{
    private readonly Func<EngineOwners.CharacterTianYuMouMobileRuntime> _resolve;
    private CharacterTianYuMouMobileEntries(Func<EngineOwners.CharacterTianYuMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterTianYuMouMobileEntries Create(Func<EngineOwners.CharacterTianYuMouMobileRuntime> resolve) => new(resolve);
}
