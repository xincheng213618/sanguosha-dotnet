#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterGongsunZanBoundaryMobileRuntime))]
internal sealed partial class CharacterGongsunZanBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGongsunZanBoundaryMobileRuntime> _resolve;
    private CharacterGongsunZanBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGongsunZanBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGongsunZanBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGongsunZanBoundaryMobileRuntime> resolve) => new(resolve);
}
