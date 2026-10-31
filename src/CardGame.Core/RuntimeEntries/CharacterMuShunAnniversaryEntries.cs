#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterMuShunAnniversaryRuntime))]
internal sealed partial class CharacterMuShunAnniversaryEntries
{
    private readonly Func<EngineOwners.CharacterMuShunAnniversaryRuntime> _resolve;
    private CharacterMuShunAnniversaryEntries(Func<EngineOwners.CharacterMuShunAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterMuShunAnniversaryEntries Create(Func<EngineOwners.CharacterMuShunAnniversaryRuntime> resolve) => new(resolve);
}
