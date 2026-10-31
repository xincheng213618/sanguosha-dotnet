#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhangFeiBoundaryMobileRuntime))]
internal sealed partial class CharacterZhangFeiBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterZhangFeiBoundaryMobileRuntime> _resolve;
    private CharacterZhangFeiBoundaryMobileEntries(Func<EngineOwners.CharacterZhangFeiBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangFeiBoundaryMobileEntries Create(Func<EngineOwners.CharacterZhangFeiBoundaryMobileRuntime> resolve) => new(resolve);
}
