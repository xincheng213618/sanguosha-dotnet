#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDongZhaoMobileShiEntries
{
    internal void AnswerShiDz(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerShiDz(choice);
    internal void AssertShiDz(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertShiDz(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShiDz(global::CardGame.Core.ProgramSkillFrame input, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginShiDz(input, op);
    internal bool CanOfferShiDz(global::CardGame.Core.ProgramTriggerLegalityContext q) => _resolve().CanOfferShiDz(q);
    internal global::CardGame.Core.PromptChoice ChooseShiDzAi(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseShiDzAi(p, f);
    internal void ContinueShiDz(long id) => _resolve().ContinueShiDz(id);
    internal void FinishShiDz(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishShiDz(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> IncludeShiDzExtra(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> ordinary) => _resolve().IncludeShiDzExtra(owner, window, ordinary);
    internal bool IsIssuedShiDzExtra(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().IsIssuedShiDzExtra(c, w);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> MobileShiDongZhaoPublicStates(global::CardGame.Core.CharacterState owner, string skillId) => _resolve().MobileShiDongZhaoPublicStates(owner, skillId);
    internal bool PaidShiDzCanReturn(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PaidShiDzCanReturn(f);
    internal void PublishShiDz(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishShiDz(f);
    internal bool ResumeShiDz(long id) => _resolve().ResumeShiDz(id);
    internal bool ReturnShiDz(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnShiDz(f);
    internal void SealShiDz(long id) => _resolve().SealShiDz(id);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ShiDzChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ShiDzChoices(f);
    internal bool ShiDzMovementProof(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation movement) => _resolve().ShiDzMovementProof(f, effect, movement);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ShiDzPayments(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ShiDzPayments(f);
    internal global::CardGame.Core.MobileShiDongZhaoExtraGrantedEvent? ShiDzPendingExtra(int beneficiary) => _resolve().ShiDzPendingExtra(beneficiary);
    internal int ShiDzSameNameMaximum(int seat) => _resolve().ShiDzSameNameMaximum(seat);
}
