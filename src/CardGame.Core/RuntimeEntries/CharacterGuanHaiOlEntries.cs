#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGuanHaiOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanHaiOlRuntime> _resolve;
    private CharacterGuanHaiOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanHaiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGuanHaiOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanHaiOlRuntime> resolve) => new(resolve);
}
