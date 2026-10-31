#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePairedHandRecastEntries
{
    internal SkillProgramStepOutcome BeginPairedHandRecast(ProgramSkillFrame supplied, int partnerSeat) => _resolve().BeginPairedHandRecast(supplied, partnerSeat);
    internal bool CanContinuePairedHandRecast(ProgramSkillFrame f) => _resolve().CanContinuePairedHandRecast(f);
    internal bool DrainPairedHandRecastChildren(ProgramSkillFrame f) => _resolve().DrainPairedHandRecastChildren(f);
    internal void IssuePairedHandRecastCost(ProgramSkillFrame f) => _resolve().IssuePairedHandRecastCost(f);
    internal void IssuePairedHandRecastDraw(ProgramSkillFrame f) => _resolve().IssuePairedHandRecastDraw(f);
    internal PairedHandRecastMaterial[] PairedHandMaterials(int seat) => _resolve().PairedHandMaterials(seat);
    internal IReadOnlyList<PromptChoice> PairedHandRecastChoices(ProgramSkillFrame f) => _resolve().PairedHandRecastChoices(f);
    internal bool PairedRecastMaterialStillOwned(PairedHandRecastParticipantReceipt participant) => _resolve().PairedRecastMaterialStillOwned(participant);
    internal void PublishPairedHandRecastChoice(ProgramSkillFrame f) => _resolve().PublishPairedHandRecastChoice(f);
    internal void ResolvePairedHandRecastChoice(PromptChoice choice) => _resolve().ResolvePairedHandRecastChoice(choice);
    internal bool ResumePairedHandRecast(long frameId) => _resolve().ResumePairedHandRecast(frameId);
    internal bool ReturnPairedHandRecastMovement(ProgramSkillFrame f) => _resolve().ReturnPairedHandRecastMovement(f);
    internal PromptChoice SelectAiPairedHandRecast(PendingDecision decision, ProgramSkillFrame f) => _resolve().SelectAiPairedHandRecast(decision, f);
    internal long PairedHandRecastSequence { get => _resolve().PairedHandRecastSequence; }
}
