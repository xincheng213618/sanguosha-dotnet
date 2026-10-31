#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHeQiOlEntries
{
    internal void AssertRegisteredHeQi(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().AssertRegisteredHeQi(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginHeQi(global::CardGame.Core.ProgramSkillFrame supplied, int target) => _resolve().BeginHeQi(supplied, target);
    internal bool CanContinueEquipmentSuitProgram(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CanContinueEquipmentSuitProgram(frame);
    internal bool CanContinueHeQi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueHeQi(f);
    internal global::CardGame.Core.ProgramSkillFrame CaptureEquipmentSuitProgramSource(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CaptureEquipmentSuitProgramSource(frame);
    internal void ContinueHeQiAfterPublicReveal(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueHeQiAfterPublicReveal(f);
    internal int EquipmentSuitCount(global::CardGame.Core.CharacterState owner) => _resolve().EquipmentSuitCount(owner);
    internal bool EquipmentSuitSourceQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant source) => _resolve().EquipmentSuitSourceQualified(owner, source);
    internal void FinishHeQi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishHeQi(f);
    internal bool HasHeQiBeforeDamageObserver(long id) => _resolve().HasHeQiBeforeDamageObserver(id);
    internal bool HasHeQiDamageObserver(long id) => _resolve().HasHeQiDamageObserver(id);
    internal bool HeQiActivationAllowed(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramActivation a) => _resolve().HeQiActivationAllowed(owner, a);
    internal bool HeQiActivationTargetAllowed(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CharacterState target, global::CardGame.Core.SkillProgramActivation a) => _resolve().HeQiActivationTargetAllowed(owner, target, a);
    internal global::CardGame.Core.PromptChoice HeQiAiChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HeQiAiChoice(f);
    internal global::CardGame.Core.PromptChoice HeQiChoice(global::CardGame.Core.ProgramSkillFrame f, string branch, string label, global::System.Collections.Generic.IReadOnlyList<int>? cards = default) => _resolve().HeQiChoice(f, branch, label, cards);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> HeQiChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HeQiChoices(f);
    internal int HeQiEmptySlots(int seat) => _resolve().HeQiEmptySlots(seat);
    internal bool HeQiFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().HeQiFirstChild(root, child);
    internal global::CardGame.Core.ProgramSkillFrame? HeQiObserverRoot() => _resolve().HeQiObserverRoot();
    internal bool HeQiTargetAllowed(int owner, int target) => _resolve().HeQiTargetAllowed(owner, target);
    internal bool IsEquipmentSuitGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant) => _resolve().IsEquipmentSuitGrantQualified(owner, grant);
    internal bool IsHeQiActivation(global::CardGame.Core.SkillProgramActivation a) => _resolve().IsHeQiActivation(a);
    internal bool IsHeQiDying() => _resolve().IsHeQiDying();
    internal bool IsHeQiMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsHeQiMovement(f, e, p);
    internal void IssueHeQiSegment(long id, global::CardGame.Core.HeQiReceipt r) => _resolve().IssueHeQiSegment(id, r);
    internal void PublishHeQiChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishHeQiChoice(f);
    internal void RequireHeQi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireHeQi(f);
    internal void ResolveHeQiChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveHeQiChoice(choice);
    internal bool ResumeHeQi(long id) => _resolve().ResumeHeQi(id);
    internal bool ReturnHeQiMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnHeQiMovement(f);
    internal void SynchronizeEquipmentSuitSkillGrants() => _resolve().SynchronizeEquipmentSuitSkillGrants();
    internal bool TryAdvanceHeQiSubtree() => _resolve().TryAdvanceHeQiSubtree();
    internal bool ValidateEquipmentSuitProgramSource(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ValidateEquipmentSuitProgramSource(frame);
    internal global::System.Collections.Generic.IReadOnlySet<string> EquipmentSuitCapabilitySkillIds { get => _resolve().EquipmentSuitCapabilitySkillIds; }
}
