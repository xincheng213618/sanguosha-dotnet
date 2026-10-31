#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuangYueyingOlMouEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangYueyingOlMouRuntime> _resolve;
    private CharacterHuangYueyingOlMouEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangYueyingOlMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuangYueyingOlMouEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangYueyingOlMouRuntime> resolve) => new(resolve);
}
