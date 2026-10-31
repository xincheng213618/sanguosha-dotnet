#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhangWenAnniversaryRuntime))]
internal sealed partial class CharacterZhangWenAnniversaryEntries
{
    private readonly Func<EngineOwners.CharacterZhangWenAnniversaryRuntime> _resolve;
    private CharacterZhangWenAnniversaryEntries(Func<EngineOwners.CharacterZhangWenAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangWenAnniversaryEntries Create(Func<EngineOwners.CharacterZhangWenAnniversaryRuntime> resolve) => new(resolve);
}
