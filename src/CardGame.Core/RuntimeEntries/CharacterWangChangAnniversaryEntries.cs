#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterWangChangAnniversaryRuntime))]
internal sealed partial class CharacterWangChangAnniversaryEntries
{
    private readonly Func<EngineOwners.CharacterWangChangAnniversaryRuntime> _resolve;
    private CharacterWangChangAnniversaryEntries(Func<EngineOwners.CharacterWangChangAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWangChangAnniversaryEntries Create(Func<EngineOwners.CharacterWangChangAnniversaryRuntime> resolve) => new(resolve);
}
