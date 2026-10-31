#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterZhangXiuMouOlRuntime))]
internal sealed partial class CharacterZhangXiuMouOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangXiuMouOlRuntime> _resolve;
    private CharacterZhangXiuMouOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangXiuMouOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangXiuMouOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangXiuMouOlRuntime> resolve) => new(resolve);
}
