#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterLiuHongOlRuntime))]
internal sealed partial class CharacterLiuHongOlEntries
{
    private readonly Func<EngineOwners.CharacterLiuHongOlRuntime> _resolve;
    private CharacterLiuHongOlEntries(Func<EngineOwners.CharacterLiuHongOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLiuHongOlEntries Create(Func<EngineOwners.CharacterLiuHongOlRuntime> resolve) => new(resolve);
}
