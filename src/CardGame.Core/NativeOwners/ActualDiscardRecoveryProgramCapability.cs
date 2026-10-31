#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ActualDiscardRecoveryProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineActualDiscardRecoveriesRuntime>> getCoreGameEngineActualDiscardRecoveriesRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineCapturedEquipmentDrawsRuntime>> getCoreGameEngineCapturedEquipmentDrawsRuntime) : IActualDiscardRecoveryProgramHost
{
    public SkillProgramStepOutcome RestoreActualDiscardBatch(ProgramSkillFrame f, string stateId) => getCoreGameEngineActualDiscardRecoveriesRuntime().Value.BeginActualDiscardRecovery(f, stateId);
    public SkillProgramStepOutcome PlaceCapturedEquipmentAndDraw(ProgramSkillFrame f, int target, string bind, int draw) => getCoreGameEngineCapturedEquipmentDrawsRuntime().Value.BeginCapturedEquipmentAndDraw(f, target, bind, draw);
}
