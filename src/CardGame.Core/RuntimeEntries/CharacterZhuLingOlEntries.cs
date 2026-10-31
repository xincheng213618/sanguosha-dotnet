#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhuLingOlEntries
{
    private readonly Func<EngineOwners.CharacterZhuLingOlRuntime> _resolve;
    private CharacterZhuLingOlEntries(Func<EngineOwners.CharacterZhuLingOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhuLingOlEntries Create(Func<EngineOwners.CharacterZhuLingOlRuntime> resolve) => new(resolve);
}
