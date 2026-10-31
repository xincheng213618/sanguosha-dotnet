#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterSunQuanOlMagicRuntime))]
internal sealed partial class CharacterSunQuanOlMagicEntries
{
    private readonly Func<EngineOwners.CharacterSunQuanOlMagicRuntime> _resolve;
    private CharacterSunQuanOlMagicEntries(Func<EngineOwners.CharacterSunQuanOlMagicRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSunQuanOlMagicEntries Create(Func<EngineOwners.CharacterSunQuanOlMagicRuntime> resolve) => new(resolve);
}
