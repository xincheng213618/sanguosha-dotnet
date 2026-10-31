#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheS5XuchuAiRuntime
{
    internal bool TrySelectShanheS5LuoyiAiOption(PendingDecision decision, ProgramSkillFrame frame, ProgramExecutionPlan plan, out PromptChoice choice)
    {
        choice = null !;
        var apply = plan.Instructions.Skip(frame.InstructionIndex).FirstOrDefault(e => e.Op == SkillProgramEffectOp.ResolveShanheS5Luoyi && e.StateId == "apply");
        if (apply is null || decision.PlayerSeat != frame.OwnerSeat)
            return false;
        var skip = decision.Choices.SingleOrDefault(c => c.Parameters.GetValueOrDefault("option-id") == ShanheS5LuoyiComposition.Replace);
        var normal = decision.Choices.SingleOrDefault(c => c.Parameters.GetValueOrDefault("option-id") == ShanheS5LuoyiComposition.Normal);
        if (skip is null || normal is null)
            return false;
        var parent = _coreShanheS5XuchuOperations.ShanheS5LuoyiDrawPlan(frame);
        var frozen = _coreShanheS5XuchuOperations.ShanheS5LuoyiFrozen(frame, apply.SourceBind!);
        var revealed = _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramCardsRevealedEvent>().Single(e => e.FrameId == frame.Id && e.Bind == apply.SourceBind);
        var gained = revealed.Cards.Where(c => ProgramCardSetFilter.Matches(c.Kind, c.Suit, [], [SkillProgramCardCategory.Basic], [EquipmentSlot.Weapon], [CardKind.Duel])).ToArray();
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        var normalCount = Math.Max(0, checked((parent.FrozenBaseDrawCount ?? _coreActualTurnsOperations.GetTurnDrawCount(owner)) + parent.NormalDrawAdjustment));
        // The revealed faces and X are public. A known acquired attack can exploit the full
        // next-owner-turn bonus; an existing anonymous hand has a smaller attack opportunity
        // prior. An empty hand with no acquired attack cannot immediately exploit that bonus.
        // Neither estimate reads an unrevealed deck face or another player's hand.
        var knownAttack = gained.Any(c => CoreNativeCardClassificationsRuntime.IsSlashCard(c.Kind) || c.Kind == CardKind.Duel);
        var bonusOpportunity = knownAttack ? 11d : _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Count > 0 ? 6d : 0d;
        var skipValue = gained.Length * 8d + frozen.X * bonusOpportunity;
        var normalValue = normalCount * 8d;
        choice = skipValue > normalValue ? skip : normal;
        return true;
    }
}
