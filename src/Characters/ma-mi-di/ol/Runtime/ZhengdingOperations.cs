using System.Text.Json.Serialization;
namespace CardGame.Core;
internal sealed class GrowZhengdingMatchingResponseDescriptor() : MaMiDiDescriptor(SkillProgramEffectOp.GrowZhengdingMatchingResponse, SkillProgramTriggerWindow.CardResponseCompleted) {
    public override ProgramFactRequirements FactRequirements => new(Triggered: ProgramTrackedFact.NativeActionAppearance); }
