#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class ActualDiscardRecoveryProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineActualDiscardRecoveriesEntries> getCoreGameEngineActualDiscardRecoveriesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCapturedEquipmentDrawsEntries> getCoreGameEngineCapturedEquipmentDrawsRuntime) : IActualDiscardRecoveryProgramHost
{
    public SkillProgramStepOutcome RestoreActualDiscardBatch(ProgramSkillFrame f, string stateId) => getCoreGameEngineActualDiscardRecoveriesRuntime().BeginActualDiscardRecovery(f, stateId);
    public SkillProgramStepOutcome PlaceCapturedEquipmentAndDraw(ProgramSkillFrame f, int target, string bind, int draw) => getCoreGameEngineCapturedEquipmentDrawsRuntime().BeginCapturedEquipmentAndDraw(f, target, bind, draw);
}
