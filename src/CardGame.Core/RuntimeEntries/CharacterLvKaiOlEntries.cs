#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterLvKaiOlRuntime))]
internal sealed partial class CharacterLvKaiOlEntries
{
    private readonly Func<EngineOwners.CharacterLvKaiOlRuntime> _resolve;
    private CharacterLvKaiOlEntries(Func<EngineOwners.CharacterLvKaiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLvKaiOlEntries Create(Func<EngineOwners.CharacterLvKaiOlRuntime> resolve) => new(resolve);
}
