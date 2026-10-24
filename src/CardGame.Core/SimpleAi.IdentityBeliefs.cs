namespace CardGame.Core;

public sealed partial class SimpleAiBrain
{
    private static readonly Role[] IdentityRoles = [Role.Lord, Role.Loyalist, Role.Rebel, Role.Renegade];
    private readonly Queue<IdentityAttackObservation> _identityUnresolvedAttacks = new();
    private long _identityObservationRevision;
    private GameSnapshot? _identityBeliefView;
    private long _identityBeliefRevision = -1;
    private Dictionary<int, IdentityRolePosterior> _identityBeliefs = [];

    private readonly record struct IdentityAttackObservation(int SourceSeat, int TargetSeat, long Revision);

    private void CopyIdentityEvidenceTo(SimpleAiBrain copy)
    {
        // Detached participant decisions retain this observer's public history,
        // but recompute posteriors from their own supplied snapshot and cache.
        foreach (var attack in _identityUnresolvedAttacks) copy._identityUnresolvedAttacks.Enqueue(attack);
        copy._identityObservationRevision = _identityObservationRevision;
    }

    private sealed record IdentityRolePosterior(double Lord, double Loyalist, double Rebel, double Renegade)
    {
        public double Probability(Role role) => role switch
        {
            Role.Lord => Lord, Role.Loyalist => Loyalist, Role.Rebel => Rebel,
            Role.Renegade => Renegade, _ => 0d
        };
    }

    // These hooks accept the same public evidence as historical policies. An AOE
    // cannot select the Lord independently from its other mandatory recipients,
    // so its per-target notifications do not establish a directed role signal.
    private void ObserveIdentityPublicAttack(PublicAttackEvidence evidence)
    {
        if (evidence.Kind == PublicAttackKind.GroupAttack || evidence.SourceSeat < 0 ||
            evidence.TargetSeat < 0 || evidence.SourceSeat == evidence.TargetSeat)
            return;
        AgeIdentityEvidence();
        if (evidence.SourceSeat == Seat) return;

        var delta = evidence.TargetSeat == evidence.LordSeat ? 3d : evidence.RevealedTargetRole switch
        {
            Role.Rebel => -1.4d,
            Role.Loyalist => 1.6d,
            _ => 0d
        };
        if (evidence.TargetSeat != evidence.LordSeat && evidence.RevealedTargetRole is null)
        {
            // An uncertain opponent is weak evidence, not a known faction. Its
            // later public death can clarify this observation for this viewer.
            delta -= Math.Clamp(_rebelSuspicion.GetValueOrDefault(evidence.TargetSeat) * .18d, -.7d, .7d);
            _identityUnresolvedAttacks.Enqueue(new(evidence.SourceSeat, evidence.TargetSeat, _identityObservationRevision));
            while (_identityUnresolvedAttacks.Count > 128) _identityUnresolvedAttacks.Dequeue();
        }
        _rebelSuspicion[evidence.SourceSeat] = Math.Clamp(
            _rebelSuspicion.GetValueOrDefault(evidence.SourceSeat) + delta, -6d, 6d);
    }

    private void ObserveIdentityDeath(int? killerSeat, Role revealedVictimRole)
    {
        AgeIdentityEvidence();
        if (killerSeat is null || killerSeat == Seat || killerSeat < 0) return;
        var delta = revealedVictimRole switch
        {
            Role.Rebel => -2.25d,
            Role.Loyalist => 2.75d,
            Role.Lord => 4d,
            _ => 0d
        };
        _rebelSuspicion[killerSeat.Value] = Math.Clamp(
            _rebelSuspicion.GetValueOrDefault(killerSeat.Value) + delta, -6d, 6d);
    }

    private void AgeIdentityEvidence()
    {
        // A public-event clock works before this observer's first turn too. No
        // inferred identity becomes permanent merely because an early guess was
        // made; contradictory directed actions can reverse the bounded signal.
        foreach (var seat in _rebelSuspicion.Keys.ToArray()) _rebelSuspicion[seat] *= .97d;
        _identityObservationRevision++;
    }

    private double GetIdentityRoleProbability(GameSnapshot view, PlayerSnapshot target, Role role)
    {
        EnsureIdentityBeliefs(view);
        return _identityBeliefs.TryGetValue(target.Seat, out var belief) ? belief.Probability(role) : 0d;
    }

    private double GetIdentityHostility(GameSnapshot view, Role role, PlayerSnapshot target)
    {
        if (target.Seat == Seat) return -1000d;
        EnsureIdentityBeliefs(view);
        if (!_identityBeliefs.TryGetValue(target.Seat, out var belief)) return 0d;
        var hostility = role switch
        {
            Role.Lord or Role.Loyalist => -1000d * belief.Lord - 100d * belief.Loyalist +
                100d * belief.Rebel + 55d * belief.Renegade,
            Role.Rebel => 100d * belief.Lord + 65d * belief.Loyalist -
                100d * belief.Rebel + 20d * belief.Renegade,
            Role.Renegade => GetIdentityRenegadeHostility(view, target, belief.Rebel, belief.Loyalist),
            _ => 0d
        };
        if (role == Role.Rebel && target.Role is null && belief.Rebel < .6d)
        {
            // The opening quota can make every unknown a slightly likely ally.
            // An ordinary Slash has zero printed play value, so a small positive
            // pressure value preserves tempo when the Lord cannot be reached.
            // Stronger public Rebel evidence still earns full ally protection.
            hostility = Math.Max(hostility, 5d);
        }
        if (role is Role.Lord or Role.Loyalist && target.Role is null && belief.Renegade > 0 &&
            view.Players.Where(player => player.IsAlive).All(player =>
                GetIdentityRoleProbability(view, player, Role.Rebel) < 1e-9))
        {
            // With the Rebels gone, unresolved Loyalist/Renegade mixtures must
            // not become permanent allies. Pressure the most plausible remaining
            // contenders equally if tied, without changing their role posterior
            // or naming one seat as a certainly known Renegade.
            var mostLikely = view.Players.Where(player => player.IsAlive && player.Role is null)
                .Select(player => GetIdentityRoleProbability(view, player, Role.Renegade))
                .DefaultIfEmpty(0d).Max();
            if (belief.Renegade >= mostLikely - 1e-9)
                hostility = Math.Max(hostility, 10d + belief.Renegade * 45d);
        }
        return hostility;
    }

    private void EnsureIdentityBeliefs(GameSnapshot view)
    {
        if (ReferenceEquals(view, _identityBeliefView) && _identityBeliefRevision == _identityObservationRevision)
            return;
        _identityBeliefView = view;
        _identityBeliefRevision = _identityObservationRevision;
        _identityBeliefs = [];
        var unknown = view.Players.Where(player => player.Role is null).ToArray();
        foreach (var player in view.Players.Where(player => player.Role is not null))
            _identityBeliefs[player.Seat] = ExactIdentityRole(player.Role!.Value);
        if (unknown.Length == 0) return;

        var composition = view.IdentityComposition;
        var remaining = composition is null ? null : new[]
            { composition.Lords, composition.Loyalists, composition.Rebels, composition.Renegades };
        if (remaining is not null)
        {
            if (remaining.Any(count => count < 0) || remaining.Sum(count => (long)count) != view.Players.Count)
                remaining = null;
            else
            {
                foreach (var player in view.Players.Where(player => player.Role is not null))
                {
                    var index = Array.IndexOf(IdentityRoles, player.Role!.Value);
                    if (index < 0) { remaining = null; break; }
                    remaining[index]--;
                }
                if (remaining?.Any(count => count < 0) == true) remaining = null;
            }
        }
        if (remaining is null || unknown.Length > 8)
        {
            foreach (var player in unknown) _identityBeliefs[player.Seat] = FallbackIdentityBelief(view, player);
            return;
        }

        // Exact role-count conditioning over at most eight unknown seats. Role
        // likelihoods never inspect player hands, general identities or hidden
        // engine state. Each complete assignment has the published composition.
        var likelihood = unknown.Select(player => IdentityLikelihood(view, player)).ToArray();
        var weighted = unknown.Select(_ => new double[IdentityRoles.Length]).ToArray();
        var assignment = new int[unknown.Length];
        var counts = remaining!;
        var priorCounts = counts.ToArray();
        var total = 0d;
        Enumerate(0, 1d);
        if (!(total > 0) || !double.IsFinite(total))
        {
            foreach (var player in unknown) _identityBeliefs[player.Seat] = FallbackIdentityBelief(view, player);
            return;
        }
        for (var seatIndex = 0; seatIndex < unknown.Length; seatIndex++)
        {
            var values = new double[IdentityRoles.Length];
            for (var roleIndex = 0; roleIndex < values.Length; roleIndex++)
            {
                // A small quota-prior mixture preserves every count marginal,
                // while preventing public behavioural evidence alone from being
                // treated as certain knowledge of a still-hidden role.
                values[roleIndex] = .9d * weighted[seatIndex][roleIndex] / total +
                    .1d * priorCounts[roleIndex] / unknown.Length;
            }
            _identityBeliefs[unknown[seatIndex].Seat] = new(values[0], values[1], values[2], values[3]);
        }

        void Enumerate(int index, double weight)
        {
            if (index == unknown.Length)
            {
                total += weight;
                for (var i = 0; i < assignment.Length; i++) weighted[i][assignment[i]] += weight;
                return;
            }
            for (var roleIndex = 0; roleIndex < IdentityRoles.Length; roleIndex++)
            {
                if (counts[roleIndex] == 0) continue;
                counts[roleIndex]--;
                assignment[index] = roleIndex;
                Enumerate(index + 1, weight * likelihood[index][roleIndex]);
                counts[roleIndex]++;
            }
        }
    }

    private double[] IdentityLikelihood(GameSnapshot view, PlayerSnapshot player)
    {
        var retrospective = 0d;
        foreach (var attack in _identityUnresolvedAttacks.Where(attack => attack.SourceSeat == player.Seat))
        {
            var nowKnownRole = view.Players.FirstOrDefault(target => target.Seat == attack.TargetSeat)?.Role;
            var signal = nowKnownRole switch { Role.Rebel => -.65d, Role.Loyalist => .75d, _ => 0d };
            retrospective += signal * Math.Pow(.97d, _identityObservationRevision - attack.Revision);
        }
        var suspicion = Math.Clamp(_rebelSuspicion.GetValueOrDefault(player.Seat) +
            Math.Clamp(retrospective, -2d, 2d), -6d, 6d);
        return [1d, Math.Exp(-suspicion * .6d), Math.Exp(suspicion * .65d), 1d];
    }

    private IdentityRolePosterior FallbackIdentityBelief(GameSnapshot view, PlayerSnapshot player)
    {
        var likelihood = IdentityLikelihood(view, player);
        var lordKnown = view.Players.Any(target => target.Role == Role.Lord);
        var prior = lordKnown ? new[] { 0d, .3d, .5d, .2d } : new[] { .1d, .27d, .45d, .18d };
        var weights = likelihood.Select((value, index) => value * prior[index]).ToArray();
        var total = weights.Sum();
        var values = weights.Select((value, index) => .9d * value / total + .1d * prior[index]).ToArray();
        return new(values[0], values[1], values[2], values[3]);
    }

    private static IdentityRolePosterior ExactIdentityRole(Role role) => new(
        role == Role.Lord ? 1d : 0d, role == Role.Loyalist ? 1d : 0d,
        role == Role.Rebel ? 1d : 0d, role == Role.Renegade ? 1d : 0d);
}
