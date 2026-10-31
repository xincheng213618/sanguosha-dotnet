#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhouYuMobileRuntime))]
internal sealed partial class CharacterZhouYuMobileEntries
{
    private readonly Func<EngineOwners.CharacterZhouYuMobileRuntime> _resolve;
    private CharacterZhouYuMobileEntries(Func<EngineOwners.CharacterZhouYuMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhouYuMobileEntries Create(Func<EngineOwners.CharacterZhouYuMobileRuntime> resolve) => new(resolve);
}
