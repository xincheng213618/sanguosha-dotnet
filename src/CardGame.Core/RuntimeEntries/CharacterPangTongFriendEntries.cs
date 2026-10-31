#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterPangTongFriendRuntime))]
internal sealed partial class CharacterPangTongFriendEntries
{
    private readonly Func<EngineOwners.CharacterPangTongFriendRuntime> _resolve;
    private CharacterPangTongFriendEntries(Func<EngineOwners.CharacterPangTongFriendRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterPangTongFriendEntries Create(Func<EngineOwners.CharacterPangTongFriendRuntime> resolve) => new(resolve);
}
