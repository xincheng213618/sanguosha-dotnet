#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterYangFengOlRuntime))]
internal sealed partial class CharacterYangFengOlEntries
{
    private readonly Func<EngineOwners.CharacterYangFengOlRuntime> _resolve;
    private CharacterYangFengOlEntries(Func<EngineOwners.CharacterYangFengOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterYangFengOlEntries Create(Func<EngineOwners.CharacterYangFengOlRuntime> resolve) => new(resolve);
}
