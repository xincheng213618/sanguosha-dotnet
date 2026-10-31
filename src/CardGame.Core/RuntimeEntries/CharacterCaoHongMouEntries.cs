#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterCaoHongMouRuntime))]
internal sealed partial class CharacterCaoHongMouEntries
{
    private readonly Func<EngineOwners.CharacterCaoHongMouRuntime> _resolve;
    private CharacterCaoHongMouEntries(Func<EngineOwners.CharacterCaoHongMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoHongMouEntries Create(Func<EngineOwners.CharacterCaoHongMouRuntime> resolve) => new(resolve);
}
