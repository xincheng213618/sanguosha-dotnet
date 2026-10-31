#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterXunXuAnniversaryRuntime))]
internal sealed partial class CharacterXunXuAnniversaryEntries
{
    private readonly Func<EngineOwners.CharacterXunXuAnniversaryRuntime> _resolve;
    private CharacterXunXuAnniversaryEntries(Func<EngineOwners.CharacterXunXuAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXunXuAnniversaryEntries Create(Func<EngineOwners.CharacterXunXuAnniversaryRuntime> resolve) => new(resolve);
}
