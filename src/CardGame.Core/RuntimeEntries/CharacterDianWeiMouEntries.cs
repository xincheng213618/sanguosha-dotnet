#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDianWeiMouEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDianWeiMouRuntime> _resolve;
    private CharacterDianWeiMouEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDianWeiMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDianWeiMouEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDianWeiMouRuntime> resolve) => new(resolve);
}
