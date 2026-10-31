#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterMaXiuMaTieOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaXiuMaTieOlRuntime> _resolve;
    private CharacterMaXiuMaTieOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaXiuMaTieOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterMaXiuMaTieOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaXiuMaTieOlRuntime> resolve) => new(resolve);
}
