#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterWenYangOlRuntime))]
internal sealed partial class CharacterWenYangOlEntries
{
    private readonly Func<EngineOwners.CharacterWenYangOlRuntime> _resolve;
    private CharacterWenYangOlEntries(Func<EngineOwners.CharacterWenYangOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWenYangOlEntries Create(Func<EngineOwners.CharacterWenYangOlRuntime> resolve) => new(resolve);
}
