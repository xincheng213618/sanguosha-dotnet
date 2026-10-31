#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGuoJiaMouEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoJiaMouRuntime> _resolve;
    private CharacterGuoJiaMouEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoJiaMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGuoJiaMouEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoJiaMouRuntime> resolve) => new(resolve);
}
