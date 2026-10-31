#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterCaoAngMouRuntime))]
internal sealed partial class CharacterCaoAngMouEntries
{
    private readonly Func<EngineOwners.CharacterCaoAngMouRuntime> _resolve;
    private CharacterCaoAngMouEntries(Func<EngineOwners.CharacterCaoAngMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoAngMouEntries Create(Func<EngineOwners.CharacterCaoAngMouRuntime> resolve) => new(resolve);
}
