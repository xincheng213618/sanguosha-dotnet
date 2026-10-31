#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterJiangJiMouRuntime))]
internal sealed partial class CharacterJiangJiMouEntries
{
    private readonly Func<EngineOwners.CharacterJiangJiMouRuntime> _resolve;
    private CharacterJiangJiMouEntries(Func<EngineOwners.CharacterJiangJiMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterJiangJiMouEntries Create(Func<EngineOwners.CharacterJiangJiMouRuntime> resolve) => new(resolve);
}
