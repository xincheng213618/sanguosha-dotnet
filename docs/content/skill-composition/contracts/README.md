# Mechanism contracts

These are required contracts for changes to the named mechanisms and their
consumers, including Core, characters, activities, modes and tests. Read the
affected topic before editing; there is no requirement to read every topic.

[AGENTS.md](../../../../AGENTS.md) holds repository-wide working constraints.
[CAPABILITY_MAP.md](../CAPABILITY_MAP.md) describes the capability method and links
to mechanism topics. [RUNTIME.md](../RUNTIME.md) and
[RUNTIME_EXPRESSIONS.md](../RUNTIME_EXPRESSIONS.md) describe authoring syntax.

| When changing | Read |
| --- | --- |
| Command commits, events, privacy, frame continuation, delivery, trigger usage, target/payment binding, fact demand, dependency direction or catalog discovery | [Runtime boundaries](RUNTIME_BOUNDARIES.md) |
| Skill grants, upgrades, availability, restrictions, applied states, physical marks, hand limits, judgment attachments or generated equipment | [Skills and applied states](SKILLS_AND_STATES.md) |
| Use/target admission, quotas/debits, distance/range, directed rights, Slash requests, Zhangba materials or action-menu snapshots | [Card-use legality](USE_LEGALITY.md) |
| Actual-use/phase history, confirmation, native returns/targets, repeated effects, damage adjustments or HP recovery receipts | [Card-use history and resolution](USE_RESOLUTION.md) |
| Card representations, physical entities, claims, deliveries, retirement, acquisition, Give or exact movement requests | [Materials and movement](MATERIALS_AND_MOVEMENT.md) |
| Cursor suspension, response requirements/relations, card-action invalidation or public reveal windows/history | [Responses and visibility](RESPONSES_AND_VISIBILITY.md) |
| Formats, save compatibility, tests or artifact cleanup | [Development gates](../../../DEVELOPMENT_GATES.md) |

A change crossing several boundaries follows each relevant contract. For example,
an issued Slash that acquires its material may involve legality, native resolution
and material delivery; a public reveal can also require runtime privacy boundaries.

## Maintaining the contracts

- Keep the mechanism's current owner, semantics, exceptions and remaining gaps
  together in the relevant topic. Update it when any of those change. Use commit
  history for migration chronology, verification timings and size statistics.
- Keep exact distinctions that affect behavior: scope, sampling time, physical
  identity, source loss, owner death, payment and typed returns.
- Link to related topics and update the capability map when a method or link
  changes. Search headings or the named API to locate a contract.
- Add a topic when a distinct boundary needs its own navigation entry.

This organization follows OpenAI's guidance on
[project instructions](https://learn.chatgpt.com/docs/agent-configuration/agents-md)
and [task-relevant context](https://developers.openai.com/blog/rethinking-skills-and-prompts-for-gpt-6-astra):
keep common instructions concise and route detailed reading by the work being done.
