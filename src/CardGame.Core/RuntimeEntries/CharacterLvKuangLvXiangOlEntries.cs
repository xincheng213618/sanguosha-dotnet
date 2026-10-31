#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterLvKuangLvXiangOlRuntime))]
internal sealed partial class CharacterLvKuangLvXiangOlEntries
{
    private readonly Func<EngineOwners.CharacterLvKuangLvXiangOlRuntime> _resolve;
    private CharacterLvKuangLvXiangOlEntries(Func<EngineOwners.CharacterLvKuangLvXiangOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLvKuangLvXiangOlEntries Create(Func<EngineOwners.CharacterLvKuangLvXiangOlRuntime> resolve) => new(resolve);
}
