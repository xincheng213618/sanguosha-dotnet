#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterMaChengOlRuntime))]
internal sealed partial class CharacterMaChengOlEntries
{
    private readonly Func<EngineOwners.CharacterMaChengOlRuntime> _resolve;
    private CharacterMaChengOlEntries(Func<EngineOwners.CharacterMaChengOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterMaChengOlEntries Create(Func<EngineOwners.CharacterMaChengOlRuntime> resolve) => new(resolve);
}
