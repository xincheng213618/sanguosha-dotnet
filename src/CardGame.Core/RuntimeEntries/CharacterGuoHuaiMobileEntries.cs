#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGuoHuaiMobileEntries
{
    private readonly Func<EngineOwners.CharacterGuoHuaiMobileRuntime> _resolve;
    private CharacterGuoHuaiMobileEntries(Func<EngineOwners.CharacterGuoHuaiMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGuoHuaiMobileEntries Create(Func<EngineOwners.CharacterGuoHuaiMobileRuntime> resolve) => new(resolve);
}
