#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterMangYaChangAnniversaryRuntime))]
internal sealed partial class CharacterMangYaChangAnniversaryEntries
{
    private readonly Func<EngineOwners.CharacterMangYaChangAnniversaryRuntime> _resolve;
    private CharacterMangYaChangAnniversaryEntries(Func<EngineOwners.CharacterMangYaChangAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterMangYaChangAnniversaryEntries Create(Func<EngineOwners.CharacterMangYaChangAnniversaryRuntime> resolve) => new(resolve);
}
