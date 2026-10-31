#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePairedHandRecastEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginPairedHandRecast(global::CardGame.Core.ProgramSkillFrame supplied, int partnerSeat) => _resolve().BeginPairedHandRecast(supplied, partnerSeat);
    internal bool CanContinuePairedHandRecast(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinuePairedHandRecast(f);
    internal bool DrainPairedHandRecastChildren(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainPairedHandRecastChildren(f);
    internal bool IsPairedHandRecastMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsPairedHandRecastMovement(f, effect, pending);
    internal void IssuePairedHandRecastCost(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssuePairedHandRecastCost(f);
    internal void IssuePairedHandRecastDraw(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssuePairedHandRecastDraw(f);
    internal global::CardGame.Core.PairedHandRecastMaterial[] PairedHandMaterials(int seat) => _resolve().PairedHandMaterials(seat);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> PairedHandRecastChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PairedHandRecastChoices(f);
    internal bool PairedRecastMaterialStillOwned(global::CardGame.Core.PairedHandRecastParticipantReceipt participant) => _resolve().PairedRecastMaterialStillOwned(participant);
    internal void PublishPairedHandRecastChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishPairedHandRecastChoice(f);
    internal void ResolvePairedHandRecastChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolvePairedHandRecastChoice(choice);
    internal bool ResumePairedHandRecast(long frameId) => _resolve().ResumePairedHandRecast(frameId);
    internal bool ReturnPairedHandRecastMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnPairedHandRecastMovement(f);
    internal global::CardGame.Core.PromptChoice SelectAiPairedHandRecast(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiPairedHandRecast(decision, f);
    internal long PairedHandRecastSequence { get => _resolve().PairedHandRecastSequence; }
}
