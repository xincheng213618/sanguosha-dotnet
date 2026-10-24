# 移动版界法正

核对日期：2026-10-25。人物 `boundary:fa-zheng-mobile`，蜀势力、男性、3体力，移动版独立界限突破版本。

## 来源

- [移动版官网：界·法正，编号134](https://www.sanguosha.cn/index.php/pc/hero-detail-134.html)。恩怨、眩惑正文完整：一次从他人获得至少两张真实牌可令其摸一；每点伤害独立令来源交一手牌或失去一体力；眩惑替换自己的摸牌，令另一人摸二，再以其实际攻击范围指定杀的对象，不用杀则获得其两张牌。
- [移动版 BWIKI：眩惑／界法正](https://wiki.biligame.com/msgs/%E7%9C%A9%E6%83%91/%E7%95%8C%E6%B3%95%E6%AD%A3)，2025-08-28编辑，同版完整描述一致。
- [三国杀官网2016年法正攻略](https://www.sanguosha.com/news/20161205_9021_5212)，两项技能与移动版当前正文相同。其说明强调一次性从同一角色获得两张牌，不能将不同次数累计；取的牌可以是手牌或装备，逐伤害点处理。
- [旧版法正 FAQ 镜像](https://sanguosha.fandom.com/zh/wiki/%E6%B3%95%E6%AD%A3?variant=zh-hans)明确回答法正在受令者攻击范围内时可指定自己。此为同文技能的旧 FAQ 镜像，不作为移动版现行正文的替代来源；也符合“另一名”相对于受令者、排除其自身的通常含义。

不采用 OL 界法正摸牌结束后先交两张牌、拒杀时私看全部手牌取二的版本；恩怨不要求交红色或红桃手牌。普通移动法正的治疗报恩、红桃交牌眩惑也不是本版。

## 实现与边界

主体完整复用现有 `classic:enyuan`、`classic:xuanhuo` 的 DSL 操作，使用独立移动版内容 ID。恩怨得牌按原生 `cardsGained/perSourceOwner` 的实际同源拥有者批次计数，门槛二张；牌堆补牌不当作从他人取得牌，也不累计不同得牌批次。伤害按 `perDamagePoint` 独立提供可选恩怨：来源决定交自己一张真实手牌，或原生失去一点体力；后者不是伤害，不套旧版限花色交牌规则。

眩惑是 `drawPhaseStarting` 的 replacement，自己的普通摸牌不并行执行。受益人先完成原生摸二及其孩子，再由法正选择其攻击范围内的另一角色。出杀沿既有真实 Slash 请求，保留受令者为使用者、真实原材料、转化与提供牌能力、原生目标合法性和 typed 子返回；不是直接制造伤害，也不要求杀造成伤害才算执行。拒绝或无法使用杀时，从原受益人的手牌／装备区盲选或公开选足至多两张真实牌，按原批同时取得，因而可正常接恩怨。不额外查看全部暗手牌，取牌数不足二张时只取现有牌。

本版明确要求“其攻击范围内”，不能把按杀的点数、特定实体、用牌特权等允许越距离使用某张杀的能力当成攻击范围增加。本批增加本人窄 helper，严格绑定移动版原眩惑摸牌替换帧、固定请求节点、原受令者和内容 hash；在建立候选、显示目标、冷恢复校验及真实付款前均使用实际攻击范围。其他技能来源继续委托原 helper，保留既有合法权限。法正本人与受令者不同且在其范围内时仍可作为受杀对象，不擅加第三个不同角色限制。

## 必要验证

仅增加一项新行为检查 `Mobile boundary Fa Zheng: actual attack range native Slash or atomic take Enyuan cold`：用真实十三点杀距离策略构造原远位资格，确认旧请求仍保留该能力而本版严格排除远位；再走实际原杀付款和拒绝取二触发恩怨两条原生路径，在摸牌孩子、目标选择、出杀付款及取牌游标作代表性冷恢复。纯复用的逐点恩怨和同源批次规则不复制通用静态定义／隐私矩阵。

相关既有回归为 `Participant hand actual requested Slash native AI paid gift`、`Participant hand program bound alcohol rescue typed return and cold`、`card movement programs compose atomic per-card and per-batch triggers with replay`，由整批统一验证。
