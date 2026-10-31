#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterLiFengOlRuntime))]
internal sealed partial class CharacterLiFengOlEntries
{
    private readonly Func<EngineOwners.CharacterLiFengOlRuntime> _resolve;
    private CharacterLiFengOlEntries(Func<EngineOwners.CharacterLiFengOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLiFengOlEntries Create(Func<EngineOwners.CharacterLiFengOlRuntime> resolve) => new(resolve);
}
