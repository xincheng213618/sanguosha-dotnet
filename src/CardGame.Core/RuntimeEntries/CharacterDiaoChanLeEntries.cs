#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterDiaoChanLeRuntime))]
internal sealed partial class CharacterDiaoChanLeEntries
{
    private readonly Func<EngineOwners.CharacterDiaoChanLeRuntime> _resolve;
    private CharacterDiaoChanLeEntries(Func<EngineOwners.CharacterDiaoChanLeRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDiaoChanLeEntries Create(Func<EngineOwners.CharacterDiaoChanLeRuntime> resolve) => new(resolve);
}
