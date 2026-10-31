#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRoundOnceCardlessProgramRequestsEntries
{
    internal void AssertRoundOnceCardlessProgramRequestCompletion(CardUseFrame use) => _resolve().AssertRoundOnceCardlessProgramRequestCompletion(use);
    internal void BindRoundOnceCardlessProgramRequestUse(CardUseFrame use) => _resolve().BindRoundOnceCardlessProgramRequestUse(use);
    internal bool CanContinueIssuedRoundOnceCardlessProgramRequest(ProgramSkillFrame parent) => _resolve().CanContinueIssuedRoundOnceCardlessProgramRequest(parent);
    internal bool HasRoundOnceCardlessProgramSlash(CharacterState actor, IEnumerable<int> targets) => _resolve().HasRoundOnceCardlessProgramSlash(actor, targets);
    internal bool ResumeRoundOnceCardlessProgramRequest(long id) => _resolve().ResumeRoundOnceCardlessProgramRequest(id);
    internal void ReturnRoundOnceCardlessProgramRequestUse(CardUseFrame completed) => _resolve().ReturnRoundOnceCardlessProgramRequestUse(completed);
    internal bool RoundOnceCardlessProgramActor(ProgramSkillFrame f, SkillProgramEffectOp op, int actor) => _resolve().RoundOnceCardlessProgramActor(f, op, actor);
    internal bool RoundOnceCardlessProgramRequestRidesOn(CardUseFrame use, ProgramSkillFrame parent) => _resolve().RoundOnceCardlessProgramRequestRidesOn(use, parent);
    internal IReadOnlyList<PromptChoice> RoundOnceCardlessProgramSlashChoices(ProgramSkillFrame frame, int actorSeat, IReadOnlyList<int> targets, string? resultBind = default) => _resolve().RoundOnceCardlessProgramSlashChoices(frame, actorSeat, targets, resultBind);
    internal bool RoundOnceCardlessProgramTargetLegal(CharacterState actor, int target, CardConversionSource source, bool ignoreDistance = false) => _resolve().RoundOnceCardlessProgramTargetLegal(actor, target, source, ignoreDistance);
    internal int[] RoundOnceCardlessProgramTargets(ProgramSkillFrame f, int actor, SkillProgramEffectOp op) => _resolve().RoundOnceCardlessProgramTargets(f, actor, op);
    internal bool TryResolveRoundOnceCardlessProgramSlash(PromptChoice choice, bool advance) => _resolve().TryResolveRoundOnceCardlessProgramSlash(choice, advance);
    internal bool ValidRoundOnceCardlessProgramReceipt(ProgramSkillFrame parent, RoundOnceCardlessProgramRequestReceipt r, bool requireBound = true) => _resolve().ValidRoundOnceCardlessProgramReceipt(parent, r, requireBound);
}
