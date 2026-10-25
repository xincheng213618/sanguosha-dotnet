namespace CardGame.Core;

public sealed partial class GameEngine
{
    [ProgramAiChoice(SkillProgramEffectOp.ApplyMobileWuYiBenxi)]
    private PromptChoice MobileWuYiBenxiAi(PendingDecision decision, ProgramSkillFrame frame, SkillProgramEffect effect) => decision.Choices.First();
    [ProgramCapability]
    private sealed class MobileWuYiBenxiHost(GameEngine engine) : IMobileWuYiBenxiHost
    { public SkillProgramStepOutcome Apply(ProgramSkillFrame frame) => engine.ApplyMobileWuYiBenxi(frame); }

    private SkillProgramStepOutcome ApplyMobileWuYiBenxi(ProgramSkillFrame supplied)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        var use = EnhancementCardUse(frame);
        if (_currentSeat != frame.OwnerSeat || _players.Any(p => p.IsAlive && p.Seat != frame.OwnerSeat && GetCombatDistance(frame.OwnerSeat, p.Seat) != 1))
            return SkillProgramStepOutcome.Continue;
        if (frame.CardEnhancementDraft is not null) throw new InvalidOperationException("Mobile Benxi cannot reissue an unfinished current-card benefit.");
        ReplaceRuntimeFrame(use.Id, use = use with { IgnoresArmor = true, Enhancements = use.Enhancements | CurrentCardEnhancement.IgnoreArmor, EnhancementOwnerSeat = frame.OwnerSeat });
        if (ActiveCardAttack is { } attack && attack.ResolutionId == use.Id) attack.SetIgnoresArmor(true);
        AdvanceEventRulesAndQueueFact(new CurrentCardEnhancedEvent(use.Id, use.Action!.ActionId, frame.OwnerSeat, CurrentCardEnhancement.IgnoreArmor));
        if (!IsSlashCard(use.CardKind) || CurrentCardExtraTargets(frame).Length == 0) return SkillProgramStepOutcome.Continue;
        ReplaceRuntimeTop(frame with { CardEnhancementDraft = new(use.Id, Array.AsReadOnly(new[] { CurrentCardEnhancement.IgnoreArmor })) });
        PublishCardEnhancementPrompt(GetActiveProgramFrame(frame.Id));
        return SkillProgramStepOutcome.AwaitChoice;
    }
}
