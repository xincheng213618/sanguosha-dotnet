#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhangNiBoundaryMobileRuntime))]
internal sealed partial class CharacterZhangNiBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterZhangNiBoundaryMobileRuntime> _resolve;
    private CharacterZhangNiBoundaryMobileEntries(Func<EngineOwners.CharacterZhangNiBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangNiBoundaryMobileEntries Create(Func<EngineOwners.CharacterZhangNiBoundaryMobileRuntime> resolve) => new(resolve);
}
