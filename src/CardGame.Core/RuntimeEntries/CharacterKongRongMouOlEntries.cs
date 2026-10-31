#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterKongRongMouOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterKongRongMouOlRuntime> _resolve;
    private CharacterKongRongMouOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterKongRongMouOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterKongRongMouOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterKongRongMouOlRuntime> resolve) => new(resolve);
}
