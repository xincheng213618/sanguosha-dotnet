#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangXiuMouEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangXiuMouRuntime> _resolve;
    private CharacterZhangXiuMouEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangXiuMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangXiuMouEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangXiuMouRuntime> resolve) => new(resolve);
}
