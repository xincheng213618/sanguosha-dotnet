#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhugeGuoOlRuntime))]
internal sealed partial class CharacterZhugeGuoOlEntries
{
    private readonly Func<EngineOwners.CharacterZhugeGuoOlRuntime> _resolve;
    private CharacterZhugeGuoOlEntries(Func<EngineOwners.CharacterZhugeGuoOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhugeGuoOlEntries Create(Func<EngineOwners.CharacterZhugeGuoOlRuntime> resolve) => new(resolve);
}
