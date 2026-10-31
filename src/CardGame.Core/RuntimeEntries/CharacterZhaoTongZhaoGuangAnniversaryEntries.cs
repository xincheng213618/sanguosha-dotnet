#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhaoTongZhaoGuangAnniversaryRuntime))]
internal sealed partial class CharacterZhaoTongZhaoGuangAnniversaryEntries
{
    private readonly Func<EngineOwners.CharacterZhaoTongZhaoGuangAnniversaryRuntime> _resolve;
    private CharacterZhaoTongZhaoGuangAnniversaryEntries(Func<EngineOwners.CharacterZhaoTongZhaoGuangAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhaoTongZhaoGuangAnniversaryEntries Create(Func<EngineOwners.CharacterZhaoTongZhaoGuangAnniversaryRuntime> resolve) => new(resolve);
}
