#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterPangTongMouMobileRuntime))]
internal sealed partial class CharacterPangTongMouMobileEntries
{
    private readonly Func<EngineOwners.CharacterPangTongMouMobileRuntime> _resolve;
    private CharacterPangTongMouMobileEntries(Func<EngineOwners.CharacterPangTongMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterPangTongMouMobileEntries Create(Func<EngineOwners.CharacterPangTongMouMobileRuntime> resolve) => new(resolve);
}
