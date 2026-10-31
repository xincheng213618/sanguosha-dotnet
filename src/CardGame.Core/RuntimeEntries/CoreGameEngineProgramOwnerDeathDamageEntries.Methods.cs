#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramOwnerDeathDamageEntries
{
    internal bool IsOriginalDyingSuspendedByOwnerDeathDamage(global::CardGame.Core.DyingFrame dying) => _resolve().IsOriginalDyingSuspendedByOwnerDeathDamage(dying);
    internal bool IsOwnerDeathProgramDamage(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().IsOwnerDeathProgramDamage(frame);
    internal bool IsOwnerDeathSuspendedDying(long programFrameId) => _resolve().IsOwnerDeathSuspendedDying(programFrameId);
}
