#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDuanWeiOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDuanWeiOlRuntime> _resolve;
    private CharacterDuanWeiOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDuanWeiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDuanWeiOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDuanWeiOlRuntime> resolve) => new(resolve);
}
