#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterCheZhouAnniversaryRuntime))]
internal sealed partial class CharacterCheZhouAnniversaryEntries
{
    private readonly Func<EngineOwners.CharacterCheZhouAnniversaryRuntime> _resolve;
    private CharacterCheZhouAnniversaryEntries(Func<EngineOwners.CharacterCheZhouAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCheZhouAnniversaryEntries Create(Func<EngineOwners.CharacterCheZhouAnniversaryRuntime> resolve) => new(resolve);
}
