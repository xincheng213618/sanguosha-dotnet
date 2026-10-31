#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterQuHuangOlRuntime))]
internal sealed partial class CharacterQuHuangOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterQuHuangOlRuntime> _resolve;
    private CharacterQuHuangOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterQuHuangOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterQuHuangOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterQuHuangOlRuntime> resolve) => new(resolve);
}
