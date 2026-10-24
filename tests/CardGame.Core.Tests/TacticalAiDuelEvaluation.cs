using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

/// <summary>
/// An explicitly requested diagnostic experiment, not a registered smoke check.
/// Policies compete within the same game; paired games swap their camps while
/// retaining the exact initial seed, roles, generals, hands and engine policy.
/// Trusted setup/outcome diagnostics never become inputs to a brain.
/// </summary>
internal static class TacticalAiDuelEvaluation
{
    private static readonly FieldInfo BrainsField = typeof(GameEngine).GetField(
        "_aiBrains", BindingFlags.Instance | BindingFlags.NonPublic) ??
        throw new InvalidOperationException("The AI evaluation fixture cannot find the engine brain map.");

    public static int Run(
        string output,
        int pairs = 12,
        int baselinePolicy = AiDecisionPolicy.CooperativeCategoryGiftVersion,
        int? candidatePolicy = null,
        int firstPair = 0)
    {
        var candidate = candidatePolicy ?? AiDecisionPolicy.CurrentVersion;
        if (pairs is < 1 or > 120)
            throw new ArgumentOutOfRangeException(nameof(pairs), "The diagnostic accepts 1 to 120 paired setups.");
        if (firstPair is < 0 or > 10000)
            throw new ArgumentOutOfRangeException(nameof(firstPair), "The first paired setup must be between 0 and 10000.");
        if (!AiDecisionPolicy.IsSupported(baselinePolicy) || !AiDecisionPolicy.IsSupported(candidate) || candidate == baselinePolicy)
            throw new ArgumentException("Supply two distinct supported AI policies.");

        var registry = StandardContentRegistry.CreateWithActiveSkillsAndTeamModes();
        var matches = new List<MatchResult>(pairs * 2);
        var paired = new List<PairResult>(pairs);
        for (var offset = 0; offset < pairs; offset++)
        {
            var index = firstPair + offset;
            // Three symmetric team pairs for each identity diagnostic pair.
            // Identity comparisons rotate the focal camp rather than collapsing
            // the three different win conditions into a single playing-strength rate.
            var team = index % 4 != 3;
            var focal = team ? "team:blue" : ((index / 4) % 3) switch
            {
                0 => "LordAndLoyalists",
                1 => "Rebels",
                _ => "Renegade"
            };
            var seed = 724000 + index * 7919;
            var first = Play(registry, index, seed, team, focal, true, baselinePolicy, candidate);
            var second = Play(registry, index, seed, team, focal, false, baselinePolicy, candidate);
            var sameSetup = first.InitialStateHash == second.InitialStateHash;
            if (!sameSetup)
                throw new InvalidOperationException($"Paired setup {index} changed its initial state when policies were swapped.");
            matches.Add(first);
            matches.Add(second);
            paired.Add(new PairResult(index, first.Mode, focal, sameSetup,
                first.Success && second.Success,
                first.FocalWin, second.FocalWin,
                first.FocalWin is { } firstWin && second.FocalWin is { } secondWin
                    ? (firstWin ? 1 : 0) - (secondWin ? 1 : 0)
                    : null));
            Console.WriteLine($"AI duel pair {offset + 1}/{pairs} (setup {index}): {first.Mode}, {focal}; " +
                $"candidate-on-focal={first.WinnerCamp}, baseline-on-focal={second.WinnerCamp}" +
                (first.Success && second.Success ? "" : "; FAILED"));
        }

        var attemptedTeamMatches = matches.Where(match => match.Mode == "team:standard-2v2").ToArray();
        var teamMatches = attemptedTeamMatches.Where(match => match.Success).ToArray();
        var completeTeamPairs = paired.Where(pair => pair.Mode == "team:standard-2v2" && pair.Success)
            .Select(pair => pair.Pair).ToHashSet();
        var completeTeamMatches = attemptedTeamMatches.Where(match => completeTeamPairs.Contains(match.Pair)).ToArray();
        var identity = paired.Where(pair => pair.Mode != "team:standard-2v2").GroupBy(pair => pair.FocalCamp).Select(group => new
        {
            Camp = group.Key,
            Pairs = group.Count(),
            SuccessfulPairs = group.Count(pair => pair.Success),
            FocalWinsWithCandidate = group.Count(pair => pair.Success && pair.CandidateFocalWin == true),
            FocalWinsWithBaseline = group.Count(pair => pair.Success && pair.BaselineFocalWin == true),
            NetFocalWins = group.Where(pair => pair.Success).Sum(pair => pair.FocalWinDelta ?? 0)
        }).ToArray();
        var summary = new
        {
            BaselinePolicy = baselinePolicy,
            CandidatePolicy = candidate,
            HostPolicy = baselinePolicy,
            FirstPair = firstPair,
            LastPairInclusive = firstPair + pairs - 1,
            Pairs = paired.Count,
            Games = matches.Count,
            SuccessfulGames = matches.Count(match => match.Success),
            FailedGames = matches.Count(match => !match.Success),
            PrimaryTeam = new
            {
                PairedSetups = paired.Count(pair => pair.Mode == "team:standard-2v2"),
                AttemptedGames = attemptedTeamMatches.Length,
                FailedGames = attemptedTeamMatches.Count(match => !match.Success),
                SuccessfulGames = teamMatches.Length,
                CandidateWins = teamMatches.Count(match => match.WinnerPolicy == candidate),
                BaselineWins = teamMatches.Count(match => match.WinnerPolicy == baselinePolicy),
                Draws = teamMatches.Count(match => match.WinnerCamp == "Draw"),
                CandidatePointRate = teamMatches.Length == 0 ? (double?)null : Math.Round(
                    teamMatches.Sum(match => match.WinnerPolicy == candidate ? 1d : match.WinnerCamp == "Draw" ? .5d : 0d) / teamMatches.Length, 4),
                CandidatePointRateDenominator = "All successful team games, including a successful partner from an incomplete pair. Prefer CompletePairedSetups for balanced policy comparison.",
                CompletePairedSetups = new
                {
                    Pairs = completeTeamPairs.Count,
                    Games = completeTeamMatches.Length,
                    CandidateWins = completeTeamMatches.Count(match => match.WinnerPolicy == candidate),
                    BaselineWins = completeTeamMatches.Count(match => match.WinnerPolicy == baselinePolicy),
                    Draws = completeTeamMatches.Count(match => match.WinnerCamp == "Draw"),
                    CandidatePointRate = completeTeamMatches.Length == 0 ? (double?)null : Math.Round(
                        completeTeamMatches.Sum(match => match.WinnerPolicy == candidate ? 1d : match.WinnerCamp == "Draw" ? .5d : 0d) /
                        completeTeamMatches.Length, 4)
                }
            },
            IdentityDiagnostics = identity,
            MeanGameMilliseconds = Math.Round(matches.Average(match => match.Milliseconds), 2),
            MaximumGameMilliseconds = Math.Round(matches.Max(match => match.Milliseconds), 2),
            MaximumTurns = matches.Max(match => match.Turns),
            Limit = "Small paired diagnostic sample on the standard roster and deck, not a significance test or evidence for all generals. " +
                "The two legs are correlated. Complete paired team setups are the balanced primary comparison; all failures and successful unpaired partners remain in the full evidence. " +
                "Identity camps have different win conditions and are reported separately. " +
                "Mixed policies are injected only in this test fixture and are not replayable checkpoints."
        };
        var destination = Path.GetFullPath(output);
        Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
        var jsonOptions = new JsonSerializerOptions { WriteIndented = true };
        File.WriteAllText(destination, JsonSerializer.Serialize(new
        {
            Method = "Fixed-seed noninteractive setup; policy assignments are swapped between the same camps in each pair. " +
                "Both legs use the baseline host policy, exact engine seat seeds, and includeJudgmentMaterials=false. " +
                "Only SimpleAiBrain differs, isolating brain scoring from host version gates. Initial full-state hashes must match. " +
                "AI decision methods continue to receive the engine's own viewer snapshots; trusted roles/hands are used only for assignment and audit. " +
                "Every completed game must have an empty resolution stack, zero processing cards and the exact initial physical-card inventory. " +
                "The turn cap is 240; draws remain visible rather than being counted as wins.",
            Summary = summary,
            Pairs = paired,
            Matches = matches
        }, jsonOptions));
        Console.WriteLine(JsonSerializer.Serialize(summary, jsonOptions));
        Console.WriteLine($"Saved paired AI evidence: {destination}");
        return matches.All(match => match.Success) ? 0 : 1;
    }

    private static MatchResult Play(
        ContentRegistry registry,
        int pair,
        int seed,
        bool team,
        string focalCamp,
        bool candidateOnFocal,
        int baseline,
        int candidate)
    {
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = seed,
            PlayerCount = team ? 4 : 5,
            ModeId = team ? "team:standard-2v2" : null,
            HumanSeat = -1,
            HumanRole = null,
            UseInteractiveSetup = false,
            AiPolicyVersion = baseline,
            MaxTurns = 240
        }, registry);
        var initial = game.CreateSnapshot(-1, true);
        var initialHash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(SnapshotJson.Serialize(initial))));
        var inventory = game.CreateCardZoneDiagnostics().Select(card => card.CardId).Order().ToArray();
        if (inventory.Distinct().Count() != inventory.Length)
            throw new InvalidOperationException("Initial evaluation inventory contains duplicate physical cards.");
        var brains = BrainsField.GetValue(game) as Dictionary<int, SimpleAiBrain> ??
            throw new InvalidOperationException("The AI evaluation fixture found an unexpected engine brain map.");
        if (brains.Count != initial.Players.Count || initial.Players.Any(player => player.IsHuman || !brains.ContainsKey(player.Seat)))
            throw new InvalidOperationException("Every evaluation seat must have an engine-created AI.");
        var roster = initial.Players.Select(player =>
        {
            var camp = team ? player.TeamId! : Camp(player.Role ?? throw new InvalidOperationException("Identity setup omitted a role."));
            var policy = (camp == focalCamp) == candidateOnFocal ? candidate : baseline;
            var brain = new SimpleAiBrain(player.Seat, unchecked(seed * 397) ^ (player.Seat + 1),
                policy, includeJudgmentMaterials: false);
            brains[player.Seat] = brain;
            if (!ReferenceEquals(brains[player.Seat], brain))
                throw new InvalidOperationException("The evaluation did not replace the requested seat's AI.");
            return new SeatSetup(player.Seat, player.GeneralId, camp, policy);
        }).ToArray();
        if (roster.All(seat => seat.Policy == baseline) || roster.All(seat => seat.Policy == candidate) ||
            game.CreateCheckpoint().Options.AiPolicyVersion != baseline)
            throw new InvalidOperationException("The evaluation must compare mixed brains under an identical baseline host policy.");

        var watch = Stopwatch.StartNew();
        var accepted = false;
        string? error = null;
        try
        {
            accepted = game.Submit(new StartGameCommand()).Accepted;
        }
        catch (Exception exception)
        {
            // Retain bounded diagnostic evidence and run the paired leg too.
            // A fault must not silently vanish from the denominator.
            var details = exception.ToString();
            error = details.Length <= 16384 ? details : details[..16384] + "\n[exception detail truncated at 16384 characters]";
        }
        watch.Stop();
        var final = game.CreateSnapshot(-1, true);
        var finalInventory = game.CreateCardZoneDiagnostics().Select(card => card.CardId).Order().ToArray();
        var inventoryPreserved = inventory.SequenceEqual(finalInventory) && finalInventory.Distinct().Count() == inventory.Length;
        var settled = game.ResolutionStack.Count == 0 && final.ProcessingCardCount == 0;
        var success = error is null && accepted && final.Status == EngineStatus.Completed && settled && inventoryPreserved;
        var winner = team ? final.WinnerTeamId ?? final.Winner.ToString() : final.Winner.ToString();
        var winnerPolicy = roster.FirstOrDefault(seat => seat.Camp == winner)?.Policy;
        return new MatchResult(pair, candidateOnFocal ? "candidate-on-focal" : "baseline-on-focal",
            seed, team ? "team:standard-2v2" : "identity:standard-5", focalCamp,
            candidateOnFocal ? candidate : baseline, baseline, initialHash, roster,
            success, final.Status.ToString(), winner, winnerPolicy,
            success ? winner == focalCamp : null, final.TurnNumber,
            Math.Round(watch.Elapsed.TotalMilliseconds, 2), accepted, settled, inventoryPreserved,
            inventory.Length, game.Events.Count, game.AiThoughts.Count, error);
    }

    private static string Camp(Role role) => role switch
    {
        Role.Lord or Role.Loyalist => "LordAndLoyalists",
        Role.Rebel => "Rebels",
        Role.Renegade => "Renegade",
        _ => throw new ArgumentOutOfRangeException(nameof(role))
    };

    private sealed record SeatSetup(int Seat, string GeneralId, string Camp, int Policy);
    private sealed record PairResult(int Pair, string Mode, string FocalCamp, bool SameInitialState,
        bool Success, bool? CandidateFocalWin, bool? BaselineFocalWin, int? FocalWinDelta);
    private sealed record MatchResult(int Pair, string Leg, int Seed, string Mode, string FocalCamp,
        int FocalPolicy, int HostPolicy, string InitialStateHash, IReadOnlyList<SeatSetup> Roster,
        bool Success, string Status, string WinnerCamp, int? WinnerPolicy, bool? FocalWin,
        int Turns, double Milliseconds, bool StartAccepted, bool Settled, bool InventoryPreserved,
        int PhysicalCards, int Events, int Thoughts, string? Error);
}
