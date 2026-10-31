#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterGongsunZanMouMobileRuntime))]
internal sealed partial class CharacterGongsunZanMouMobileEntries
{
    private readonly Func<EngineOwners.CharacterGongsunZanMouMobileRuntime> _resolve;
    private CharacterGongsunZanMouMobileEntries(Func<EngineOwners.CharacterGongsunZanMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGongsunZanMouMobileEntries Create(Func<EngineOwners.CharacterGongsunZanMouMobileRuntime> resolve) => new(resolve);
}
