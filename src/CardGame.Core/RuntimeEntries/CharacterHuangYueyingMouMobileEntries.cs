#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterHuangYueyingMouMobileRuntime))]
internal sealed partial class CharacterHuangYueyingMouMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangYueyingMouMobileRuntime> _resolve;
    private CharacterHuangYueyingMouMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangYueyingMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuangYueyingMouMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangYueyingMouMobileRuntime> resolve) => new(resolve);
}
