#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterGuoZhaoAnniversaryRuntime))]
internal sealed partial class CharacterGuoZhaoAnniversaryEntries
{
    private readonly Func<EngineOwners.CharacterGuoZhaoAnniversaryRuntime> _resolve;
    private CharacterGuoZhaoAnniversaryEntries(Func<EngineOwners.CharacterGuoZhaoAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGuoZhaoAnniversaryEntries Create(Func<EngineOwners.CharacterGuoZhaoAnniversaryRuntime> resolve) => new(resolve);
}
