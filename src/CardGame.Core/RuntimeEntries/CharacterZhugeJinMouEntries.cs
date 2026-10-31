#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhugeJinMouEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeJinMouRuntime> _resolve;
    private CharacterZhugeJinMouEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeJinMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhugeJinMouEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeJinMouRuntime> resolve) => new(resolve);
}
