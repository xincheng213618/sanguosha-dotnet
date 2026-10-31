#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterZhangLiaoOlSpRuntime))]
internal sealed partial class CharacterZhangLiaoOlSpEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangLiaoOlSpRuntime> _resolve;
    private CharacterZhangLiaoOlSpEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangLiaoOlSpRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangLiaoOlSpEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangLiaoOlSpRuntime> resolve) => new(resolve);
}
