#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterYuanYinAnniversaryRuntime))]
internal sealed partial class CharacterYuanYinAnniversaryEntries
{
    private readonly Func<EngineOwners.CharacterYuanYinAnniversaryRuntime> _resolve;
    private CharacterYuanYinAnniversaryEntries(Func<EngineOwners.CharacterYuanYinAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterYuanYinAnniversaryEntries Create(Func<EngineOwners.CharacterYuanYinAnniversaryRuntime> resolve) => new(resolve);
}
