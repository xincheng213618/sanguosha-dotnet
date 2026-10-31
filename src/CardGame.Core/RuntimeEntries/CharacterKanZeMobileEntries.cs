#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterKanZeMobileEntries
{
    private readonly Func<EngineOwners.CharacterKanZeMobileRuntime> _resolve;
    private CharacterKanZeMobileEntries(Func<EngineOwners.CharacterKanZeMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterKanZeMobileEntries Create(Func<EngineOwners.CharacterKanZeMobileRuntime> resolve) => new(resolve);
}
