#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhouFeiBoundaryMobileRuntime))]
internal sealed partial class CharacterZhouFeiBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterZhouFeiBoundaryMobileRuntime> _resolve;
    private CharacterZhouFeiBoundaryMobileEntries(Func<EngineOwners.CharacterZhouFeiBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhouFeiBoundaryMobileEntries Create(Func<EngineOwners.CharacterZhouFeiBoundaryMobileRuntime> resolve) => new(resolve);
}
