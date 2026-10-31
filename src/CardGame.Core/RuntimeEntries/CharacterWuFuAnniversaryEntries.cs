#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterWuFuAnniversaryRuntime))]
internal sealed partial class CharacterWuFuAnniversaryEntries
{
    private readonly Func<EngineOwners.CharacterWuFuAnniversaryRuntime> _resolve;
    private CharacterWuFuAnniversaryEntries(Func<EngineOwners.CharacterWuFuAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWuFuAnniversaryEntries Create(Func<EngineOwners.CharacterWuFuAnniversaryRuntime> resolve) => new(resolve);
}
