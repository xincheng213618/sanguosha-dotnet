#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterJiangJiMouEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterJiangJiMouRuntime> _resolve;
    private CharacterJiangJiMouEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterJiangJiMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterJiangJiMouEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterJiangJiMouRuntime> resolve) => new(resolve);
}
