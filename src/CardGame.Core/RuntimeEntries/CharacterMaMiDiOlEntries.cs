#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterMaMiDiOlRuntime))]
internal sealed partial class CharacterMaMiDiOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaMiDiOlRuntime> _resolve;
    private CharacterMaMiDiOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaMiDiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterMaMiDiOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaMiDiOlRuntime> resolve) => new(resolve);
}
