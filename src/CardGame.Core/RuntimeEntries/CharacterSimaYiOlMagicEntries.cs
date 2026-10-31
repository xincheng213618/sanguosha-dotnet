#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterSimaYiOlMagicRuntime))]
internal sealed partial class CharacterSimaYiOlMagicEntries
{
    private readonly Func<EngineOwners.CharacterSimaYiOlMagicRuntime> _resolve;
    private CharacterSimaYiOlMagicEntries(Func<EngineOwners.CharacterSimaYiOlMagicRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSimaYiOlMagicEntries Create(Func<EngineOwners.CharacterSimaYiOlMagicRuntime> resolve) => new(resolve);
}
