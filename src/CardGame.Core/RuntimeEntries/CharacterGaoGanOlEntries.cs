#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGaoGanOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGaoGanOlRuntime> _resolve;
    private CharacterGaoGanOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGaoGanOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGaoGanOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGaoGanOlRuntime> resolve) => new(resolve);
}
