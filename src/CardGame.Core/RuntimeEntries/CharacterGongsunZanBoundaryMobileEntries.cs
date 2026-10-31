#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterGongsunZanBoundaryMobileRuntime))]
internal sealed partial class CharacterGongsunZanBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterGongsunZanBoundaryMobileRuntime> _resolve;
    private CharacterGongsunZanBoundaryMobileEntries(Func<EngineOwners.CharacterGongsunZanBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGongsunZanBoundaryMobileEntries Create(Func<EngineOwners.CharacterGongsunZanBoundaryMobileRuntime> resolve) => new(resolve);
}
