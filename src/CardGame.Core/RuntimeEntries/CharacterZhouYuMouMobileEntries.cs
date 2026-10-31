#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhouYuMouMobileRuntime))]
internal sealed partial class CharacterZhouYuMouMobileEntries
{
    private readonly Func<EngineOwners.CharacterZhouYuMouMobileRuntime> _resolve;
    private CharacterZhouYuMouMobileEntries(Func<EngineOwners.CharacterZhouYuMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhouYuMouMobileEntries Create(Func<EngineOwners.CharacterZhouYuMouMobileRuntime> resolve) => new(resolve);
}
