#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDiaoChanLeEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDiaoChanLeRuntime> _resolve;
    private CharacterDiaoChanLeEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDiaoChanLeRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDiaoChanLeEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDiaoChanLeRuntime> resolve) => new(resolve);
}
