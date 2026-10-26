# 朱然（移动版）

> 此版本与 `classic:zhu-ran` 技能规则等价，独立武将定义已删除。本文保留规则和素材来源；处理名单见 [武将技能去重](../GENERAL_RULES_EQUIVALENCE.md)。

核对日期：2026-10-25。普通移动身份 `mobile:zhu-ran`，吴、男性、4体力，完整复用 `classic:danshou`。

- [移动官网武将131](https://www.sanguosha.cn/index.php/pc/hero-detail-131.html)：已读取完整HTML。当前胆守是出牌阶段内按发动序数支付递增张数的牌，选择攻击范围内其他角色；第一档弃对方一牌，第二档由对方交一牌，第三档造成一点伤害，之后各档自己与目标各摸两张。玩法也逐项说明这四种收益。
- [移动 BWIKI 朱然](https://wiki.biligame.com/msgs/%E6%9C%B1%E7%84%B6)，页面更新于2026-01-10：当前普通朱然与官网一致，和界朱然、谋朱然分别列出，均不是受伤后终止回合的早期胆守。
- [官方2024-11-25技能描述规范调整公告1959](https://www.sanguosha.cn/index.php/pc/news-detail-1959.html)：包含胆守（标），明确此次为描述规范调整，不改变实际强度与效果。没有从这份公告推断额外的机制变更。

这里采用当前双源一致的规则，不采用历史“伤害后摸牌并终止结算”的版本。现有 `classic:danshou` 使用 `nextPhaseActivationOrdinal`，每个真实出牌阶段重新计算发动序数，手牌和装备均可支付；原生选择限制为持有者攻击范围内其他角色。四个完整分支已由现有 DSL 表达：第一档从目标手牌、装备或判定区实际弃牌，第二档由目标自己选择手牌或装备交出，第三档实际伤害，第四及以后自己和目标各原生摸两张。原代价的实体、阶段次数、付款孩子及返回均沿已有程序帧处理。

本人物只新增 Content 和官方画像，不增加移动版同义技能、Runtime、终止回合机制、pending 或重复 fixture。沿用 `skill executor resumes child resolution without repeating paid effects`、原生选牌付款、`Native draw attempt: sequential recipients and actual gain observers cold` 与共用动态阶段次数验证；不将已有界胆守结束阶段目标次数检查当作普通胆守的定义。

立绘为[官网当前原画53700](https://www.sanguosha.cn/storage/uploads/images/skins/53700.jpg)，530×750，保存为 `official-mobile-zhu-ran.jpg`，未裁剪、重采样或重绘。SHA-256：`f55ac7acbb0a118f22d4f6ec617ab0b51e11acbe0a45231ac8a96ea27552855a`。
