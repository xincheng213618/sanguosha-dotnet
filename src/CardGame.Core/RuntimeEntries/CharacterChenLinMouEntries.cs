#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterChenLinMouEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterChenLinMouRuntime> _resolve;
    private CharacterChenLinMouEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChenLinMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterChenLinMouEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChenLinMouRuntime> resolve) => new(resolve);
}
