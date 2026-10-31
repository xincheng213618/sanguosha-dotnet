#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterChenDaoMobileShiEntries
{
    internal void AnswerShiCdChoice(PromptChoice choice) => _resolve().AnswerShiCdChoice(choice);
    internal void AppendShiCdEnding(List<TurnEndingBoundaryItem> items) => _resolve().AppendShiCdEnding(items);
    internal void BindShiCdDraw(ProgramSkillFrame supplied, int count) => _resolve().BindShiCdDraw(supplied, count);
    internal bool CanOfferShiCd(ProgramTriggerLegalityContext g) => _resolve().CanOfferShiCd(g);
    internal void CaptureShiCdUse(IGameEvent payload) => _resolve().CaptureShiCdUse(payload);
    internal void ChangeShiCdMarks(ProgramSkillFrame f, int amount) => _resolve().ChangeShiCdMarks(f, amount);
    internal PromptChoice ChooseShiCdAi(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseShiCdAi(p, f);
    internal void CollectShiCdResponseUse(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result) => _resolve().CollectShiCdResponseUse(action, window, result);
    internal void CompleteShiCdWanglie(long actionId) => _resolve().CompleteShiCdWanglie(actionId);
    internal IReadOnlyList<ProgramTriggerCandidate> IncludeShiCdEnding(CharacterState owner, SkillProgramTriggerWindow window, IReadOnlyList<ProgramTriggerCandidate> ordinary) => _resolve().IncludeShiCdEnding(owner, window, ordinary);
    internal bool IsIssuedShiCdEnding(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().IsIssuedShiCdEnding(c, context);
    internal bool IsIssuedShiCdResponse(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().IsIssuedShiCdResponse(c, context);
    internal bool IsMobileShiChenDaoChoiceResult(ProgramSkillFrame f, ProgramChoiceResultBinding b) => _resolve().IsMobileShiChenDaoChoiceResult(f, b);
    internal bool IsMobileShiWanglieUnrespondable(long id) => _resolve().IsMobileShiWanglieUnrespondable(id);
    internal bool KeepPaidShiCdDraw(ProgramSkillFrame f) => _resolve().KeepPaidShiCdDraw(f);
    internal IReadOnlyList<ProgramPublicRuleStateSnapshot> MobileShiChenDaoPublicStates(CharacterState owner, string skillId) => _resolve().MobileShiChenDaoPublicStates(owner, skillId);
    internal CardConversionSource? MobileShiWanglieCounterspellSource(CharacterState owner, IReadOnlyList<int> ids) => _resolve().MobileShiWanglieCounterspellSource(owner, ids);
    internal bool ObservesShiCdResponseUse(CardActionContext action) => _resolve().ObservesShiCdResponseUse(action);
    internal void PublishShiCdChoice(ProgramSkillFrame f) => _resolve().PublishShiCdChoice(f);
    internal void RequireShiCdChoice(ProgramSkillFrame f) => _resolve().RequireShiCdChoice(f);
    internal SkillProgramStepOutcome ResolveShiCd(ProgramSkillFrame input, SkillProgramEffectOp op) => _resolve().ResolveShiCd(input, op);
    internal IReadOnlyList<PromptChoice> ShiCdChoices(ProgramSkillFrame f) => _resolve().ShiCdChoices(f);
    internal int ShiCdMarks(int owner) => _resolve().ShiCdMarks(owner);
    internal MobileShiHongyiPreparedEvent? ShiCdPendingEnding(int owner) => _resolve().ShiCdPendingEnding(owner);
    internal bool ShiCdWanglieBan(int seat, long? existingUse = default) => _resolve().ShiCdWanglieBan(seat, existingUse);
    internal bool ShiCdWanglieCardDistance(ProgramCardDistanceQuery q) => _resolve().ShiCdWanglieCardDistance(q);
    internal string? ShiCdWanglieCardLabel(int cardId) => _resolve().ShiCdWanglieCardLabel(cardId);
    internal MobileShiWanglieSelectedEvent? ShiCdWanglieGrant(int seat) => _resolve().ShiCdWanglieGrant(seat);
    internal bool ShiCdWanglieMaterials(int seat, IEnumerable<int>? ids) => _resolve().ShiCdWanglieMaterials(seat, ids);
    internal bool ShiCdWanglieOtherTargets(ProgramDirectedTargetQuery q) => _resolve().ShiCdWanglieOtherTargets(q);
    internal LegalAction? ShiCdWanglieProject(ProgramLegalActionQuery q) => _resolve().ShiCdWanglieProject(q);
    internal bool ShiCdWanglieSlashDistance(ProgramSlashDistanceQuery q) => _resolve().ShiCdWanglieSlashDistance(q);
}
