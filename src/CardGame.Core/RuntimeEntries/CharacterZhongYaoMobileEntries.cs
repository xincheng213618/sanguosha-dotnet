#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhongYaoMobileEntries
{
    private readonly Func<EngineOwners.CharacterZhongYaoMobileRuntime> _resolve;
    private CharacterZhongYaoMobileEntries(Func<EngineOwners.CharacterZhongYaoMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhongYaoMobileEntries Create(Func<EngineOwners.CharacterZhongYaoMobileRuntime> resolve) => new(resolve);
}
