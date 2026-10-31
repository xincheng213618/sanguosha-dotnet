#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLvDaiAnniversaryEntries
{
    private readonly Func<EngineOwners.CharacterLvDaiAnniversaryRuntime> _resolve;
    private CharacterLvDaiAnniversaryEntries(Func<EngineOwners.CharacterLvDaiAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLvDaiAnniversaryEntries Create(Func<EngineOwners.CharacterLvDaiAnniversaryRuntime> resolve) => new(resolve);
}
