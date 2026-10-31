#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterKeBiNengOlRuntime))]
internal sealed partial class CharacterKeBiNengOlEntries
{
    private readonly Func<EngineOwners.CharacterKeBiNengOlRuntime> _resolve;
    private CharacterKeBiNengOlEntries(Func<EngineOwners.CharacterKeBiNengOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterKeBiNengOlEntries Create(Func<EngineOwners.CharacterKeBiNengOlRuntime> resolve) => new(resolve);
}
