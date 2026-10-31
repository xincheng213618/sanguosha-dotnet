#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterHuBanOlRuntime))]
internal sealed partial class CharacterHuBanOlEntries
{
    private readonly Func<EngineOwners.CharacterHuBanOlRuntime> _resolve;
    private CharacterHuBanOlEntries(Func<EngineOwners.CharacterHuBanOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuBanOlEntries Create(Func<EngineOwners.CharacterHuBanOlRuntime> resolve) => new(resolve);
}
