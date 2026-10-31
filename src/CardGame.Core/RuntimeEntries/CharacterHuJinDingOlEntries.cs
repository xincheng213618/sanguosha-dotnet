#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterHuJinDingOlRuntime))]
internal sealed partial class CharacterHuJinDingOlEntries
{
    private readonly Func<EngineOwners.CharacterHuJinDingOlRuntime> _resolve;
    private CharacterHuJinDingOlEntries(Func<EngineOwners.CharacterHuJinDingOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuJinDingOlEntries Create(Func<EngineOwners.CharacterHuJinDingOlRuntime> resolve) => new(resolve);
}
