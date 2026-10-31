#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterSimaShiMouRuntime))]
internal sealed partial class CharacterSimaShiMouEntries
{
    private readonly Func<EngineOwners.CharacterSimaShiMouRuntime> _resolve;
    private CharacterSimaShiMouEntries(Func<EngineOwners.CharacterSimaShiMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSimaShiMouEntries Create(Func<EngineOwners.CharacterSimaShiMouRuntime> resolve) => new(resolve);
}
