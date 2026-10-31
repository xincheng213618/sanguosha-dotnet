#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterLuBuOlMagicRuntime))]
internal sealed partial class CharacterLuBuOlMagicEntries
{
    private readonly Func<EngineOwners.CharacterLuBuOlMagicRuntime> _resolve;
    private CharacterLuBuOlMagicEntries(Func<EngineOwners.CharacterLuBuOlMagicRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuBuOlMagicEntries Create(Func<EngineOwners.CharacterLuBuOlMagicRuntime> resolve) => new(resolve);
}
