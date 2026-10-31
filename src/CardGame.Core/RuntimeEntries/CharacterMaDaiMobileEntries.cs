#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterMaDaiMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaDaiMobileRuntime> _resolve;
    private CharacterMaDaiMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaDaiMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterMaDaiMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaDaiMobileRuntime> resolve) => new(resolve);
}
