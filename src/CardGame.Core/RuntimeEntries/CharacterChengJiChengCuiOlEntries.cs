#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterChengJiChengCuiOlRuntime))]
internal sealed partial class CharacterChengJiChengCuiOlEntries
{
    private readonly Func<EngineOwners.CharacterChengJiChengCuiOlRuntime> _resolve;
    private CharacterChengJiChengCuiOlEntries(Func<EngineOwners.CharacterChengJiChengCuiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterChengJiChengCuiOlEntries Create(Func<EngineOwners.CharacterChengJiChengCuiOlRuntime> resolve) => new(resolve);
}
