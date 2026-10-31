#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterQianZhaoOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterQianZhaoOlRuntime> _resolve;
    private CharacterQianZhaoOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterQianZhaoOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterQianZhaoOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterQianZhaoOlRuntime> resolve) => new(resolve);
}
