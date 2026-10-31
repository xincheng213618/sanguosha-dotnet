#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterPangTongFriendRuntime))]
internal sealed partial class CharacterPangTongFriendEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangTongFriendRuntime> _resolve;
    private CharacterPangTongFriendEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangTongFriendRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterPangTongFriendEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangTongFriendRuntime> resolve) => new(resolve);
}
