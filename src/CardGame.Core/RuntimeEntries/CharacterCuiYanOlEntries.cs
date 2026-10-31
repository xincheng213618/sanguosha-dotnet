#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterCuiYanOlRuntime))]
internal sealed partial class CharacterCuiYanOlEntries
{
    private readonly Func<EngineOwners.CharacterCuiYanOlRuntime> _resolve;
    private CharacterCuiYanOlEntries(Func<EngineOwners.CharacterCuiYanOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCuiYanOlEntries Create(Func<EngineOwners.CharacterCuiYanOlRuntime> resolve) => new(resolve);
}
