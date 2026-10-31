#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterHuJinDingOlRuntime))]
internal sealed partial class CharacterHuJinDingOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuJinDingOlRuntime> _resolve;
    private CharacterHuJinDingOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuJinDingOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuJinDingOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuJinDingOlRuntime> resolve) => new(resolve);
}
