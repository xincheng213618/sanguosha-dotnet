#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterCaoPiBoundaryMobileRuntime))]
internal sealed partial class CharacterCaoPiBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterCaoPiBoundaryMobileRuntime> _resolve;
    private CharacterCaoPiBoundaryMobileEntries(Func<EngineOwners.CharacterCaoPiBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoPiBoundaryMobileEntries Create(Func<EngineOwners.CharacterCaoPiBoundaryMobileRuntime> resolve) => new(resolve);
}
