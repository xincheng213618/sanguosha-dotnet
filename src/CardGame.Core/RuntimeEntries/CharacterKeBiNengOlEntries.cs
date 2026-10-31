#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterKeBiNengOlRuntime))]
internal sealed partial class CharacterKeBiNengOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterKeBiNengOlRuntime> _resolve;
    private CharacterKeBiNengOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterKeBiNengOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterKeBiNengOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterKeBiNengOlRuntime> resolve) => new(resolve);
}
