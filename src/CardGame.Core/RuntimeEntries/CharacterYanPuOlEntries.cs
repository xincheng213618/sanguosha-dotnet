#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterYanPuOlRuntime))]
internal sealed partial class CharacterYanPuOlEntries
{
    private readonly Func<EngineOwners.CharacterYanPuOlRuntime> _resolve;
    private CharacterYanPuOlEntries(Func<EngineOwners.CharacterYanPuOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterYanPuOlEntries Create(Func<EngineOwners.CharacterYanPuOlRuntime> resolve) => new(resolve);
}
