#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterWangJiBoundaryMobileRuntime))]
internal sealed partial class CharacterWangJiBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterWangJiBoundaryMobileRuntime> _resolve;
    private CharacterWangJiBoundaryMobileEntries(Func<EngineOwners.CharacterWangJiBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWangJiBoundaryMobileEntries Create(Func<EngineOwners.CharacterWangJiBoundaryMobileRuntime> resolve) => new(resolve);
}
