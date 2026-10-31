#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterDuKuiAnniversaryRuntime))]
internal sealed partial class CharacterDuKuiAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDuKuiAnniversaryRuntime> _resolve;
    private CharacterDuKuiAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDuKuiAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDuKuiAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDuKuiAnniversaryRuntime> resolve) => new(resolve);
}
