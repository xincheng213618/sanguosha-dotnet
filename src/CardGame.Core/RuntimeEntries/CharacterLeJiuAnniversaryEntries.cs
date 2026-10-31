#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterLeJiuAnniversaryRuntime))]
internal sealed partial class CharacterLeJiuAnniversaryEntries
{
    private readonly Func<EngineOwners.CharacterLeJiuAnniversaryRuntime> _resolve;
    private CharacterLeJiuAnniversaryEntries(Func<EngineOwners.CharacterLeJiuAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLeJiuAnniversaryEntries Create(Func<EngineOwners.CharacterLeJiuAnniversaryRuntime> resolve) => new(resolve);
}
