#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterKanZeAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterKanZeAnniversaryRuntime> _resolve;
    private CharacterKanZeAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterKanZeAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterKanZeAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterKanZeAnniversaryRuntime> resolve) => new(resolve);
}
