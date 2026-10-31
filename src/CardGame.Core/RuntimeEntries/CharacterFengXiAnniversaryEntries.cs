#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterFengXiAnniversaryRuntime))]
internal sealed partial class CharacterFengXiAnniversaryEntries
{
    private readonly Func<EngineOwners.CharacterFengXiAnniversaryRuntime> _resolve;
    private CharacterFengXiAnniversaryEntries(Func<EngineOwners.CharacterFengXiAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterFengXiAnniversaryEntries Create(Func<EngineOwners.CharacterFengXiAnniversaryRuntime> resolve) => new(resolve);
}
