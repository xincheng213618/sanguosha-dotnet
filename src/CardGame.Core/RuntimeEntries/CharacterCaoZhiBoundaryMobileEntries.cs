#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoZhiBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoZhiBoundaryMobileRuntime> _resolve;
    private CharacterCaoZhiBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoZhiBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoZhiBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoZhiBoundaryMobileRuntime> resolve) => new(resolve);
}
