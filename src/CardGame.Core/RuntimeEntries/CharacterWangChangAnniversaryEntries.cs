#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWangChangAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangChangAnniversaryRuntime> _resolve;
    private CharacterWangChangAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangChangAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWangChangAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangChangAnniversaryRuntime> resolve) => new(resolve);
}
