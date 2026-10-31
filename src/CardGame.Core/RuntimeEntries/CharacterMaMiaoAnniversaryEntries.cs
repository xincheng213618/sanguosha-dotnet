#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterMaMiaoAnniversaryRuntime))]
internal sealed partial class CharacterMaMiaoAnniversaryEntries
{
    private readonly Func<EngineOwners.CharacterMaMiaoAnniversaryRuntime> _resolve;
    private CharacterMaMiaoAnniversaryEntries(Func<EngineOwners.CharacterMaMiaoAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterMaMiaoAnniversaryEntries Create(Func<EngineOwners.CharacterMaMiaoAnniversaryRuntime> resolve) => new(resolve);
}
