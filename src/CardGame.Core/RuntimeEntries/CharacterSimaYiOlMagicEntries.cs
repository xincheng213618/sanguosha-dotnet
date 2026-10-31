#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterSimaYiOlMagicRuntime))]
internal sealed partial class CharacterSimaYiOlMagicEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaYiOlMagicRuntime> _resolve;
    private CharacterSimaYiOlMagicEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaYiOlMagicRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSimaYiOlMagicEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaYiOlMagicRuntime> resolve) => new(resolve);
}
