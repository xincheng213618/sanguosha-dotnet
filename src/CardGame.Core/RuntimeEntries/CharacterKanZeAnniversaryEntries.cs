#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterKanZeAnniversaryRuntime))]
internal sealed partial class CharacterKanZeAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterKanZeAnniversaryRuntime> _resolve;
    private CharacterKanZeAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterKanZeAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterKanZeAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterKanZeAnniversaryRuntime> resolve) => new(resolve);
}
