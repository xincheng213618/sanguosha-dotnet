#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterZhangQiYingOlRuntime))]
internal sealed partial class CharacterZhangQiYingOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangQiYingOlRuntime> _resolve;
    private CharacterZhangQiYingOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangQiYingOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangQiYingOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangQiYingOlRuntime> resolve) => new(resolve);
}
