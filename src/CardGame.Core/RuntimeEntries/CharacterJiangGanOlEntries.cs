#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterJiangGanOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterJiangGanOlRuntime> _resolve;
    private CharacterJiangGanOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterJiangGanOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterJiangGanOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterJiangGanOlRuntime> resolve) => new(resolve);
}
