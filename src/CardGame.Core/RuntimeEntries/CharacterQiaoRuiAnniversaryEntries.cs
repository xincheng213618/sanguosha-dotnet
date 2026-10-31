#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterQiaoRuiAnniversaryRuntime))]
internal sealed partial class CharacterQiaoRuiAnniversaryEntries
{
    private readonly Func<EngineOwners.CharacterQiaoRuiAnniversaryRuntime> _resolve;
    private CharacterQiaoRuiAnniversaryEntries(Func<EngineOwners.CharacterQiaoRuiAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterQiaoRuiAnniversaryEntries Create(Func<EngineOwners.CharacterQiaoRuiAnniversaryRuntime> resolve) => new(resolve);
}
