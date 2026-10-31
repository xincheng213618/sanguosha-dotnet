#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterDiaoChanOlMagicRuntime))]
internal sealed partial class CharacterDiaoChanOlMagicEntries
{
    private readonly Func<EngineOwners.CharacterDiaoChanOlMagicRuntime> _resolve;
    private CharacterDiaoChanOlMagicEntries(Func<EngineOwners.CharacterDiaoChanOlMagicRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDiaoChanOlMagicEntries Create(Func<EngineOwners.CharacterDiaoChanOlMagicRuntime> resolve) => new(resolve);
}
