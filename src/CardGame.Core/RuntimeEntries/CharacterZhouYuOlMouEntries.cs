#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhouYuOlMouEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouYuOlMouRuntime> _resolve;
    private CharacterZhouYuOlMouEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouYuOlMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhouYuOlMouEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouYuOlMouRuntime> resolve) => new(resolve);
}
