#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterXieLingYuOlRuntime))]
internal sealed partial class CharacterXieLingYuOlEntries
{
    private readonly Func<EngineOwners.CharacterXieLingYuOlRuntime> _resolve;
    private CharacterXieLingYuOlEntries(Func<EngineOwners.CharacterXieLingYuOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXieLingYuOlEntries Create(Func<EngineOwners.CharacterXieLingYuOlRuntime> resolve) => new(resolve);
}
