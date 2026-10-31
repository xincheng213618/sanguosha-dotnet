#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterChenQunBoundaryMobileRuntime))]
internal sealed partial class CharacterChenQunBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterChenQunBoundaryMobileRuntime> _resolve;
    private CharacterChenQunBoundaryMobileEntries(Func<EngineOwners.CharacterChenQunBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterChenQunBoundaryMobileEntries Create(Func<EngineOwners.CharacterChenQunBoundaryMobileRuntime> resolve) => new(resolve);
}
