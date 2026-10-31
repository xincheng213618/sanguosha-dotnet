#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterHuZhaoAnniversaryRuntime))]
internal sealed partial class CharacterHuZhaoAnniversaryEntries
{
    private readonly Func<EngineOwners.CharacterHuZhaoAnniversaryRuntime> _resolve;
    private CharacterHuZhaoAnniversaryEntries(Func<EngineOwners.CharacterHuZhaoAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuZhaoAnniversaryEntries Create(Func<EngineOwners.CharacterHuZhaoAnniversaryRuntime> resolve) => new(resolve);
}
