#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterNiuJinOlEntries
{
    private readonly Func<EngineOwners.CharacterNiuJinOlRuntime> _resolve;
    private CharacterNiuJinOlEntries(Func<EngineOwners.CharacterNiuJinOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterNiuJinOlEntries Create(Func<EngineOwners.CharacterNiuJinOlRuntime> resolve) => new(resolve);
}
