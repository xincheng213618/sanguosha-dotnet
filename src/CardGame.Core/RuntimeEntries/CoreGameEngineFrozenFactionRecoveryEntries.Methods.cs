#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFrozenFactionRecoveryEntries
{
    internal void AssertFrozenFactionRecovery(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertFrozenFactionRecovery(f);
    internal void DrawProgramToFrozenFactionCount(global::CardGame.Core.ProgramSkillFrame input) => _resolve().DrawProgramToFrozenFactionCount(input);
    internal void FreezeProgramLivingFactionRecovery(global::CardGame.Core.ProgramSkillFrame input) => _resolve().FreezeProgramLivingFactionRecovery(input);
}
