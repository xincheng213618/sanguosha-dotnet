#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterLvKuangLvXiangOlRuntime))]
internal sealed partial class CharacterLvKuangLvXiangOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLvKuangLvXiangOlRuntime> _resolve;
    private CharacterLvKuangLvXiangOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLvKuangLvXiangOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLvKuangLvXiangOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLvKuangLvXiangOlRuntime> resolve) => new(resolve);
}
