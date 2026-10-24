namespace CardGame.Core;

public sealed partial class GameEngine
{
    private IModeEncounterHost CreateModeEncounterHost() => new ModeEncounterHost(this);

    /// <summary>Native state mutations only. Encounter qualification and transition order live in mode features.</summary>
    private sealed class ModeEncounterHost(GameEngine engine) : IModeEncounterHost
    {
        public ModeEncounterFacts CaptureFacts() => new(engine._setupComplete, engine._winner != Winner.None,
            engine._playerCount, engine._options.HumanSeat, engine._currentSeat, engine._turnNumber,
            engine._phase, engine._resolutionStack.Count, engine._pendingDecision?.Kind,
            Array.AsReadOnly(engine._players.Select(player => new ModeEncounterPlayerFacts(player.Seat,
                player.Name, player.General.Id, player.IsAlive, player.Hp, player.MaxHp, player.TeamId)).ToArray()));

        public IReadOnlyList<TEvent> History<TEvent>() where TEvent : IGameEvent =>
            Array.AsReadOnly(engine.CompleteProgramEventHistory().OfType<TEvent>().ToArray());

        public ModeEncounterGeneralFacts General(string generalId)
        {
            var general = engine._generalPool.Single(candidate => candidate.Id == generalId);
            return new(general.Id, general.Name, general.BaseHp, general.InitialHp ?? general.BaseHp);
        }

        public int NextRandom(int exclusiveMaximum) => engine._random.Next(exclusiveMaximum);

        public void ReplaceCombatant(ModeCombatantReplacement request)
        {
            ArgumentNullException.ThrowIfNull(request);
            if (!engine.IsValidPlayerSeat(request.Seat) || engine._players[request.Seat].General.Id != request.ExpectedGeneralId)
                throw new InvalidOperationException("An encounter replacement lost its original combatant.");
            var previous = engine._players[request.Seat];
            var general = engine._generalPool.Single(candidate => candidate.Id == request.GeneralId);
            CharacterState replacement;
            if (request.PreserveCurrentState)
            {
                replacement = previous;
                replacement.General = general;
                replacement.MaxHp = general.BaseHp;
                replacement.Hp = general.InitialHp ?? general.BaseHp;
            }
            else
            {
                replacement = new CharacterState
                {
                    Seat = previous.Seat, Name = previous.Name, IsHuman = false, Role = previous.Role,
                    TeamId = previous.TeamId, TeamRevealed = true, RoleRevealed = true,
                    General = general, GeneralSelected = true, GeneralRevealed = true,
                    Hp = general.InitialHp ?? general.BaseHp, MaxHp = general.BaseHp
                };
                if (engine.HasPrivateGeneralLibraryCapability) replacement.GeneralLibraryGenderQuery = engine.GetPrivateGeneralLibraryGender;
                engine._players[replacement.Seat] = replacement;
                engine._skillRuntimeState.ResetOwner(replacement.Seat);
                foreach (var key in engine._programUses.Keys.Where(key => key.Seat == replacement.Seat).ToArray()) engine._programUses.Remove(key);
                foreach (var key in engine._programPhaseUses.Keys.Where(key => key.Seat == replacement.Seat).ToArray()) engine._programPhaseUses.Remove(key);
                foreach (var key in engine._programContributionUses.Keys.Where(key => key.SkillOwnerSeat == replacement.Seat).ToArray()) engine._programContributionUses.Remove(key);
                foreach (var key in engine._programBooleanStates.Keys.Where(key => key.OwnerSeat == replacement.Seat).ToArray()) engine._programBooleanStates.Remove(key);
                engine._observedSkillGrantRevisions[replacement.Seat] = replacement.SkillGrants.Revision;
            }
            engine._skillBindingIndex.Invalidate(replacement.Seat);
            foreach (var id in engine.EnabledContentSkillIds(replacement)) engine.RegisterTaggedConversionSkill(replacement, id);
        }

        public void ClearDecision(bool resumeRunning = false)
        {
            engine._pendingDecision = null;
            if (resumeRunning) engine._status = EngineStatus.Running;
        }

        public void CancelResolutionTree(Func<ModeCancelledResolution, IGameEvent> cancellationFact)
        {
            ArgumentNullException.ThrowIfNull(cancellationFact);
            // Retiring an actual-turn subtree has no ordinary child return.
            // Already paid physical cards stay paid until native processing cleanup.
            while (engine._resolutionStack.LastOrDefault() is { } top)
            {
                engine._resolutionStack.CompleteTop(top.Id, top.Kind);
                engine.AdvanceEventRulesAndQueueFact(cancellationFact(new(top.Id, top.Kind, engine._turnNumber)));
            }
        }

        public void ClearTransientBatches()
        {
            engine._pendingCardsMovedBatches.Clear();
            engine._pendingHpChanges.Clear();
            engine._pendingCharacterStateChanges.Clear();
        }

        public void DiscardProcessingCards(string reason) => engine.MoveAllCards(CardLocation.Processing,
            CardLocation.DiscardPile, new CardMoveReason(reason));

        public void FinishInterruptedTurn(int expectedSeat, int expectedTurnNumber,
            string skillGrantEndReason, IGameEvent interruptionFact)
        {
            void AssertOriginalTurn()
            {
                if (!engine.IsValidPlayerSeat(expectedSeat) || engine._currentSeat != expectedSeat ||
                    engine._turnNumber != expectedTurnNumber)
                    throw new InvalidOperationException("Interrupted-turn cleanup lost its original actual turn.");
            }
            AssertOriginalTurn();
            var previous = engine._players[expectedSeat];
            engine.CloseCurrentPlayPhaseSkillGrants(skillGrantEndReason);
            engine.CancelScheduledProgramPhase();
            engine.ClearActualDiscardRecoveryPhase();
            AssertOriginalTurn();
            engine.ExpireActualTurnEndEffects(previous);
            AssertOriginalTurn();
            engine._phase = TurnPhase.Finished;
            engine.AdvanceEventRulesAndQueueFact(interruptionFact);
            AssertOriginalTurn();
            engine.AdvanceEventRulesAndQueueFact(new TurnEndedEvent(expectedTurnNumber, previous.Seat));
            AssertOriginalTurn();
        }

        public void BeginNaturalTurnAt(int seat)
        {
            if (!engine.IsValidPlayerSeat(seat)) throw new ArgumentOutOfRangeException(nameof(seat));
            engine._currentSeat = seat;
            engine._turnProgression = new(ActualTurnKind.Normal, seat, engine._turnNumber);
            engine._phase = TurnPhase.NotStarted;
        }

        public void EndCurrentTurn() => engine.EndTurn();
        public void DrawInitialCards(int seat, int count) => engine.DrawCards(engine._players[seat], count,
            log: true, CardMoveReasons.InitialDeal);
        public void DeclareTeamWinner(string teamId)
        {
            engine._winnerTeamId = teamId;
            engine._winner = engine.GetWinnerForTeam(teamId);
            engine.AdvanceEventRulesAndQueueFact(new WinnerDeterminedEvent(engine._winner, teamId, null));
        }
        public void PublishFact(IGameEvent fact) => engine.AdvanceEventRulesAndQueueFact(fact);
        public void PublishState() => engine.AdvanceRulesAndPublishState();
        public void Log(string kind, string message, int seat) => engine.AddLog(kind, message, seat);
    }
}
