#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDiaoChanOlMagicEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDiaoChanOlMagicRuntime> _resolve;
    private CharacterDiaoChanOlMagicEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDiaoChanOlMagicRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDiaoChanOlMagicEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDiaoChanOlMagicRuntime> resolve) => new(resolve);
}
