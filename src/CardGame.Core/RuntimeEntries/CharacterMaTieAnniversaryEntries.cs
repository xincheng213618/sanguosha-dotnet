#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterMaTieAnniversaryRuntime))]
internal sealed partial class CharacterMaTieAnniversaryEntries
{
    private readonly Func<EngineOwners.CharacterMaTieAnniversaryRuntime> _resolve;
    private CharacterMaTieAnniversaryEntries(Func<EngineOwners.CharacterMaTieAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterMaTieAnniversaryEntries Create(Func<EngineOwners.CharacterMaTieAnniversaryRuntime> resolve) => new(resolve);
}
