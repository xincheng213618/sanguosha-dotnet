#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDamageTargetMountProgramsEntries
{
    internal global::CardGame.Core.CardUseFrame? ActualDamageTargetMountUse(int ownerSeat, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().ActualDamageTargetMountUse(ownerSeat, context);
    internal void AssertDamageTargetMountState(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertDamageTargetMountState(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginDamageTargetMount(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BeginDamageTargetMount(f);
    internal bool CanContinuePaidDamageTargetMount(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinuePaidDamageTargetMount(f);
    internal bool CanOfferDamageTargetMount(int ownerSeat, global::CardGame.Core.ProgramSkillWindowContext context, string? skillId = default) => _resolve().CanOfferDamageTargetMount(ownerSeat, context, skillId);
    internal bool CanOfferDamageTargetMountEdition(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferDamageTargetMountEdition(candidate, context);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> DamageTargetMountChoices(long frameId, int ownerSeat, int targetSeat, string? skillId = default) => _resolve().DamageTargetMountChoices(frameId, ownerSeat, targetSeat, skillId);
    internal bool IsValidDamageTargetMount(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IsValidDamageTargetMount(f);
    internal void ResolveDamageTargetMount(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveDamageTargetMount(choice);
    internal bool ResumeDamageTargetMount(long id) => _resolve().ResumeDamageTargetMount(id);
}
