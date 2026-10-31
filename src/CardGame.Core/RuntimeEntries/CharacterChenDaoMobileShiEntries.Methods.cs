#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterChenDaoMobileShiEntries
{
    internal void AnswerShiCdChoice(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerShiCdChoice(choice);
    internal void AppendShiCdEnding(global::System.Collections.Generic.List<global::CardGame.Core.TurnEndingBoundaryItem> items) => _resolve().AppendShiCdEnding(items);
    internal void BindShiCdDraw(global::CardGame.Core.ProgramSkillFrame supplied, int count) => _resolve().BindShiCdDraw(supplied, count);
    internal bool CanOfferShiCd(global::CardGame.Core.ProgramTriggerLegalityContext g) => _resolve().CanOfferShiCd(g);
    internal void CaptureShiCdUse(global::CardGame.Core.IGameEvent payload) => _resolve().CaptureShiCdUse(payload);
    internal void ChangeShiCdMarks(global::CardGame.Core.ProgramSkillFrame f, int amount) => _resolve().ChangeShiCdMarks(f, amount);
    internal global::CardGame.Core.PromptChoice ChooseShiCdAi(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseShiCdAi(p, f);
    internal void CollectShiCdResponseUse(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.List<global::CardGame.Core.ProgramCardTriggerCandidate> result) => _resolve().CollectShiCdResponseUse(action, window, result);
    internal void CompleteShiCdWanglie(long actionId) => _resolve().CompleteShiCdWanglie(actionId);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> IncludeShiCdEnding(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> ordinary) => _resolve().IncludeShiCdEnding(owner, window, ordinary);
    internal bool IsIssuedShiCdEnding(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsIssuedShiCdEnding(c, context);
    internal bool IsIssuedShiCdResponse(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsIssuedShiCdResponse(c, context);
    internal bool IsMobileShiChenDaoChoiceResult(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramChoiceResultBinding b) => _resolve().IsMobileShiChenDaoChoiceResult(f, b);
    internal bool IsMobileShiWanglieUnrespondable(long id) => _resolve().IsMobileShiWanglieUnrespondable(id);
    internal bool KeepPaidShiCdDraw(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepPaidShiCdDraw(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> MobileShiChenDaoPublicStates(global::CardGame.Core.CharacterState owner, string skillId) => _resolve().MobileShiChenDaoPublicStates(owner, skillId);
    internal global::CardGame.Core.CardConversionSource? MobileShiWanglieCounterspellSource(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int> ids) => _resolve().MobileShiWanglieCounterspellSource(owner, ids);
    internal bool ObservesShiCdResponseUse(global::CardGame.Core.CardActionContext action) => _resolve().ObservesShiCdResponseUse(action);
    internal void PublishShiCdChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishShiCdChoice(f);
    internal void RequireShiCdChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireShiCdChoice(f);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolveShiCd(global::CardGame.Core.ProgramSkillFrame input, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().ResolveShiCd(input, op);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ShiCdChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ShiCdChoices(f);
    internal int ShiCdMarks(int owner) => _resolve().ShiCdMarks(owner);
    internal global::CardGame.Core.MobileShiHongyiPreparedEvent? ShiCdPendingEnding(int owner) => _resolve().ShiCdPendingEnding(owner);
    internal bool ShiCdWanglieBan(int seat, long? existingUse = default) => _resolve().ShiCdWanglieBan(seat, existingUse);
    internal bool ShiCdWanglieCardDistance(global::CardGame.Core.ProgramCardDistanceQuery q) => _resolve().ShiCdWanglieCardDistance(q);
    internal string? ShiCdWanglieCardLabel(int cardId) => _resolve().ShiCdWanglieCardLabel(cardId);
    internal global::CardGame.Core.MobileShiWanglieSelectedEvent? ShiCdWanglieGrant(int seat) => _resolve().ShiCdWanglieGrant(seat);
    internal bool ShiCdWanglieMaterials(int seat, global::System.Collections.Generic.IEnumerable<int>? ids) => _resolve().ShiCdWanglieMaterials(seat, ids);
    internal bool ShiCdWanglieOtherTargets(global::CardGame.Core.ProgramDirectedTargetQuery q) => _resolve().ShiCdWanglieOtherTargets(q);
    internal global::CardGame.Core.LegalAction? ShiCdWanglieProject(global::CardGame.Core.ProgramLegalActionQuery q) => _resolve().ShiCdWanglieProject(q);
    internal bool ShiCdWanglieSlashDistance(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().ShiCdWanglieSlashDistance(q);
}
