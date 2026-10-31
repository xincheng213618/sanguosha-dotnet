#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterXunYuMouRuntime))]
internal sealed partial class CharacterXunYuMouEntries
{
    private readonly Func<EngineOwners.CharacterXunYuMouRuntime> _resolve;
    private CharacterXunYuMouEntries(Func<EngineOwners.CharacterXunYuMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXunYuMouEntries Create(Func<EngineOwners.CharacterXunYuMouRuntime> resolve) => new(resolve);
}
