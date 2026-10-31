#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterXuanGongZhuOlRuntime))]
internal sealed partial class CharacterXuanGongZhuOlEntries
{
    private readonly Func<EngineOwners.CharacterXuanGongZhuOlRuntime> _resolve;
    private CharacterXuanGongZhuOlEntries(Func<EngineOwners.CharacterXuanGongZhuOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXuanGongZhuOlEntries Create(Func<EngineOwners.CharacterXuanGongZhuOlRuntime> resolve) => new(resolve);
}
