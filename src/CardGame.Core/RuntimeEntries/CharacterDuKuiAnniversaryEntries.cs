#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDuKuiAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDuKuiAnniversaryRuntime> _resolve;
    private CharacterDuKuiAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDuKuiAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDuKuiAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDuKuiAnniversaryRuntime> resolve) => new(resolve);
}
