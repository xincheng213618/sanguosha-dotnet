#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterLuBuOlMagicRuntime))]
internal sealed partial class CharacterLuBuOlMagicEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuBuOlMagicRuntime> _resolve;
    private CharacterLuBuOlMagicEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuBuOlMagicRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuBuOlMagicEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuBuOlMagicRuntime> resolve) => new(resolve);
}
