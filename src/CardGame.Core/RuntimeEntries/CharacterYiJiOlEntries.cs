#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYiJiOlEntries
{
    private readonly Func<EngineOwners.CharacterYiJiOlRuntime> _resolve;
    private CharacterYiJiOlEntries(Func<EngineOwners.CharacterYiJiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterYiJiOlEntries Create(Func<EngineOwners.CharacterYiJiOlRuntime> resolve) => new(resolve);
}
