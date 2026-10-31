#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class RoundGainedSourceUseCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineRoundGainedTrickTargetsEntries> getCoreGameEngineRoundGainedTrickTargetsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineRoundGainedEquipmentDrawsEntries> getCoreGameEngineRoundGainedEquipmentDrawsRuntime) : IRoundGainedSourceUseHost
{
    public SkillProgramStepOutcome AdjustOneRoundGainedOrdinaryTrickTarget(ProgramSkillFrame frame) => getCoreGameEngineRoundGainedTrickTargetsRuntime().OfferRoundGainedTrickTarget(frame);
    public SkillProgramStepOutcome DrawForRoundGainedEquipmentUse(ProgramSkillFrame frame) => getCoreGameEngineRoundGainedEquipmentDrawsRuntime().BeginRoundGainedEquipmentDraw(frame);
}
