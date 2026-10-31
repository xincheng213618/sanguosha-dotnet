#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterNiuFuOlRuntime))]
internal sealed partial class CharacterNiuFuOlEntries
{
    private readonly Func<EngineOwners.CharacterNiuFuOlRuntime> _resolve;
    private CharacterNiuFuOlEntries(Func<EngineOwners.CharacterNiuFuOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterNiuFuOlEntries Create(Func<EngineOwners.CharacterNiuFuOlRuntime> resolve) => new(resolve);
}
