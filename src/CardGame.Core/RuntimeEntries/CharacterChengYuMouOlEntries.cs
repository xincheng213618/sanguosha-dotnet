#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterChengYuMouOlRuntime))]
internal sealed partial class CharacterChengYuMouOlEntries
{
    private readonly Func<EngineOwners.CharacterChengYuMouOlRuntime> _resolve;
    private CharacterChengYuMouOlEntries(Func<EngineOwners.CharacterChengYuMouOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterChengYuMouOlEntries Create(Func<EngineOwners.CharacterChengYuMouOlRuntime> resolve) => new(resolve);
}
