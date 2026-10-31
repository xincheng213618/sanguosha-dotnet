#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRoundOnceCardlessProgramRequestsEntries
{
    internal void AssertRoundOnceCardlessProgramRequestCompletion(global::CardGame.Core.CardUseFrame use) => _resolve().AssertRoundOnceCardlessProgramRequestCompletion(use);
    internal void BindRoundOnceCardlessProgramRequestUse(global::CardGame.Core.CardUseFrame use) => _resolve().BindRoundOnceCardlessProgramRequestUse(use);
    internal bool CanContinueIssuedRoundOnceCardlessProgramRequest(global::CardGame.Core.ProgramSkillFrame parent) => _resolve().CanContinueIssuedRoundOnceCardlessProgramRequest(parent);
    internal bool HasRoundOnceCardlessProgramSlash(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IEnumerable<int> targets) => _resolve().HasRoundOnceCardlessProgramSlash(actor, targets);
    internal bool ResumeRoundOnceCardlessProgramRequest(long id) => _resolve().ResumeRoundOnceCardlessProgramRequest(id);
    internal void ReturnRoundOnceCardlessProgramRequestUse(global::CardGame.Core.CardUseFrame completed) => _resolve().ReturnRoundOnceCardlessProgramRequestUse(completed);
    internal bool RoundOnceCardlessProgramActor(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffectOp op, int actor) => _resolve().RoundOnceCardlessProgramActor(f, op, actor);
    internal bool RoundOnceCardlessProgramRequestRidesOn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramSkillFrame parent) => _resolve().RoundOnceCardlessProgramRequestRidesOn(use, parent);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> RoundOnceCardlessProgramSlashChoices(global::CardGame.Core.ProgramSkillFrame frame, int actorSeat, global::System.Collections.Generic.IReadOnlyList<int> targets, string? resultBind = default) => _resolve().RoundOnceCardlessProgramSlashChoices(frame, actorSeat, targets, resultBind);
    internal bool RoundOnceCardlessProgramTargetLegal(global::CardGame.Core.CharacterState actor, int target, global::CardGame.Core.CardConversionSource source, bool ignoreDistance = false) => _resolve().RoundOnceCardlessProgramTargetLegal(actor, target, source, ignoreDistance);
    internal int[] RoundOnceCardlessProgramTargets(global::CardGame.Core.ProgramSkillFrame f, int actor, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().RoundOnceCardlessProgramTargets(f, actor, op);
    internal bool TryResolveRoundOnceCardlessProgramSlash(global::CardGame.Core.PromptChoice choice, bool advance) => _resolve().TryResolveRoundOnceCardlessProgramSlash(choice, advance);
    internal bool ValidRoundOnceCardlessProgramReceipt(global::CardGame.Core.ProgramSkillFrame parent, global::CardGame.Core.RoundOnceCardlessProgramRequestReceipt r, bool requireBound = true) => _resolve().ValidRoundOnceCardlessProgramReceipt(parent, r, requireBound);
}
