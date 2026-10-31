#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterDiaoChanOlMagicRuntime))]
internal sealed partial class CharacterDiaoChanOlMagicEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDiaoChanOlMagicRuntime> _resolve;
    private CharacterDiaoChanOlMagicEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDiaoChanOlMagicRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDiaoChanOlMagicEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDiaoChanOlMagicRuntime> resolve) => new(resolve);
}
