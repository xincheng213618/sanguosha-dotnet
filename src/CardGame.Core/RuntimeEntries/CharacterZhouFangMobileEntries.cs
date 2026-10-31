#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhouFangMobileRuntime))]
internal sealed partial class CharacterZhouFangMobileEntries
{
    private readonly Func<EngineOwners.CharacterZhouFangMobileRuntime> _resolve;
    private CharacterZhouFangMobileEntries(Func<EngineOwners.CharacterZhouFangMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhouFangMobileEntries Create(Func<EngineOwners.CharacterZhouFangMobileRuntime> resolve) => new(resolve);
}
