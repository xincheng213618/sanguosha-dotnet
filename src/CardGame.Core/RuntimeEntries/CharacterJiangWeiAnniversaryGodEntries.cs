#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterJiangWeiAnniversaryGodRuntime))]
internal sealed partial class CharacterJiangWeiAnniversaryGodEntries
{
    private readonly Func<EngineOwners.CharacterJiangWeiAnniversaryGodRuntime> _resolve;
    private CharacterJiangWeiAnniversaryGodEntries(Func<EngineOwners.CharacterJiangWeiAnniversaryGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterJiangWeiAnniversaryGodEntries Create(Func<EngineOwners.CharacterJiangWeiAnniversaryGodRuntime> resolve) => new(resolve);
}
