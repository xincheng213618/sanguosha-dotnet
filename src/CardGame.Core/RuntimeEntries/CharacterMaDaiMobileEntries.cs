#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterMaDaiMobileEntries
{
    private readonly Func<EngineOwners.CharacterMaDaiMobileRuntime> _resolve;
    private CharacterMaDaiMobileEntries(Func<EngineOwners.CharacterMaDaiMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterMaDaiMobileEntries Create(Func<EngineOwners.CharacterMaDaiMobileRuntime> resolve) => new(resolve);
}
