#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterZhouQunOlRuntime))]
internal sealed partial class CharacterZhouQunOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouQunOlRuntime> _resolve;
    private CharacterZhouQunOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouQunOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhouQunOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouQunOlRuntime> resolve) => new(resolve);
}
