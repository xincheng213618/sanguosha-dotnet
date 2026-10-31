#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterBuLianShiBoundaryMobileRuntime))]
internal sealed partial class CharacterBuLianShiBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterBuLianShiBoundaryMobileRuntime> _resolve;
    private CharacterBuLianShiBoundaryMobileEntries(Func<EngineOwners.CharacterBuLianShiBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterBuLianShiBoundaryMobileEntries Create(Func<EngineOwners.CharacterBuLianShiBoundaryMobileRuntime> resolve) => new(resolve);
}
