#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterWuGuoTaiBoundaryMobileRuntime))]
internal sealed partial class CharacterWuGuoTaiBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterWuGuoTaiBoundaryMobileRuntime> _resolve;
    private CharacterWuGuoTaiBoundaryMobileEntries(Func<EngineOwners.CharacterWuGuoTaiBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWuGuoTaiBoundaryMobileEntries Create(Func<EngineOwners.CharacterWuGuoTaiBoundaryMobileRuntime> resolve) => new(resolve);
}
