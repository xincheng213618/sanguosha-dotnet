#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhouFangOlEntries
{
    private readonly Func<EngineOwners.CharacterZhouFangOlRuntime> _resolve;
    private CharacterZhouFangOlEntries(Func<EngineOwners.CharacterZhouFangOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhouFangOlEntries Create(Func<EngineOwners.CharacterZhouFangOlRuntime> resolve) => new(resolve);
}
