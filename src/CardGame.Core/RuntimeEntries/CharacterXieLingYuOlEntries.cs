#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterXieLingYuOlRuntime))]
internal sealed partial class CharacterXieLingYuOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXieLingYuOlRuntime> _resolve;
    private CharacterXieLingYuOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXieLingYuOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXieLingYuOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXieLingYuOlRuntime> resolve) => new(resolve);
}
