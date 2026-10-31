#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterZhangLingOlRuntime))]
internal sealed partial class CharacterZhangLingOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangLingOlRuntime> _resolve;
    private CharacterZhangLingOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangLingOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangLingOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangLingOlRuntime> resolve) => new(resolve);
}
