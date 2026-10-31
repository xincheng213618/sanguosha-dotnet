#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhangFenAnniversaryRuntime))]
internal sealed partial class CharacterZhangFenAnniversaryEntries
{
    private readonly Func<EngineOwners.CharacterZhangFenAnniversaryRuntime> _resolve;
    private CharacterZhangFenAnniversaryEntries(Func<EngineOwners.CharacterZhangFenAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangFenAnniversaryEntries Create(Func<EngineOwners.CharacterZhangFenAnniversaryRuntime> resolve) => new(resolve);
}
