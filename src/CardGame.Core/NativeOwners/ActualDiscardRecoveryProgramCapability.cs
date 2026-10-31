#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ActualDiscardRecoveryProgramCapability(Func<RuntimeEntries.CoreGameEngineActualDiscardRecoveriesEntries> getCoreGameEngineActualDiscardRecoveriesRuntime, Func<RuntimeEntries.CoreGameEngineCapturedEquipmentDrawsEntries> getCoreGameEngineCapturedEquipmentDrawsRuntime) : IActualDiscardRecoveryProgramHost
{
    public SkillProgramStepOutcome RestoreActualDiscardBatch(ProgramSkillFrame f, string stateId) => getCoreGameEngineActualDiscardRecoveriesRuntime().BeginActualDiscardRecovery(f, stateId);
    public SkillProgramStepOutcome PlaceCapturedEquipmentAndDraw(ProgramSkillFrame f, int target, string bind, int draw) => getCoreGameEngineCapturedEquipmentDrawsRuntime().BeginCapturedEquipmentAndDraw(f, target, bind, draw);
}
