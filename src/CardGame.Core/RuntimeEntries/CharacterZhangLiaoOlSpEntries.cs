#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhangLiaoOlSpRuntime))]
internal sealed partial class CharacterZhangLiaoOlSpEntries
{
    private readonly Func<EngineOwners.CharacterZhangLiaoOlSpRuntime> _resolve;
    private CharacterZhangLiaoOlSpEntries(Func<EngineOwners.CharacterZhangLiaoOlSpRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangLiaoOlSpEntries Create(Func<EngineOwners.CharacterZhangLiaoOlSpRuntime> resolve) => new(resolve);
}
