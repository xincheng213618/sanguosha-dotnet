#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterHuanShuJunOlRuntime))]
internal sealed partial class CharacterHuanShuJunOlEntries
{
    private readonly Func<EngineOwners.CharacterHuanShuJunOlRuntime> _resolve;
    private CharacterHuanShuJunOlEntries(Func<EngineOwners.CharacterHuanShuJunOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuanShuJunOlEntries Create(Func<EngineOwners.CharacterHuanShuJunOlRuntime> resolve) => new(resolve);
}
