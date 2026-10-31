#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterCaiYangAnniversaryRuntime))]
internal sealed partial class CharacterCaiYangAnniversaryEntries
{
    private readonly Func<EngineOwners.CharacterCaiYangAnniversaryRuntime> _resolve;
    private CharacterCaiYangAnniversaryEntries(Func<EngineOwners.CharacterCaiYangAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaiYangAnniversaryEntries Create(Func<EngineOwners.CharacterCaiYangAnniversaryRuntime> resolve) => new(resolve);
}
