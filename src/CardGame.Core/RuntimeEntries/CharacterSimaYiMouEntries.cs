#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterSimaYiMouRuntime))]
internal sealed partial class CharacterSimaYiMouEntries
{
    private readonly Func<EngineOwners.CharacterSimaYiMouRuntime> _resolve;
    private CharacterSimaYiMouEntries(Func<EngineOwners.CharacterSimaYiMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSimaYiMouEntries Create(Func<EngineOwners.CharacterSimaYiMouRuntime> resolve) => new(resolve);
}
