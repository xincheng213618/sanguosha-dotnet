#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramDamageInsertionRuntimeEntries
{
    internal bool IsOriginalDyingSuspendedByProgramInsertion(global::CardGame.Core.DyingFrame dying) => _resolve().IsOriginalDyingSuspendedByProgramInsertion(dying);
    internal global::CardGame.Core.ProgramDamageInsertionRuntime DamageInsertions { get => _resolve().DamageInsertions; }
}
