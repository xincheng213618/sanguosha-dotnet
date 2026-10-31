#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhaoXiangOlRuntime))]
internal sealed partial class CharacterZhaoXiangOlEntries
{
    private readonly Func<EngineOwners.CharacterZhaoXiangOlRuntime> _resolve;
    private CharacterZhaoXiangOlEntries(Func<EngineOwners.CharacterZhaoXiangOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhaoXiangOlEntries Create(Func<EngineOwners.CharacterZhaoXiangOlRuntime> resolve) => new(resolve);
}
