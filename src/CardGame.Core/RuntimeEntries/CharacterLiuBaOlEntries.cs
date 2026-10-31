#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterLiuBaOlRuntime))]
internal sealed partial class CharacterLiuBaOlEntries
{
    private readonly Func<EngineOwners.CharacterLiuBaOlRuntime> _resolve;
    private CharacterLiuBaOlEntries(Func<EngineOwners.CharacterLiuBaOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLiuBaOlEntries Create(Func<EngineOwners.CharacterLiuBaOlRuntime> resolve) => new(resolve);
}
