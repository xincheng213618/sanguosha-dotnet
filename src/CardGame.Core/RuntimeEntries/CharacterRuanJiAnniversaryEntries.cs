#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterRuanJiAnniversaryRuntime))]
internal sealed partial class CharacterRuanJiAnniversaryEntries
{
    private readonly Func<EngineOwners.CharacterRuanJiAnniversaryRuntime> _resolve;
    private CharacterRuanJiAnniversaryEntries(Func<EngineOwners.CharacterRuanJiAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterRuanJiAnniversaryEntries Create(Func<EngineOwners.CharacterRuanJiAnniversaryRuntime> resolve) => new(resolve);
}
