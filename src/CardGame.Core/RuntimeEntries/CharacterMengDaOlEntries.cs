#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterMengDaOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMengDaOlRuntime> _resolve;
    private CharacterMengDaOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMengDaOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterMengDaOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMengDaOlRuntime> resolve) => new(resolve);
}
