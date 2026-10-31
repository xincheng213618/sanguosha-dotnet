#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhenJiOlGodRuntime))]
internal sealed partial class CharacterZhenJiOlGodEntries
{
    private readonly Func<EngineOwners.CharacterZhenJiOlGodRuntime> _resolve;
    private CharacterZhenJiOlGodEntries(Func<EngineOwners.CharacterZhenJiOlGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhenJiOlGodEntries Create(Func<EngineOwners.CharacterZhenJiOlGodRuntime> resolve) => new(resolve);
}
