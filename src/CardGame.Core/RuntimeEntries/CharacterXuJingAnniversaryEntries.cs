#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterXuJingAnniversaryRuntime))]
internal sealed partial class CharacterXuJingAnniversaryEntries
{
    private readonly Func<EngineOwners.CharacterXuJingAnniversaryRuntime> _resolve;
    private CharacterXuJingAnniversaryEntries(Func<EngineOwners.CharacterXuJingAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXuJingAnniversaryEntries Create(Func<EngineOwners.CharacterXuJingAnniversaryRuntime> resolve) => new(resolve);
}
