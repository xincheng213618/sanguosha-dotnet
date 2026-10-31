#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterJiangWeiMouEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterJiangWeiMouRuntime> _resolve;
    private CharacterJiangWeiMouEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterJiangWeiMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterJiangWeiMouEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterJiangWeiMouRuntime> resolve) => new(resolve);
}
